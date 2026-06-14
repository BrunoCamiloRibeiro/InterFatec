using Microsoft.EntityFrameworkCore;
using FabysUnha.Models;
using FabysUnha.Data;
using FabysUnha.Enums;
using FabysUnha.Models.SqlViews;

namespace FabysUnha.Repositories;

/// <summary>
/// Repositório responsável pelas operações de acesso a dados relacionadas aos Clientes.
/// Implementa a interface <see cref="IClientesRepository"/>.
/// </summary>
public class ClientesRepository : IClientesRepository
{
    private readonly AppDbContext _db;

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="ClientesRepository"/>.
    /// </summary>
    /// <param name="db">O contexto de banco de dados da aplicação.</param>
    public ClientesRepository(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Obtém todos os clientes cadastrados no sistema, ordenados por nome.
    /// </summary>
    /// <returns>Uma lista contendo todos os clientes e seus respectivos agendamentos.</returns>
    public async Task<IEnumerable<Clientes>> ObterTodosClientes()
    {
        // Busca a lista de clientes usando a view, sem rastreamento de estado para melhor performance
        var clientes = await _db.Set<ListaClientesView>()
            .AsNoTracking()
            .OrderBy(cliente => cliente.Nome)
            .ToListAsync();

        // Carrega todos os agendamentos previamente agrupados por cliente
        var agendamentosPorCliente = await CarregarAgendamentosPorClienteAsync();

        // Mapeia as views para as entidades de domínio, associando seus agendamentos
        return clientes.Select(cliente => CriarCliente(cliente, agendamentosPorCliente));
    }

    /// <summary>
    /// Obtém os detalhes de um cliente específico pelo seu identificador (ID).
    /// </summary>
    /// <param name="id">O identificador único do cliente.</param>
    /// <returns>O cliente encontrado, incluindo sua senha e agendamentos, ou nulo se não existir.</returns>
    public async Task<Clientes?> ObterClientePorId(int id)
    {
        // Busca o cliente específico na view
        var cliente = await _db.Set<ListaClientesView>()
            .AsNoTracking()
            .FirstOrDefaultAsync(cliente => cliente.Id == id);

        // Se o cliente não existir, retorna nulo imediatamente
        if (cliente == null)
            return null;

        // Carrega os agendamentos apenas do cliente específico
        var agendamentosPorCliente = await CarregarAgendamentosPorClienteAsync(id);
        var clienteRetorno = CriarCliente(cliente, agendamentosPorCliente);

        // Como a senha não está na view, precisamos buscá-la diretamente da tabela usando comandos ADO.NET
        var connection = _db.Database.GetDbConnection();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT senha FROM Pessoas WHERE id = @id";
        var param = command.CreateParameter();
        param.ParameterName = "@id";
        param.Value = id;
        command.Parameters.Add(param);

        // Abre a conexão de banco de dados, executa a consulta e lê o resultado
        await _db.Database.OpenConnectionAsync();
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            // Se a coluna não for nula, atribui a senha à entidade de retorno
            if (!reader.IsDBNull(0))
            {
                clienteRetorno.Senha = reader.GetString(0);
            }
        }
        await _db.Database.CloseConnectionAsync();

        return clienteRetorno;
    }

    /// <summary>
    /// Registra um novo cliente no banco de dados.
    /// </summary>
    /// <param name="cliente">O objeto contendo os dados do cliente.</param>
    public async Task RegistrarCliente(Clientes cliente)
    {
        // Executa uma stored procedure para inserir o cliente, com interpolação de strings segura (SQL Injection prevented)
        await _db.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_InsertCliente {cliente.Nome}, {cliente.Telefone}, {(int)cliente.Status}, {cliente.Senha}");
    }

    /// <summary>
    /// Atualiza as informações de um cliente já existente.
    /// </summary>
    /// <param name="cliente">O cliente com os dados atualizados.</param>
    public async Task AtualizarCliente(Clientes cliente)
    {
        // Executa uma stored procedure de atualização, passando os dados necessários
        await _db.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_UpdateCliente {cliente.Id}, {cliente.Nome}, {cliente.Telefone}, {(int)cliente.Status}, {cliente.Senha}");
    }

    /// <summary>
    /// Realiza a exclusão lógica do cliente, marcando seu status como Inativo.
    /// </summary>
    /// <param name="cliente">O cliente a ser inativado.</param>
    public async Task ExcluirCliente(Clientes cliente)
    {
        // Define o status como Inativo (Exclusão lógica - Soft Delete)
        cliente.Status = PessoaStatus.Inativo;
        
        // Chama a stored procedure para atualizar o status no banco de dados
        await _db.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_UpdateCliente {cliente.Id}, {cliente.Nome}, {cliente.Telefone}, {(int)cliente.Status}, {cliente.Senha}");
    }

    /// <summary>
    /// Obtém um cliente pelo seu número de telefone.
    /// </summary>
    /// <param name="telefone">O telefone cadastrado do cliente.</param>
    /// <returns>O cliente correspondente ou nulo se não houver.</returns>
    public async Task<Clientes?> ObterClientePorTelefone(string telefone)
    {
        // Busca o cliente na view através do filtro de telefone
        var cliente = await _db.Set<ListaClientesView>()
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Telefone == telefone);

        // Se não encontrar, retorna nulo
        if (cliente == null)
            return null;

        // Retorna a entidade montada com os dados básicos
        return new Clientes
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Telefone = cliente.Telefone,
            Status = (PessoaStatus)cliente.StatusId
        };
    }

    /// <summary>
    /// Carrega e agrupa os agendamentos para facilitar a vinculação a clientes.
    /// </summary>
    /// <param name="clienteId">Parâmetro opcional de ID do cliente para filtrar a busca.</param>
    /// <returns>Dicionário agrupado onde a chave é o ID do cliente e o valor é a lista de agendamentos dele.</returns>
    private async Task<Dictionary<int, List<Agendamentos>>> CarregarAgendamentosPorClienteAsync(int? clienteId = null)
    {
        // Monta a query inicial selecionando apenas os campos necessários (ID do cliente e Data)
        var query = _db.Agendamentos
            .AsNoTracking()
            .Select(agendamento => new { agendamento.ClienteId, agendamento.Data });

        // Aplica o filtro de cliente, se o ID for fornecido
        if (clienteId.HasValue)
            query = query.Where(agendamento => agendamento.ClienteId == clienteId.Value);

        var agendamentos = await query.ToListAsync();

        // Agrupa os agendamentos pelo ClienteId e converte em dicionário para buscas mais rápidas depois
        return agendamentos
            .GroupBy(agendamento => agendamento.ClienteId)
            .ToDictionary(
                grupo => grupo.Key,
                grupo => grupo
                    .Select(agendamento => new Agendamentos
                    {
                        ClienteId = grupo.Key,
                        Data = agendamento.Data
                    })
                    .ToList());
    }

    /// <summary>
    /// Instancia um objeto da classe de domínio <see cref="Clientes"/> a partir dos dados da view.
    /// </summary>
    /// <param name="cliente">A entidade visualizada com os dados básicos do cliente.</param>
    /// <param name="agendamentosPorCliente">O dicionário contendo todos os agendamentos já agrupados.</param>
    /// <returns>Uma nova instância da classe <see cref="Clientes"/>.</returns>
    private static Clientes CriarCliente(
        ListaClientesView cliente,
        IReadOnlyDictionary<int, List<Agendamentos>> agendamentosPorCliente)
    {
        // Recupera de forma segura a lista de agendamentos deste cliente
        agendamentosPorCliente.TryGetValue(cliente.Id, out var agendamentos);

        return new Clientes
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Telefone = cliente.Telefone,
            Status = (PessoaStatus)cliente.StatusId,
            // Associa a lista de agendamentos ou cria uma lista vazia caso não possua nenhum
            Agendamentos = agendamentos ?? new List<Agendamentos>()
        };
    }
}
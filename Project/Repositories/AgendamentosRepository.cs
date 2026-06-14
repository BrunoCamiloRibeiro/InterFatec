using Microsoft.EntityFrameworkCore;
using FabysUnha.Data; 
using FabysUnha.Models;
using FabysUnha.Enums;
using FabysUnha.Models.SqlViews;

namespace FabysUnha.Repositories;

/// <summary>
/// Repositório responsável pelas operações de acesso a dados relacionadas aos Agendamentos.
/// Implementa a interface <see cref="IAgendamentosRepository"/>.
/// </summary>
public class AgendamentosRepository : IAgendamentosRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="AgendamentosRepository"/>.
    /// </summary>
    /// <param name="context">O contexto de banco de dados da aplicação.</param>
    public AgendamentosRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtém todos os agendamentos registrados no banco de dados.
    /// </summary>
    /// <returns>Uma lista contendo todos os agendamentos.</returns>
    public async Task<IEnumerable<Agendamentos>> ObterTodosAgendamentos()
    {
        // Busca os dados básicos dos agendamentos sem rastreamento para melhor performance
        var agendamentos = await _context.Agendamentos
            .AsNoTracking()
            .Select(agendamento => new
            {
                agendamento.Nr,
                agendamento.ClienteId,
                agendamento.Data,
                agendamento.Status,
                agendamento.Total
            })
            .ToListAsync();

        // Carrega a visualização de clientes em um dicionário para busca rápida em memória
        var clientes = await _context.Set<ListaClientesView>()
            .AsNoTracking()
            .Select(cliente => new
            {
                cliente.Id,
                cliente.Nome,
                cliente.Telefone,
                cliente.StatusId
            })
            .ToDictionaryAsync(cliente => cliente.Id);

        // Agrupa os serviços agendados por número do agendamento, contando a quantidade
        var servicosPorAgendamento = await _context.Servicos_Agendados
            .AsNoTracking()
            .GroupBy(servico => servico.AgendamentoNr)
            .Select(grupo => new { Nr = grupo.Key, Quantidade = grupo.Count() })
            .ToDictionaryAsync(item => item.Nr, item => item.Quantidade);

        // Agrupa os produtos agendados por número do agendamento, contando a quantidade
        var produtosPorAgendamento = await _context.Produtos_Agendados
            .AsNoTracking()
            .GroupBy(produto => produto.AgendamentoNr)
            .Select(grupo => new { Nr = grupo.Key, Quantidade = grupo.Count() })
            .ToDictionaryAsync(item => item.Nr, item => item.Quantidade);

        // Mapeia os dados obtidos para as entidades reais, relacionando as informações carregadas em memória
        return agendamentos.Select(agendamento => new Agendamentos
        {
            Nr = agendamento.Nr,
            ClienteId = agendamento.ClienteId,
            Data = agendamento.Data,
            Status = agendamento.Status,
            Total = agendamento.Total,
            // Preenche os dados do cliente se o mesmo for encontrado no dicionário
            Cliente = clientes.TryGetValue(agendamento.ClienteId, out var cliente)
                ? new Clientes
                {
                    Id = cliente.Id,
                    Nome = cliente.Nome,
                    Telefone = cliente.Telefone,
                    Status = (PessoaStatus)cliente.StatusId
                }
                : null,
            // Cria instâncias vazias de serviços e produtos para representar a quantidade associada
            Servicos_Agendados = Enumerable.Range(0, servicosPorAgendamento.GetValueOrDefault(agendamento.Nr)).Select(_ => new Servicos_Agendados()).ToList(),
            Produtos_Agendados = Enumerable.Range(0, produtosPorAgendamento.GetValueOrDefault(agendamento.Nr)).Select(_ => new Produtos_Agendados()).ToList()
        }).ToList();
    }

    /// <summary>
    /// Obtém um agendamento específico com base no seu identificador (número).
    /// </summary>
    /// <param name="id">O identificador único do agendamento.</param>
    /// <returns>O agendamento encontrado ou nulo se não existir.</returns>
    // Mudar esses includes pra porra de um view dps
    public async Task<Agendamentos?> ObterAgendamentoPorId(int id)
    {
        // Busca o agendamento incluindo seus relacionamentos (serviços e produtos)
        var agendamento = await _context.Agendamentos
            .Include(a => a.Servicos_Agendados) 
                .ThenInclude(sa => sa.Servico)
            .Include(a => a.Produtos_Agendados)
                .ThenInclude(pa => pa.Produto)
            .Include(a => a.Produtos_Agendados)
                .ThenInclude(pa => pa.ServicoAgendado!)
                    .ThenInclude(sa => sa.Servico)
            .FirstOrDefaultAsync(a => a.Nr == id);

        // Se o agendamento não for encontrado, retorna nulo
        if (agendamento == null)
            return null;

        // Carrega e associa o cliente correspondente ao agendamento
        agendamento.Cliente = await CarregarClienteAsync(agendamento.ClienteId);

        // Busca os funcionários responsáveis por cada serviço agendado
        var funcionarios = await CarregarFuncionariosAsync(
            agendamento.Servicos_Agendados.Select(sa => sa.FuncionarioId));

        // Associa a instância do funcionário a cada serviço do agendamento
        foreach (var servicoAgendado in agendamento.Servicos_Agendados)
        {
            if (funcionarios.TryGetValue(servicoAgendado.FuncionarioId, out var funcionario))
                servicoAgendado.Funcionario = funcionario;
        }

        return agendamento;
    }

    /// <summary>
    /// Obtém uma lista de agendamentos associados a um cliente específico.
    /// </summary>
    /// <param name="clienteId">O identificador do cliente.</param>
    /// <returns>Uma lista de agendamentos pertencentes ao cliente.</returns>
    public async Task<IEnumerable<Agendamentos>> ObterAgendamentosPorCliente(int clienteId)
    {
        // Filtra os agendamentos pelo ID do cliente, ordenando por data decrescente
        var agendamentos = await _context.Agendamentos
            .Where(a => a.ClienteId == clienteId)
            .OrderByDescending(a => a.Data)
            .Include(a => a.Servicos_Agendados)
                .ThenInclude(sa => sa.Servico)
            .Include(a => a.Produtos_Agendados)
                .ThenInclude(pa => pa.Produto)
            .AsNoTracking()
            .ToListAsync();

        return agendamentos;
    }

    /// <summary>
    /// Cria um novo agendamento no banco de dados.
    /// </summary>
    /// <param name="agendamento">O objeto de agendamento contendo os dados a serem salvos.</param>
    public async Task CriarAgendamento(Agendamentos agendamento)
    {
        // Executa uma stored procedure para inserir o agendamento no banco
        var query = "EXEC sp_InsertAgendamento @Data, @Total, @Cliente_id, @Status";
        
        // A procedure retorna o número gerado para o novo agendamento
        var nrList = await _context.Database.SqlQueryRaw<int>(query, 
            new Microsoft.Data.SqlClient.SqlParameter("@Data", agendamento.Data),
            new Microsoft.Data.SqlClient.SqlParameter("@Total", agendamento.Total),
            new Microsoft.Data.SqlClient.SqlParameter("@Cliente_id", agendamento.ClienteId),
            new Microsoft.Data.SqlClient.SqlParameter("@Status", (int)agendamento.Status)
        ).ToListAsync();

        // Obtém o número (ID) retornado pela consulta
        var nrGerado = nrList.FirstOrDefault();

        // Lança uma exceção se a inserção falhar e nenhum ID for retornado
        if (nrGerado == 0) throw new Exception("Falha ao criar agendamento via procedure.");

        // Atualiza a entidade com o número gerado no banco de dados
        agendamento.Nr = nrGerado;

        // Insere os serviços associados ao agendamento utilizando stored procedure
        foreach (var sa in agendamento.Servicos_Agendados)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_InsertServicoAgendado @Agendamento_nr, @Servico_id, @Obs, @Horario, @Funcionario_id, @Valor",
                new Microsoft.Data.SqlClient.SqlParameter("@Agendamento_nr", agendamento.Nr),
                new Microsoft.Data.SqlClient.SqlParameter("@Servico_id", sa.ServicoId),
                new Microsoft.Data.SqlClient.SqlParameter("@Obs", sa.Obs ?? (object)DBNull.Value),
                new Microsoft.Data.SqlClient.SqlParameter("@Horario", sa.Horario),
                new Microsoft.Data.SqlClient.SqlParameter("@Funcionario_id", sa.FuncionarioId),
                new Microsoft.Data.SqlClient.SqlParameter("@Valor", sa.Valor)
            );
        }

        // Insere os produtos consumidos no agendamento também via stored procedure
        foreach (var pa in agendamento.Produtos_Agendados)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_InsertProdutoAgendado @Agendamento_nr, @Servico_id, @Produto_codigo, @Preco",
                new Microsoft.Data.SqlClient.SqlParameter("@Agendamento_nr", agendamento.Nr),
                new Microsoft.Data.SqlClient.SqlParameter("@Servico_id", pa.ServicoId),
                new Microsoft.Data.SqlClient.SqlParameter("@Produto_codigo", pa.ProdutoCodigo),
                new Microsoft.Data.SqlClient.SqlParameter("@Preco", pa.Preco)
            );
        }
    }

    /// <summary>
    /// Atualiza as informações de um agendamento existente.
    /// </summary>
    /// <param name="agendamento">O agendamento com as informações atualizadas.</param>
    public async Task AtualizarAgendamento(Agendamentos agendamento)
    {
        // Atualiza os dados principais do agendamento usando uma procedure
        await _context.Database.ExecuteSqlRawAsync(
            "EXEC sp_UpdateAgendamento @Nr, @Data, @Total, @Cliente_id, @Status",
            new Microsoft.Data.SqlClient.SqlParameter("@Nr", agendamento.Nr),
            new Microsoft.Data.SqlClient.SqlParameter("@Data", agendamento.Data),
            new Microsoft.Data.SqlClient.SqlParameter("@Total", agendamento.Total),
            new Microsoft.Data.SqlClient.SqlParameter("@Cliente_id", agendamento.ClienteId),
            new Microsoft.Data.SqlClient.SqlParameter("@Status", (int)agendamento.Status)
        );

        // Remove os produtos e serviços antigos associados ao agendamento
        // Isso é necessário pois é mais simples recriar os filhos do que comparar diferenças
        await _context.Database.ExecuteSqlRawAsync(
            "DELETE FROM Produtos_Agendados WHERE agendamento_nr = @Nr",
            new Microsoft.Data.SqlClient.SqlParameter("@Nr", agendamento.Nr)
        );
        await _context.Database.ExecuteSqlRawAsync(
            "DELETE FROM Servicos_Agendados WHERE agendamento_nr = @Nr",
            new Microsoft.Data.SqlClient.SqlParameter("@Nr", agendamento.Nr)
        );

        // Insere novamente os serviços atualizados
        foreach (var sa in agendamento.Servicos_Agendados)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_InsertServicoAgendado @Agendamento_nr, @Servico_id, @Obs, @Horario, @Funcionario_id, @Valor",
                new Microsoft.Data.SqlClient.SqlParameter("@Agendamento_nr", agendamento.Nr),
                new Microsoft.Data.SqlClient.SqlParameter("@Servico_id", sa.ServicoId),
                new Microsoft.Data.SqlClient.SqlParameter("@Obs", sa.Obs ?? (object)DBNull.Value),
                new Microsoft.Data.SqlClient.SqlParameter("@Horario", sa.Horario),
                new Microsoft.Data.SqlClient.SqlParameter("@Funcionario_id", sa.FuncionarioId),
                new Microsoft.Data.SqlClient.SqlParameter("@Valor", sa.Valor)
            );
        }

        // Insere novamente os produtos atualizados
        foreach (var pa in agendamento.Produtos_Agendados)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_InsertProdutoAgendado @Agendamento_nr, @Servico_id, @Produto_codigo, @Preco",
                new Microsoft.Data.SqlClient.SqlParameter("@Agendamento_nr", agendamento.Nr),
                new Microsoft.Data.SqlClient.SqlParameter("@Servico_id", pa.ServicoId),
                new Microsoft.Data.SqlClient.SqlParameter("@Produto_codigo", pa.ProdutoCodigo),
                new Microsoft.Data.SqlClient.SqlParameter("@Preco", pa.Preco)
            );
        }
    }

    /// <summary>
    /// Exclui um agendamento do banco de dados, assim como seus relacionamentos.
    /// </summary>
    /// <param name="agendamento">O agendamento a ser excluído.</param>
    public async Task ExcluirAgendamento(Agendamentos agendamento)
    {
        // Deleta os relacionamentos na tabela Produtos_Agendados primeiro para evitar erro de Foreign Key
        await _context.Database.ExecuteSqlRawAsync(
            "DELETE FROM Produtos_Agendados WHERE agendamento_nr = @Nr",
            new Microsoft.Data.SqlClient.SqlParameter("@Nr", agendamento.Nr)
        );
        
        // Deleta os relacionamentos na tabela Servicos_Agendados
        await _context.Database.ExecuteSqlRawAsync(
            "DELETE FROM Servicos_Agendados WHERE agendamento_nr = @Nr",
            new Microsoft.Data.SqlClient.SqlParameter("@Nr", agendamento.Nr)
        );
        
        // Por fim, deleta o registro na tabela principal de Agendamentos
        await _context.Database.ExecuteSqlRawAsync(
            "DELETE FROM Agendamentos WHERE nr = @Nr",
            new Microsoft.Data.SqlClient.SqlParameter("@Nr", agendamento.Nr)
        );
    }

    /// <summary>
    /// Obtém uma lista de horários já ocupados de um funcionário em uma data específica.
    /// </summary>
    /// <param name="funcionarioId">O identificador do funcionário.</param>
    /// <param name="data">A data a ser verificada.</param>
    /// <returns>Uma lista de <see cref="TimeSpan"/> representando os horários indisponíveis.</returns>
    public async Task<List<TimeSpan>> ObterHorariosOcupados(int funcionarioId, DateTime data)
    {
        // Define o início e fim do dia desejado para buscar apenas horários daquele dia
        var dataInicio = data.Date;
        var dataFim = dataInicio.AddDays(1);

        // Consulta a junção entre Serviços Agendados e Agendamentos 
        // para extrair apenas os horários ocupados deste funcionário no dia especificado
        return await (from servicoAgendado in _context.Servicos_Agendados.AsNoTracking()
                      join agendamento in _context.Agendamentos.AsNoTracking()
                          on servicoAgendado.AgendamentoNr equals agendamento.Nr
                      where servicoAgendado.FuncionarioId == funcionarioId
                          && agendamento.Data >= dataInicio
                          && agendamento.Data < dataFim
                          // Ignora agendamentos que já foram cancelados (status livre)
                          && agendamento.Status != Enums.AgendamentoStatus.Cancelado
                      select servicoAgendado.Horario)
            .Distinct()
            .OrderBy(h => h)
            .ToListAsync();
    }

    /// <summary>
    /// Carrega as informações básicas de um cliente através de uma view.
    /// </summary>
    /// <param name="clienteId">ID do cliente a ser carregado.</param>
    /// <returns>Um objeto <see cref="Clientes"/> contendo os dados do cliente ou null se não existir.</returns>
    private async Task<Clientes?> CarregarClienteAsync(int clienteId)
    {
        // Utiliza uma view (ListaClientesView) em vez da tabela real para uma consulta mais simples
        var clienteView = await _context.Set<ListaClientesView>()
            .AsNoTracking()
            .FirstOrDefaultAsync(cliente => cliente.Id == clienteId);

        // Se o cliente não existir na base de dados, retorna nulo imediatamente
        if (clienteView == null)
            return null;

        // Mapeia os dados da view para o objeto do domínio
        return new Clientes
        {
            Id = clienteView.Id,
            Nome = clienteView.Nome,
            Telefone = clienteView.Telefone,
            Status = (PessoaStatus)clienteView.StatusId
        };
    }

    /// <summary>
    /// Carrega múltiplos funcionários de uma vez com base em uma lista de identificadores.
    /// </summary>
    /// <param name="funcionariosIds">Lista de IDs dos funcionários.</param>
    /// <returns>Um dicionário associando o ID do funcionário à sua respectiva instância para fácil localização.</returns>
    private async Task<Dictionary<int, Funcionarios>> CarregarFuncionariosAsync(IEnumerable<int> funcionariosIds)
    {
        // Remove IDs duplicados para evitar buscas redundantes no banco
        var ids = funcionariosIds.Distinct().ToList();

        // Retorna um dicionário vazio se não houver IDs a serem processados
        if (ids.Count == 0)
            return new Dictionary<int, Funcionarios>();

        // Busca os funcionários pela view correspondente
        var funcionariosView = await _context.Set<ListaFuncionariosView>()
            .AsNoTracking()
            .Where(funcionario => ids.Contains(funcionario.Id))
            .ToListAsync();

        // Mapeia o resultado e converte para dicionário para facilitar o acesso por chave (ID)
        return funcionariosView.ToDictionary(
            funcionario => funcionario.Id,
            funcionario => new Funcionarios
            {
                Id = funcionario.Id,
                Nome = funcionario.Nome ?? string.Empty,
                Telefone = funcionario.Telefone ?? string.Empty,
                Status = (PessoaStatus)funcionario.StatusId,
                Salario = funcionario.Salario,
                // Cria o objeto de especialidade caso o funcionário tenha uma
                Especialidade = !string.IsNullOrEmpty(funcionario.Especialidade)
                    ? new Especialidades { Descricao = funcionario.Especialidade }
                    : null
            });
    }
}
using Microsoft.EntityFrameworkCore;
using FabysUnha.Data;
using FabysUnha.Enums;
using FabysUnha.Models;
using FabysUnha.Services.Interfaces;

namespace FabysUnha.Services;

/// <summary>
/// Serviço de Autenticação responsável por realizar login de clientes e funcionários de maneira segura.
/// </summary>
public class LoginAuthService : ILoginAuthService
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Construtor da classe <see cref="LoginAuthService"/> com injeção do contexto de banco de dados.
    /// </summary>
    /// <param name="context">O contexto do Entity Framework para manipulação de dados.</param>
    public LoginAuthService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Autentica um cliente baseado no seu número de telefone e senha.
    /// </summary>
    /// <param name="telefone">Telefone do cliente, usado como login.</param>
    /// <param name="senha">Senha fornecida pelo usuário no ato do login.</param>
    /// <returns>Uma tupla informando se o login foi válido, além de retornar os dados do cliente e seus agendamentos.</returns>
    public async Task<(bool Valido, Clientes? Cliente, List<Agendamentos>? Agendamentos)> AutenticarClientePorTelefoneESenha(
        string telefone, 
        string senha)
    {
        // Validação inicial para evitar consultas desnecessárias se os parâmetros estiverem vazios.
        if (string.IsNullOrWhiteSpace(telefone) || string.IsNullOrWhiteSpace(senha))
            return (false, null, null);

        // Obtém a conexão do banco de dados diretamente pelo Entity Framework para executar um comando SQL puro.
        var connection = _context.Database.GetDbConnection();
        await connection.OpenAsync();

        // Cria o comando para realizar a leitura das informações do cliente.
        using var command = connection.CreateCommand();
        // A consulta faz um JOIN entre Pessoas e Clientes filtrando pelo telefone e garantindo que o status seja ativo (0).
        command.CommandText = @"SELECT p.id, p.Nome, p.Telefone, p.status, p.senha
                                FROM Pessoas AS p
                                INNER JOIN Clientes AS c ON p.id = c.pessoa_id
                                WHERE p.Telefone = @telefone AND p.status = 0";

        // Criação e configuração do parâmetro seguro para prevenir SQL Injection.
        var telefoneParam = command.CreateParameter();
        telefoneParam.ParameterName = "@telefone";
        telefoneParam.Value = telefone.Trim();
        command.Parameters.Add(telefoneParam);

        // Executa a leitura no banco de dados.
        using var reader = await command.ExecuteReaderAsync();
        
        // Se não encontrar nenhum registro correspondente ao telefone, encerra o fluxo e retorna falso.
        if (!await reader.ReadAsync())
            return (false, null, null);

        // Recupera a senha que está armazenada no banco.
        var dbSenhaHash = reader.GetString(4);
        bool senhaCorreta = false;
        
        // Verifica se a senha recuperada do banco de dados está em formato de hash (BCrypt).
        // Isso é feito checando o tamanho e o prefixo do hash gerado pelo BCrypt.
        if (dbSenhaHash.Length == 60 && (dbSenhaHash.StartsWith("$2a$") || dbSenhaHash.StartsWith("$2b$") || dbSenhaHash.StartsWith("$2x$") || dbSenhaHash.StartsWith("$2y$")))
        {
            // Compara a senha informada (em texto plano) com o hash criptografado utilizando a biblioteca BCrypt.
            senhaCorreta = BCrypt.Net.BCrypt.Verify(senha, dbSenhaHash);
        }
        else
        {
            // Para casos de sistemas legados ou senhas não criptografadas, faz comparação direta.
            senhaCorreta = (senha == dbSenhaHash);
        }

        // Se as senhas não conferem, o login falhou.
        if (!senhaCorreta)
            return (false, null, null);

        // Como a autenticação teve sucesso, popula o objeto cliente com os dados retornados do DataReader.
        var cliente = new Clientes
        {
            Id = reader.GetInt32(0),
            Nome = reader.GetString(1),
            Telefone = reader.GetString(2),
            Status = (PessoaStatus)reader.GetInt32(3),
            Senha = reader.GetString(4)
        };
        // Libera o reader pois já extraímos o que era necessário.
        await reader.CloseAsync();

        // Busca o histórico de agendamentos deste cliente utilizando Entity Framework/LINQ.
        var agendamentos = await _context.Agendamentos
            .Where(a => a.ClienteId == cliente.Id)
            .OrderByDescending(a => a.Data) // Ordena dos mais recentes para os mais antigos
            .ToListAsync();

        // Retorna o sucesso junto com os objetos carregados.
        return (true, cliente, agendamentos);
    }

    /// <summary>
    /// Autentica um funcionário no sistema usando telefone e senha.
    /// </summary>
    /// <param name="telefone">O telefone de acesso do funcionário.</param>
    /// <param name="senha">A senha fornecida para o acesso.</param>
    /// <returns>Retorna a entidade do <see cref="Funcionarios"/> caso as credenciais estejam corretas, ou null caso contrário.</returns>
    public async Task<Funcionarios?> AutenticarFuncionario(string telefone, string senha)
    {
        // Verifica parâmetros inválidos rapidamente.
        if (string.IsNullOrWhiteSpace(telefone) || string.IsNullOrWhiteSpace(senha))
            return null;

        // Abre a conexão bruta com o banco de dados.
        var connection = _context.Database.GetDbConnection();
        await connection.OpenAsync();

        // Prepara o comando SQL puro que seleciona dados da pessoa e da tabela associativa do funcionário.
        using var command = connection.CreateCommand();
        command.CommandText = @"SELECT p.id, p.Nome, p.Telefone, p.status, p.senha, f.salario, f.especialidade_id
                                FROM Pessoas AS p
                                INNER JOIN Funcionarios AS f ON p.id = f.pessoa_id
                                WHERE p.Telefone = @telefone AND p.status = 0";

        // Parametriza a query para o telefone.
        var telefoneParam = command.CreateParameter();
        telefoneParam.ParameterName = "@telefone";
        telefoneParam.Value = telefone.Trim();
        command.Parameters.Add(telefoneParam);

        // Executa a leitura.
        using var reader = await command.ExecuteReaderAsync();
        
        // Se o banco não encontrar correspondência para o telefone, retorna nulo.
        if (!await reader.ReadAsync())
            return null;

        // Obtém o hash ou a senha plana registrada.
        var dbSenhaHash = reader.GetString(4);
        bool senhaCorreta = false;
        
        // Aplica a validação de formato do BCrypt.
        if (dbSenhaHash.Length == 60 && (dbSenhaHash.StartsWith("$2a$") || dbSenhaHash.StartsWith("$2b$") || dbSenhaHash.StartsWith("$2x$") || dbSenhaHash.StartsWith("$2y$")))
        {
            // Utiliza o Verify para checar a correspondência de hash.
            senhaCorreta = BCrypt.Net.BCrypt.Verify(senha, dbSenhaHash);
        }
        else
        {
            // Compara em texto plano (fallback para senhas não seguras).
            senhaCorreta = (senha == dbSenhaHash);
        }

        // Retorna nulo se o password estiver incorreto.
        if (!senhaCorreta)
            return null;

        // Popula os dados do funcionário lidos na base.
        var funcionario = new Funcionarios
        {
            Id = reader.GetInt32(0),
            Nome = reader.GetString(1),
            Telefone = reader.GetString(2),
            Status = (PessoaStatus)reader.GetInt32(3),
            Senha = reader.GetString(4),
            Salario = reader.GetDecimal(5),
            // Trata o campo especialidade que pode ser nulo na base (IsDBNull).
            EspecialidadeId = reader.IsDBNull(6) ? null : reader.GetInt32(6)
        };

        return funcionario;
    }

    /// <summary>
    /// Busca a lista de agendamentos de um cliente específico com base no seu telefone e ID para validação extra.
    /// </summary>
    /// <param name="telefone">Telefone do cliente, servindo como uma chave de verificação.</param>
    /// <param name="clienteId">ID único do cliente.</param>
    /// <returns>Lista de <see cref="Agendamentos"/> associada, ou nulo se os dados não baterem.</returns>
    public async Task<List<Agendamentos>?> ObterAgendamentosCliente(string telefone, int clienteId)
    {
        var connection = _context.Database.GetDbConnection();
        await connection.OpenAsync();

        // Consulta de segurança para garantir que o telefone e o ID passados realmente correspondem ao mesmo cliente.
        using var command = connection.CreateCommand();
        command.CommandText = @"SELECT p.id
                                FROM Pessoas AS p
                                INNER JOIN Clientes AS c ON p.id = c.pessoa_id
                                WHERE p.Telefone = @telefone AND p.id = @clienteId";

        // Parâmetro do telefone.
        var telefoneParam = command.CreateParameter();
        telefoneParam.ParameterName = "@telefone";
        telefoneParam.Value = telefone.Trim();
        command.Parameters.Add(telefoneParam);

        // Parâmetro do ID do cliente.
        var clienteIdParam = command.CreateParameter();
        clienteIdParam.ParameterName = "@clienteId";
        clienteIdParam.Value = clienteId;
        command.Parameters.Add(clienteIdParam);

        // Avalia se o registro de correspondência exata existe.
        using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            // Se as credenciais não baterem, recusa a busca dos agendamentos retornando nulo.
            return null;
        }
        await reader.CloseAsync();

        // Obtém a lista dos agendamentos pelo Entity Framework ordenados da data mais recente para a mais antiga.
        var agendamentos = await _context.Agendamentos
            .Where(a => a.ClienteId == clienteId)
            .OrderByDescending(a => a.Data)
            .ToListAsync();

        return agendamentos;
    }
}

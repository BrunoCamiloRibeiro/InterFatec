using Microsoft.EntityFrameworkCore;
using FabysUnha.Data;
using FabysUnha.Enums;
using FabysUnha.Models;
using FabysUnha.Services.Interfaces;

namespace FabysUnha.Services;

public class LoginAuthService : ILoginAuthService
{
    private readonly AppDbContext _context;

    public LoginAuthService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(bool Valido, Clientes? Cliente, List<Agendamentos>? Agendamentos)> AutenticarClientePorTelefoneESenha(
        string telefone, 
        string senha)
    {
        if (string.IsNullOrWhiteSpace(telefone) || string.IsNullOrWhiteSpace(senha))
            return (false, null, null);

        var connection = _context.Database.GetDbConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"SELECT p.id, p.Nome, p.Telefone, p.status, p.senha
                                FROM Pessoas AS p
                                INNER JOIN Clientes AS c ON p.id = c.pessoa_id
                                WHERE p.Telefone = @telefone";

        var telefoneParam = command.CreateParameter();
        telefoneParam.ParameterName = "@telefone";
        telefoneParam.Value = telefone.Trim();
        command.Parameters.Add(telefoneParam);

        using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return (false, null, null);

        var dbSenhaHash = reader.GetString(4);
        bool senhaCorreta = false;
        if (dbSenhaHash.Length == 60 && (dbSenhaHash.StartsWith("$2a$") || dbSenhaHash.StartsWith("$2b$") || dbSenhaHash.StartsWith("$2x$") || dbSenhaHash.StartsWith("$2y$")))
            senhaCorreta = BCrypt.Net.BCrypt.Verify(senha, dbSenhaHash);
        else
            senhaCorreta = (senha == dbSenhaHash);

        if (!senhaCorreta)
            return (false, null, null);

        var cliente = new Clientes
        {
            Id = reader.GetInt32(0),
            Nome = reader.GetString(1),
            Telefone = reader.GetString(2),
            Status = (PessoaStatus)reader.GetInt32(3),
            Senha = reader.GetString(4)
        };
        await reader.CloseAsync();

        var agendamentos = await _context.Agendamentos
            .Where(a => a.ClienteId == cliente.Id)
            .OrderByDescending(a => a.Data)
            .ToListAsync();

        return (true, cliente, agendamentos);
    }

    public async Task<Funcionarios?> AutenticarFuncionario(string telefone, string senha)
    {
        if (string.IsNullOrWhiteSpace(telefone) || string.IsNullOrWhiteSpace(senha))
            return null;

        var connection = _context.Database.GetDbConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"SELECT p.id, p.Nome, p.Telefone, p.status, p.senha, f.salario, f.especialidade_id
                                FROM Pessoas AS p
                                INNER JOIN Funcionarios AS f ON p.id = f.pessoa_id
                                WHERE p.Telefone = @telefone";

        var telefoneParam = command.CreateParameter();
        telefoneParam.ParameterName = "@telefone";
        telefoneParam.Value = telefone.Trim();
        command.Parameters.Add(telefoneParam);

        using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return null;

        var dbSenhaHash = reader.GetString(4);
        bool senhaCorreta = false;
        if (dbSenhaHash.Length == 60 && (dbSenhaHash.StartsWith("$2a$") || dbSenhaHash.StartsWith("$2b$") || dbSenhaHash.StartsWith("$2x$") || dbSenhaHash.StartsWith("$2y$")))
            senhaCorreta = BCrypt.Net.BCrypt.Verify(senha, dbSenhaHash);
        else
            senhaCorreta = (senha == dbSenhaHash);

        if (!senhaCorreta)
            return null;

        var funcionario = new Funcionarios
        {
            Id = reader.GetInt32(0),
            Nome = reader.GetString(1),
            Telefone = reader.GetString(2),
            Status = (PessoaStatus)reader.GetInt32(3),
            Senha = reader.GetString(4),
            Salario = reader.GetDecimal(5),
            EspecialidadeId = reader.IsDBNull(6) ? null : reader.GetInt32(6)
        };

        return funcionario;
    }

    public async Task<List<Agendamentos>?> ObterAgendamentosCliente(string telefone, int clienteId)
    {
        var connection = _context.Database.GetDbConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"SELECT p.id
                                FROM Pessoas AS p
                                INNER JOIN Clientes AS c ON p.id = c.pessoa_id
                                WHERE p.Telefone = @telefone AND p.id = @clienteId";

        var telefoneParam = command.CreateParameter();
        telefoneParam.ParameterName = "@telefone";
        telefoneParam.Value = telefone.Trim();
        command.Parameters.Add(telefoneParam);

        var clienteIdParam = command.CreateParameter();
        clienteIdParam.ParameterName = "@clienteId";
        clienteIdParam.Value = clienteId;
        command.Parameters.Add(clienteIdParam);

        using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return null;
        await reader.CloseAsync();

        var agendamentos = await _context.Agendamentos
            .Where(a => a.ClienteId == clienteId)
            .OrderByDescending(a => a.Data)
            .ToListAsync();

        return agendamentos;
    }
}

using Microsoft.EntityFrameworkCore;
using FabysUnha.Data;
using FabysUnha.Enums;
using FabysUnha.Models;
using FabysUnha.Services.Interfaces;

namespace FabysUnha.Services;

public class ClienteAuthService : IClienteAuthService
{
    private readonly AppDbContext _context;

    public ClienteAuthService(AppDbContext context)
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
                                WHERE p.Telefone = @telefone AND p.senha = @senha";

        var telefoneParam = command.CreateParameter();
        telefoneParam.ParameterName = "@telefone";
        telefoneParam.Value = telefone.Trim();
        command.Parameters.Add(telefoneParam);

        var senhaParam = command.CreateParameter();
        senhaParam.ParameterName = "@senha";
        senhaParam.Value = senha;
        command.Parameters.Add(senhaParam);

        using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return (false, null, null);

        var cliente = new Clientes
        {
            Id = reader.GetInt32(0),
            Nome = reader.GetString(1),
            Telefone = reader.GetString(2),
            Status = (PessoaStatus)reader.GetInt32(3),
            Senha = reader.GetString(4)
        };

        var agendamentos = await _context.Agendamentos
            .Where(a => a.ClienteId == cliente.Id)
            .OrderByDescending(a => a.Data)
            .ToListAsync();

        return (true, cliente, agendamentos);
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

        var agendamentos = await _context.Agendamentos
            .Where(a => a.ClienteId == clienteId)
            .OrderByDescending(a => a.Data)
            .ToListAsync();

        return agendamentos;
    }
}

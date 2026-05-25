using Microsoft.EntityFrameworkCore;
using FabysUnha.Models;
using FabysUnha.Data;
using FabysUnha.Enums;
using FabysUnha.Models.SqlViews;

namespace FabysUnha.Repositories;

public class ClientesRepository : IClientesRepository
{
    private readonly AppDbContext _db;

    public ClientesRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Clientes>> ObterTodosClientes()
    {
        var clientes = await _db.Set<ListaClientesView>()
            .AsNoTracking()
            .OrderBy(cliente => cliente.Nome)
            .ToListAsync();

        var agendamentosPorCliente = await CarregarAgendamentosPorClienteAsync();

        return clientes.Select(cliente => CriarCliente(cliente, agendamentosPorCliente));
    }

    public async Task<Clientes?> ObterClientePorId(int id)
    {
        var cliente = await _db.Set<ListaClientesView>()
            .AsNoTracking()
            .FirstOrDefaultAsync(cliente => cliente.Id == id);

        if (cliente == null)
            return null;

        var agendamentosPorCliente = await CarregarAgendamentosPorClienteAsync(id);
        return CriarCliente(cliente, agendamentosPorCliente);
    }

    public async Task RegistrarCliente(Clientes cliente)
    {
        await _db.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_InsertCliente {cliente.Nome}, {cliente.Telefone}, {(int)cliente.Status}");
    }

    public async Task AtualizarCliente(Clientes cliente)
    {
        await _db.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_UpdateCliente {cliente.Id}, {cliente.Nome}, {cliente.Telefone}, {(int)cliente.Status}");
    }

    public async Task ExcluirCliente(Clientes cliente)
    {
        _db.Clientes.Remove(cliente);
        await _db.SaveChangesAsync();
    }

    public async Task<Clientes?> ObterClientePorTelefone(string telefone)
    {
        var cliente = await _db.Set<ListaClientesView>()
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Telefone == telefone);

        if (cliente == null)
            return null;

        return new Clientes
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Telefone = cliente.Telefone,
            Status = (PessoaStatus)cliente.StatusId
        };
    }

    private async Task<Dictionary<int, List<Agendamentos>>> CarregarAgendamentosPorClienteAsync(int? clienteId = null)
    {
        var query = _db.Agendamentos
            .AsNoTracking()
            .Select(agendamento => new { agendamento.ClienteId, agendamento.Data });

        if (clienteId.HasValue)
            query = query.Where(agendamento => agendamento.ClienteId == clienteId.Value);

        var agendamentos = await query.ToListAsync();

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

    private static Clientes CriarCliente(
        ListaClientesView cliente,
        IReadOnlyDictionary<int, List<Agendamentos>> agendamentosPorCliente)
    {
        agendamentosPorCliente.TryGetValue(cliente.Id, out var agendamentos);

        return new Clientes
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Telefone = cliente.Telefone,
            Status = (PessoaStatus)cliente.StatusId,
            Agendamentos = agendamentos ?? new List<Agendamentos>()
        };
    }
}
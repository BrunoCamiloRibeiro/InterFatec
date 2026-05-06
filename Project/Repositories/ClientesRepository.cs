using Microsoft.EntityFrameworkCore;
using FabysUnha.Models;
using FabysUnha.Data;

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
        return await _db.Clientes.ToListAsync();
    }

    public async Task<Clientes?> ObterClientePorId(int id)
    {
        return await _db.Clientes.FindAsync(id);
    }

    public async Task RegistrarCliente(Clientes cliente)
    {
        _db.Clientes.Add(cliente);
        await _db.SaveChangesAsync();
    }

    public async Task AtualizarCliente(Clientes cliente)
    {
        _db.Clientes.Update(cliente);
        await _db.SaveChangesAsync();
    }

    public async Task ExcluirCliente(Clientes cliente)
    {
        _db.Clientes.Remove(cliente);
        await _db.SaveChangesAsync();
    }
}
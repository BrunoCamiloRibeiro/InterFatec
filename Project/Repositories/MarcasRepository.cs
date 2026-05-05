using Microsoft.EntityFrameworkCore;
using FabysUnha.Models;
using FabysUnha.Data;

namespace FabysUnha.Repositories;

public class MarcasRepository : IMarcasRepository
{
    private readonly AppDbContext _db;

    public MarcasRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Marcas>> ObterTodasMarcas()
    {
        return await _db.Marcas.ToListAsync();
    }

    public async Task<Marcas?> ObterMarcaPorId(int id)
    {
        return await _db.Marcas.FindAsync(id);
    }

    public async Task CriarMarca(Marcas marca)
    {
        _db.Marcas.Add(marca);
        await _db.SaveChangesAsync();
    }

    public async Task AtualizarMarca(Marcas marca)
    {
        _db.Marcas.Update(marca);
        await _db.SaveChangesAsync();
    }

    public async Task ExcluirMarca(Marcas marca)
    {
        _db.Marcas.Remove(marca);
        await _db.SaveChangesAsync();
    }
    
}

    
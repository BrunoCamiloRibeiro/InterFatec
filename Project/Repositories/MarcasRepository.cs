using Microsoft.EntityFrameworkCore;
using FabysUnha.Models;
using FabysUnha.Data;
using FabysUnha.Enums;
using FabysUnha.Models.SqlViews;

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
        var marcasView = await _db.Set<ListaMarcasView>()
            .AsNoTracking()
            .OrderBy(marca => marca.Nome)
            .ToListAsync();

        return marcasView.Select(marca => new Marcas
        {
            Id = marca.Id,
            Nome = marca.Nome,
            Status = (MarcaStatus)marca.Status
        }).ToList();
    }

    public async Task<Marcas?> ObterMarcaPorId(int id)
    {
        return await _db.Marcas.FindAsync(id);
    }

    public async Task CriarMarca(Marcas marca)
    {
        await _db.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_InsertMarca {marca.Nome}, {(int)marca.Status}");
    }

    public async Task AtualizarMarca(Marcas marca)
    {
        await _db.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_UpdateMarca {marca.Id}, {marca.Nome}, {(int)marca.Status}");
    }

    public async Task ExcluirMarca(Marcas marca)
    {
        _db.Marcas.Remove(marca);
        await _db.SaveChangesAsync();
    }
    
}

    
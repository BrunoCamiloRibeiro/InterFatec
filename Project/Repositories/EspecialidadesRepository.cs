using Microsoft.EntityFrameworkCore;
using FabysUnha.Models;
using FabysUnha.Data;

namespace FabysUnha.Repositories;

public class EspecialidadesRepository : IEspecialidadeRepository
{
    private readonly AppDbContext _context;

    public EspecialidadesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Especialidades>> ObterTodasEspecialidades()
    {
        return await _context.Especialidades.ToListAsync();
    }

    public async Task<Especialidades?> ObterEspecialidadePorId(int id)
    {
        return await _context.Especialidades.FindAsync(id);
    }

    public async Task CriarEspecialidade(Especialidades especialidade)
    {
        _context.Especialidades.Add(especialidade);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarEspecialidade(Especialidades especialidade)
    {
        _context.Especialidades.Update(especialidade);
        await _context.SaveChangesAsync();
    }

    public async Task ExcluirEspecialidade(Especialidades especialidade)
    {
        _context.Especialidades.Remove(especialidade);
        await _context.SaveChangesAsync();
    }
}
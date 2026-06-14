using Microsoft.EntityFrameworkCore;
using FabysUnha.Models;
using FabysUnha.Data;
using FabysUnha.Enums;

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
        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_InsertEspecialidade {especialidade.Descricao}, {(int)especialidade.Status}");
    }

    public async Task AtualizarEspecialidade(Especialidades especialidade)
    {
        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_UpdateEspecialidade {especialidade.Id}, {especialidade.Descricao}, {(int)especialidade.Status}");
    }

    public async Task ExcluirEspecialidade(Especialidades especialidade)
    {
        especialidade.Status = EspecialidadeStatus.Inativo;
        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_UpdateEspecialidade {especialidade.Id}, {especialidade.Descricao}, {(int)especialidade.Status}");
    }
}
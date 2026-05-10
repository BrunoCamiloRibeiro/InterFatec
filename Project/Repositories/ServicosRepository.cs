using Microsoft.EntityFrameworkCore;
using FabysUnha.Models;
using FabysUnha.Data;

namespace FabysUnha.Repositories;

public class ServicosRepository : IServicosRepository
{
    private readonly AppDbContext _context;

    public ServicosRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Servicos>> ObterTodosServicos()
    {
        return await _context.Servicos.ToListAsync();
    }

    public async Task<Servicos?> ObterServicoPorId(int id)
    {
        return await _context.Servicos.FindAsync(id);
    }

    public async Task CriarServico(Servicos servico)
    {
        _context.Servicos.Add(servico);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarServico(Servicos servico)
    {
        _context.Servicos.Update(servico);
        await _context.SaveChangesAsync();
    }

    public async Task ExcluirServico(Servicos servico)
    {
        _context.Servicos.Remove(servico);
        await _context.SaveChangesAsync();
    }
}
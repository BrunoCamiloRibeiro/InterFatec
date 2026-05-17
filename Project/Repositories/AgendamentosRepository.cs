using Microsoft.EntityFrameworkCore;
using FabysUnha.Data; 
using FabysUnha.Models;

namespace FabysUnha.Repositories;

public class AgendamentosRepository : IAgendamentosRepository
{
    private readonly AppDbContext _context;

    public AgendamentosRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Agendamentos>> ObterTodosAgendamentos()
    {
        return await _context.Agendamentos
            .Include(a => a.Cliente) 
            .Include(a => a.Servicos_Agendados)
            .Include(a => a.Produtos_Agendados)
            .AsNoTracking()
            .ToListAsync();
    }

    // Mudar esses includes pra porra de um view dps
    public async Task<Agendamentos?> ObterAgendamentoPorId(int id)
    {
        return await _context.Agendamentos
            .Include(a => a.Cliente)
            .Include(a => a.Servicos_Agendados) 
                .ThenInclude(sa => sa.Servico)
            .Include(a => a.Servicos_Agendados)
                .ThenInclude(sa => sa.Funcionario)
            .Include(a => a.Produtos_Agendados)
                .ThenInclude(pa => pa.Produto)
            .Include(a => a.Produtos_Agendados)
                .ThenInclude(pa => pa.Servico)
            .FirstOrDefaultAsync(a => a.Nr == id);
    }

    public async Task CriarAgendamento(Agendamentos agendamento)
    {
        await _context.Agendamentos.AddAsync(agendamento);
        await _context.SaveChangesAsync(); 
    }

    public async Task AtualizarAgendamento(Agendamentos agendamento)
    {
        _context.Agendamentos.Update(agendamento);
        await _context.SaveChangesAsync();
    }

    public async Task ExcluirAgendamento(Agendamentos agendamento)
    {
        _context.Agendamentos.Remove(agendamento);
        await _context.SaveChangesAsync();
    }
}
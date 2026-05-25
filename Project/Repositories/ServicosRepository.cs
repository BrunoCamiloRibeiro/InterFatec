using Microsoft.EntityFrameworkCore;
using FabysUnha.Models;
using FabysUnha.Data;
using FabysUnha.Enums;
using FabysUnha.Models.SqlViews;

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
        var servicosView = await _context.Set<ListaServicosView>()
            .AsNoTracking()
            .OrderBy(servico => servico.Descricao)
            .ToListAsync();

        return servicosView.Select(servico => new Servicos
        {
            Id = servico.Id,
            Descricao = servico.Descricao,
            Preco = servico.Preco,
            Tempo = servico.Tempo,
            Status = (ServicoStatus)servico.StatusId
        }).ToList();
    }

    public async Task<Servicos?> ObterServicoPorId(int id)
    {
        return await _context.Servicos.FindAsync(id);
    }

    public async Task CriarServico(Servicos servico)
    {
        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_InsertServico {servico.Preco}, {servico.Descricao}, {servico.Tempo}, {(int)servico.Status}");
    }

    public async Task AtualizarServico(Servicos servico)
    {
        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_UpdateServico {servico.Id}, {servico.Preco}, {servico.Descricao}, {servico.Tempo}, {(int)servico.Status}");
    }

    public async Task ExcluirServico(Servicos servico)
    {
        _context.Servicos.Remove(servico);
        await _context.SaveChangesAsync();
    }
}
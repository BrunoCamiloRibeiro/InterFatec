using Microsoft.EntityFrameworkCore;
using FabysUnha.Data; 
using FabysUnha.Models;
using FabysUnha.Enums;
using FabysUnha.Models.SqlViews;

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
        var agendamentos = await _context.Agendamentos
            .AsNoTracking()
            .Select(agendamento => new
            {
                agendamento.Nr,
                agendamento.ClienteId,
                agendamento.Data,
                agendamento.Status,
                agendamento.Total
            })
            .ToListAsync();

        var clientes = await _context.Set<ListaClientesView>()
            .AsNoTracking()
            .Select(cliente => new
            {
                cliente.Id,
                cliente.Nome,
                cliente.Telefone,
                cliente.StatusId
            })
            .ToDictionaryAsync(cliente => cliente.Id);

        var servicosPorAgendamento = await _context.Servicos_Agendados
            .AsNoTracking()
            .GroupBy(servico => servico.AgendamentoNr)
            .Select(grupo => new { Nr = grupo.Key, Quantidade = grupo.Count() })
            .ToDictionaryAsync(item => item.Nr, item => item.Quantidade);

        var produtosPorAgendamento = await _context.Produtos_Agendados
            .AsNoTracking()
            .GroupBy(produto => produto.AgendamentoNr)
            .Select(grupo => new { Nr = grupo.Key, Quantidade = grupo.Count() })
            .ToDictionaryAsync(item => item.Nr, item => item.Quantidade);

        return agendamentos.Select(agendamento => new Agendamentos
        {
            Nr = agendamento.Nr,
            ClienteId = agendamento.ClienteId,
            Data = agendamento.Data,
            Status = agendamento.Status,
            Total = agendamento.Total,
            Cliente = clientes.TryGetValue(agendamento.ClienteId, out var cliente)
                ? new Clientes
                {
                    Id = cliente.Id,
                    Nome = cliente.Nome,
                    Telefone = cliente.Telefone,
                    Status = (PessoaStatus)cliente.StatusId
                }
                : null,
            Servicos_Agendados = Enumerable.Range(0, servicosPorAgendamento.GetValueOrDefault(agendamento.Nr)).Select(_ => new Servicos_Agendados()).ToList(),
            Produtos_Agendados = Enumerable.Range(0, produtosPorAgendamento.GetValueOrDefault(agendamento.Nr)).Select(_ => new Produtos_Agendados()).ToList()
        }).ToList();
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
                .ThenInclude(pa => pa.ServicoAgendado)
                    .ThenInclude(sa => sa.Servico)
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

    public async Task<List<TimeSpan>> ObterHorariosOcupados(int funcionarioId, DateTime data)
    {
        var dataInicio = data.Date;
        var dataFim = dataInicio.AddDays(1);

        return await _context.Servicos_Agendados
            .AsNoTracking()
            .Where(sa => sa.FuncionarioId == funcionarioId
                && sa.Agendamento != null
                && sa.Agendamento.Data >= dataInicio
                && sa.Agendamento.Data < dataFim
                && sa.Agendamento.Status != Enums.AgendamentoStatus.Cancelado)
            .Select(sa => sa.Horario)
            .Distinct()
            .OrderBy(h => h)
            .ToListAsync();
    }
}
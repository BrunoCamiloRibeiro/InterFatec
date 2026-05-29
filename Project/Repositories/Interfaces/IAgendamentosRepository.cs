using FabysUnha.Models;

namespace FabysUnha.Repositories;

public interface IAgendamentosRepository
{
    Task<IEnumerable<Agendamentos>> ObterTodosAgendamentos();
    Task<Agendamentos?> ObterAgendamentoPorId(int id);
    Task<IEnumerable<Agendamentos>> ObterAgendamentosPorCliente(int clienteId);
    Task CriarAgendamento(Agendamentos agendamento);
    Task AtualizarAgendamento(Agendamentos agendamento);
    Task ExcluirAgendamento(Agendamentos agendamento);
    Task<List<TimeSpan>> ObterHorariosOcupados(int funcionarioId, DateTime data);
}
using FabysUnha.Models;

namespace FabysUnha.Repositories;

public interface IAgendamentosRepository
{
    Task<IEnumerable<Agendamentos>> ObterTodosAgendamentos();
    Task<Agendamentos?> ObterAgendamentoPorId(int id);
    Task CriarAgendamento(Agendamentos agendamento);
    Task AtualizarAgendamento(Agendamentos agendamento);
    Task ExcluirAgendamento(Agendamentos agendamento);
}
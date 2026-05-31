using FabysUnha.Models;
using FabysUnha.ViewModels;

namespace FabysUnha.Services;

public interface IAgendamentosService
{
    Task<IEnumerable<Agendamentos>> ObterTodosAgendamentos();
    Task<Agendamentos?> ObterAgendamentoPorId(int nr);
    Task<IEnumerable<Agendamentos>> ObterAgendamentosPorCliente(int clienteId);
    Task CriarAgendamento(AgendamentoRegistroViewModel viewModel);
    Task AtualizarAgendamento(AgendamentoEditarViewModel viewModel);
    
    Task CancelarAgendamento(int nr);
    Task FinalizarAgendamento(int nr);
    Task ExcluirAgendamento(int nr);

    Task CriarAgendamentoCliente(AgendamentoClienteViewModel viewModel, int clienteId);
    Task<List<TimeSpan>> ObterHorariosDisponiveis(int funcionarioId, DateTime data);
}
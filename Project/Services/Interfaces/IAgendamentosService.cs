using FabysUnha.Models;
using FabysUnha.ViewModels;

namespace FabysUnha.Services;

public interface IAgendamentosService
{
    Task<IEnumerable<Agendamentos>> ObterTodosAgendamentos();
    Task<Agendamentos?> ObterAgendamentoPorId(int nr);
    Task CriarAgendamento(AgendamentoRegistroViewModel viewModel);
    Task AtualizarAgendamento(AgendamentoEditarViewModel viewModel);
    
    Task CancelarAgendamento(int nr);
    Task ExcluirAgendamento(int nr);
}
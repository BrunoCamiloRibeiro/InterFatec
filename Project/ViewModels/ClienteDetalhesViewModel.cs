using System.ComponentModel.DataAnnotations;

namespace FabysUnha.ViewModels;

public class ClienteDetalhesViewModel : PessoasViewModel
{
    public int Id { get; set; }

    public ICollection<AgendamentoResumoViewModel> HistoricoAgendamentos { get; set; } = new List<AgendamentoResumoViewModel>();
}
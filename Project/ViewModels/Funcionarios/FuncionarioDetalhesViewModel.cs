using System.ComponentModel.DataAnnotations;

namespace FabysUnha.ViewModels;


public class FuncionarioDetalhesViewModel : PessoasViewModel
{
    public int Id { get; set; }

    [Display(Name = "Salário")]
    public decimal Salario { get; set; }

    [Display(Name = "Especialidade")]
    public string EspecialidadeNome { get; set; } = string.Empty;

    public ICollection<ServicoAgendadoViewModel> ServicosAgendados { get; set; } = new List<ServicoAgendadoViewModel>();
}
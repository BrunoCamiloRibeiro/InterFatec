using System.ComponentModel.DataAnnotations;

namespace FabysUnha.ViewModels;

public class FuncionarioListagemViewModel : PessoasViewModel
{
    public int Id { get; set; }

    [Display(Name = "Salário")]
    public decimal Salario { get; set; }

    [Display(Name = "Especialidade")]
    public string EspecialidadeNome { get; set; } = string.Empty;
}
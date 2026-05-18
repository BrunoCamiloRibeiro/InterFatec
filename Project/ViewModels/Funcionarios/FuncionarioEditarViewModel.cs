using System.ComponentModel.DataAnnotations;

namespace FabysUnha.ViewModels;

public class FuncionarioEditarViewModel : PessoasViewModel
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "O salário é obrigatório.")]
    public decimal Salario { get; set; }

    [Required(ErrorMessage = "A especialidade é obrigatória.")]
    [Display(Name = "Especialidade")]
    public int EspecialidadeId { get; set; }
}
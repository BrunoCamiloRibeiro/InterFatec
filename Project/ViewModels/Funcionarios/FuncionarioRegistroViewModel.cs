using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FabysUnha.ViewModels;

public class FuncionarioRegistroViewModel : PessoasViewModel
{
    [Required(ErrorMessage = "O salário é obrigatório.")]
    [Range(0, 99999, ErrorMessage = "O salário deve ser um valor positivo.")]
    public decimal Salario { get; set; }

    [Required(ErrorMessage = "Selecione uma especialidade.")]
    [Display(Name = "Especialidade")]
    public int EspecialidadeId { get; set; }

    public IEnumerable<SelectListItem>? EspecialidadesList { get; set; }
}
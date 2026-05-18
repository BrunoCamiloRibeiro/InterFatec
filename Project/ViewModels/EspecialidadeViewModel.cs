using System.ComponentModel.DataAnnotations;
using FabysUnha.Enums;

namespace FabysUnha.ViewModels;

public class EspecialidadeViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [MinLength(3, ErrorMessage = "O campo Nome deve conter pelo menos 3 caracteres.")]
    [MaxLength(25, ErrorMessage = "O campo Nome deve conter no máximo 25 caracteres.")]
    [Display(Name = "Nome da Especialidade")]
    public string Nome { get; set; } = string.Empty;

    [Display(Name = "Status")]
    public EspecialidadeStatus Status { get; set; } = EspecialidadeStatus.Ativo;
}
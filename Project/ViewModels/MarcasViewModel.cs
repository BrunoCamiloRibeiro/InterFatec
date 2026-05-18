using System.ComponentModel.DataAnnotations;
using FabysUnha.Enums;

namespace FabysUnha.ViewModels;

public class MarcasViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    public string Nome { get; set; } = string.Empty;

    [Display(Name = "Status")]
    public MarcaStatus Status { get; set; } = MarcaStatus.Ativo;
}
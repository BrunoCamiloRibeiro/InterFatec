using System.ComponentModel.DataAnnotations;

namespace FabysUnha.ViewModels;

public class MarcasViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    public string Nome { get; set; } = string.Empty;
}
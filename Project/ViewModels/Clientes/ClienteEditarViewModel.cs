using System.ComponentModel.DataAnnotations;

namespace FabysUnha.ViewModels;

public class ClienteEditarViewModel : PessoasViewModel
{
    [Required]
    public int Id { get; set; } 
}
using System.ComponentModel.DataAnnotations;

namespace FabysUnha.ViewModels;

public class ClienteRegistroViewModel : PessoasViewModel
{
    [Required(ErrorMessage = "A senha é obrigatória.")]
    [DataType(DataType.Password)]
    [Display(Name = "Senha")]
    public string Senha { get; set; } = string.Empty;
}
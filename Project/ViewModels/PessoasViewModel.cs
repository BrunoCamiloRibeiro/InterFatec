using System.ComponentModel.DataAnnotations;
using FabysUnha.Enums;

namespace FabysUnha.ViewModels;

public abstract class PessoasViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [Phone(ErrorMessage = "Telefone em formato inválido.")]
    public string Telefone { get; set; } = string.Empty;

    [Display(Name = "Status")]
    public PessoaStatus Status { get; set; } = PessoaStatus.Ativo;
}
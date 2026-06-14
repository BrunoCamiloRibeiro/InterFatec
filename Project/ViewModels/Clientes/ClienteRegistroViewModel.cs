using System.ComponentModel.DataAnnotations;

namespace FabysUnha.ViewModels;

/// <summary>
/// ViewModel utilizado para capturar os dados necessários durante o registro (criação) de um novo cliente.
/// Herda de PessoasViewModel, que contém as propriedades básicas de identificação e contato.
/// </summary>
public class ClienteRegistroViewModel : PessoasViewModel
{
    /// <summary>
    /// Senha de acesso do cliente ao sistema.
    /// Este campo é obrigatório para novos registros.
    /// </summary>
    [Required(ErrorMessage = "A senha é obrigatória.")]
    [DataType(DataType.Password)]
    [Display(Name = "Senha")]
    public string Senha { get; set; } = string.Empty;
}
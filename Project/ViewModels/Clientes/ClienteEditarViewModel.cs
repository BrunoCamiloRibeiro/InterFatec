using System.ComponentModel.DataAnnotations;

namespace FabysUnha.ViewModels;

/// <summary>
/// ViewModel responsável por carregar e receber os dados durante a edição de um cliente existente.
/// Herda de PessoasViewModel para utilizar os dados básicos de uma pessoa.
/// </summary>
public class ClienteEditarViewModel : PessoasViewModel
{
    /// <summary>
    /// Identificador único do cliente que está sendo editado.
    /// Este campo é obrigatório para garantir que a atualização ocorra no registro correto.
    /// </summary>
    [Required]
    public int Id { get; set; } 
}
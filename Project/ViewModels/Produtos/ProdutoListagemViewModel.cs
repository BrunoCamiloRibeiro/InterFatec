/*
 *  Usar isso na pagina INDEX
 */

using System.ComponentModel.DataAnnotations;
using FabysUnha.Enums;

namespace FabysUnha.ViewModels;

/// <summary>
/// ViewModel utilizado para listar os produtos em uma tabela ou grid na interface.
/// Focado em conter apenas as informações necessárias para a listagem (Index).
/// </summary>
public class ProdutoListagemViewModel
{
    /// <summary>
    /// Obtém ou define o código do produto.
    /// </summary>
    public int Codigo { get; set; }

    /// <summary>
    /// Obtém ou define o nome do produto.
    /// </summary>
    [Display(Name = "Produto")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o nome da marca relacionada ao produto.
    /// </summary>
    [Display(Name = "Marca")]
    public string NomeMarca { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o preço do produto já formatado como moeda.
    /// </summary>
    [Display(Name = "Preço")]
    public string PrecoFormatado { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o status atual do produto.
    /// </summary>
    [Display(Name = "Status")]
    public ProdutoStatus Status { get; set; }
}
using System.ComponentModel.DataAnnotations;
using FabysUnha.Enums;

namespace FabysUnha.ViewModels;

/// <summary>
/// ViewModel responsável por apresentar os detalhes completos de um produto.
/// </summary>
public class ProdutoDetalhesViewModel
{
    /// <summary>
    /// Obtém ou define o código (identificador) do produto.
    /// </summary>
    public int Codigo { get; set; }

    /// <summary>
    /// Obtém ou define o nome do produto.
    /// </summary>
    [Display(Name = "Produto")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o nome da marca associada ao produto.
    /// </summary>
    [Display(Name = "Marca")]
    public string NomeMarca { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o preço formatado (em moeda local) do produto para exibição.
    /// </summary>
    [Display(Name = "Preço")]
    public string PrecoFormatado { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o caminho da imagem do produto.
    /// </summary>
    [Display(Name = "Imagem")]
    public string PathImagem { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o status atual do produto (Ativo, Inativo, etc.).
    /// </summary>
    [Display(Name = "Status")]
    public ProdutoStatus Status { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace FabysUnha.ViewModels;

public class ProdutoDetalhesViewModel
{
    public int Codigo { get; set; }

    [Display(Name = "Produto")]
    public string Nome { get; set; } = string.Empty;

    [Display(Name = "Marca")]
    public string NomeMarca { get; set; } = string.Empty;

    [Display(Name = "Preço")]
    public string PrecoFormatado { get; set; } = string.Empty;

    [Display(Name = "Imagem")]
    public string PathImagem { get; set; } = string.Empty;
}
/*
 *  Usar isso na pagina INDEX
 */

using System.ComponentModel.DataAnnotations;

namespace FabysUnha.ViewModels;

public class ProdutoListagemViewModel
{
    public int Codigo { get; set; }

    [Display(Name = "Produto")]
    public string Nome { get; set; } = string.Empty;

    [Display(Name = "Marca")]
    public string NomeMarca { get; set; } = string.Empty;

    [Display(Name = "Preço")]
    public string PrecoFormatado { get; set; } = string.Empty;
}
using System.ComponentModel.DataAnnotations;

namespace FabysUnha.ViewModels;

public class ProdutoAgendadoViewModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Selecione um produto válido.")]
    [Display(Name = "Produto")]
    public int ProdutoCodigo { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Selecione um serviço válido.")]
    [Display(Name = "Serviço")]
    public int ServicoId { get; set; }

    [Display(Name = "Preço")]
    [DataType(DataType.Currency)]
    public decimal Preco { get; set; }

    [Display(Name = "Produto")]
    public string ProdutoNome { get; set; } = string.Empty;

    [Display(Name = "Serviço")]
    public string ServicoNome { get; set; } = string.Empty;
}
using System.ComponentModel.DataAnnotations;
using FabysUnha.Enums;

namespace FabysUnha.ViewModels;

public class AgendamentoListagemViewModel
{
    public int Nr { get; set; }

    [Display(Name = "Cliente")]
    public string ClienteNome { get; set; } = string.Empty;

    [Display(Name = "Data e Hora")]
    public DateTime DataHora { get; set; }

    [Display(Name = "Status")]
    public AgendamentoStatus Status { get; set; }

    [Display(Name = "Total")]
    [DataType(DataType.Currency)]
    public decimal Total { get; set; }

    [Display(Name = "Serviços")]
    public int QuantidadeServicos { get; set; }

    [Display(Name = "Produtos")]
    public int QuantidadeProdutos { get; set; }
}
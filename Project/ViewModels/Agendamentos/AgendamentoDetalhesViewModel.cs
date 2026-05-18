using System.ComponentModel.DataAnnotations;
using FabysUnha.Enums;

namespace FabysUnha.ViewModels;

public class AgendamentoDetalhesViewModel
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

    public List<ServicoAgendadoViewModel> ServicosAgendados { get; set; } = new();

    public List<ProdutoAgendadoViewModel> ProdutosAgendados { get; set; } = new();
}
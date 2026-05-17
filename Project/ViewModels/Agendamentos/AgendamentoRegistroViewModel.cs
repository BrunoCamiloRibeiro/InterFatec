using System.ComponentModel.DataAnnotations;
using FabysUnha.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FabysUnha.ViewModels;

public class AgendamentoRegistroViewModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Selecione um cliente válido.")]
    [Display(Name = "Cliente")]
    public int ClienteId { get; set; }

    [Required(ErrorMessage = "A data e hora do agendamento são obrigatórias.")]
    [Display(Name = "Data e Hora")]
    [DataType(DataType.DateTime)]
    public DateTime DataHora { get; set; }

    [Display(Name = "Status")]
    public AgendamentoStatus Status { get; set; } = AgendamentoStatus.Pendente;

    [Display(Name = "Serviços Selecionados")]
    public List<ServicoAgendadoViewModel> ServicosSelecionados { get; set; } = new();

    [Display(Name = "Produtos Selecionados")]
    public List<ProdutoAgendadoViewModel> ProdutosSelecionados { get; set; } = new();

    public IEnumerable<SelectListItem>? ClientesList { get; set; }
    public IEnumerable<SelectListItem>? FuncionariosList { get; set; }
    public IEnumerable<SelectListItem>? ServicosList { get; set; }
    public IEnumerable<SelectListItem>? ProdutosList { get; set; }
}
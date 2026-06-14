using System.ComponentModel.DataAnnotations;
using FabysUnha.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FabysUnha.ViewModels;

/// <summary>
/// ViewModel responsável por capturar os dados necessários para o registro ou edição de um agendamento.
/// Contém as propriedades de entrada de dados e listas de seleção para a interface do usuário.
/// </summary>
public class AgendamentoRegistroViewModel
{
    /// <summary>
    /// Identificador do cliente escolhido para o agendamento.
    /// É obrigatório selecionar um cliente válido (ID > 0).
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Selecione um cliente válido.")]
    [Display(Name = "Cliente")]
    public int ClienteId { get; set; }

    /// <summary>
    /// Data e hora em que o agendamento ocorrerá.
    /// Este campo é obrigatório.
    /// </summary>
    [Required(ErrorMessage = "A data e hora do agendamento são obrigatórias.")]
    [Display(Name = "Data e Hora")]
    [DataType(DataType.DateTime)]
    public DateTime DataHora { get; set; }

    /// <summary>
    /// Status inicial do agendamento. Por padrão, começa como Pendente.
    /// </summary>
    [Display(Name = "Status")]
    public AgendamentoStatus Status { get; set; } = AgendamentoStatus.Pendente;

    /// <summary>
    /// Lista de serviços que foram selecionados para este agendamento.
    /// </summary>
    [Display(Name = "Serviços Selecionados")]
    public List<ServicoAgendadoViewModel> ServicosSelecionados { get; set; } = new();

    /// <summary>
    /// Lista de produtos que foram selecionados ou consumidos neste agendamento.
    /// </summary>
    [Display(Name = "Produtos Selecionados")]
    public List<ProdutoAgendadoViewModel> ProdutosSelecionados { get; set; } = new();

    /// <summary>
    /// Lista de clientes disponíveis para seleção no formulário (Dropdown).
    /// </summary>
    public IEnumerable<SelectListItem>? ClientesList { get; set; }
    
    /// <summary>
    /// Lista de funcionários disponíveis para seleção no formulário (Dropdown).
    /// </summary>
    public IEnumerable<SelectListItem>? FuncionariosList { get; set; }
    
    /// <summary>
    /// Lista de serviços disponíveis para seleção no formulário (Dropdown).
    /// </summary>
    public IEnumerable<SelectListItem>? ServicosList { get; set; }
    
    /// <summary>
    /// Lista de produtos disponíveis para seleção no formulário (Dropdown).
    /// </summary>
    public IEnumerable<SelectListItem>? ProdutosList { get; set; }
}
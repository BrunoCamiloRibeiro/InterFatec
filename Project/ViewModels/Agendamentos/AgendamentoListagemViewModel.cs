using System.ComponentModel.DataAnnotations;
using FabysUnha.Enums;

namespace FabysUnha.ViewModels;

/// <summary>
/// ViewModel responsável por representar os dados de um agendamento em listagens.
/// Utilizado para exibir de forma resumida as informações principais do agendamento para o usuário.
/// </summary>
public class AgendamentoListagemViewModel
{
    /// <summary>
    /// Número de identificação do agendamento.
    /// </summary>
    public int Nr { get; set; }

    /// <summary>
    /// Nome do cliente associado ao agendamento.
    /// </summary>
    [Display(Name = "Cliente")]
    public string ClienteNome { get; set; } = string.Empty;

    /// <summary>
    /// Data e hora em que o agendamento está marcado.
    /// </summary>
    [Display(Name = "Data e Hora")]
    public DateTime DataHora { get; set; }

    /// <summary>
    /// Status atual do agendamento (ex: Pendente, Concluído, Cancelado).
    /// </summary>
    [Display(Name = "Status")]
    public AgendamentoStatus Status { get; set; }

    /// <summary>
    /// Valor total do agendamento, incluindo serviços e produtos.
    /// </summary>
    [Display(Name = "Total")]
    [DataType(DataType.Currency)]
    public decimal Total { get; set; }

    /// <summary>
    /// Quantidade de serviços que foram incluídos neste agendamento.
    /// </summary>
    [Display(Name = "Serviços")]
    public int QuantidadeServicos { get; set; }

    /// <summary>
    /// Quantidade de produtos que foram incluídos neste agendamento.
    /// </summary>
    [Display(Name = "Produtos")]
    public int QuantidadeProdutos { get; set; }
}
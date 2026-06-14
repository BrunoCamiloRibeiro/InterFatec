using System.ComponentModel.DataAnnotations;

namespace FabysUnha.ViewModels;

/// <summary>
/// ViewModel que representa um produto vinculado a um agendamento específico.
/// Utilizado para manter as informações do produto associado a um serviço durante o agendamento.
/// </summary>
public class ProdutoAgendadoViewModel
{
    /// <summary>
    /// Código identificador do produto selecionado.
    /// Deve ser um código válido e maior que zero.
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Selecione um produto válido.")]
    [Display(Name = "Produto")]
    public int ProdutoCodigo { get; set; }

    /// <summary>
    /// Identificador do serviço ao qual este produto está atrelado.
    /// Deve ser um serviço válido e maior que zero.
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Selecione um serviço válido.")]
    [Display(Name = "Serviço")]
    public int ServicoId { get; set; }

    /// <summary>
    /// Preço do produto no momento do agendamento.
    /// Formatado como moeda para exibição.
    /// </summary>
    [Display(Name = "Preço")]
    [DataType(DataType.Currency)]
    public decimal Preco { get; set; }

    /// <summary>
    /// Nome ou descrição do produto selecionado.
    /// </summary>
    [Display(Name = "Produto")]
    public string ProdutoNome { get; set; } = string.Empty;

    /// <summary>
    /// Nome ou descrição do serviço ao qual o produto está atrelado.
    /// </summary>
    [Display(Name = "Serviço")]
    public string ServicoNome { get; set; } = string.Empty;
}
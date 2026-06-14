using System.ComponentModel.DataAnnotations;
using FabysUnha.Enums;

namespace FabysUnha.ViewModels.Servicos;

/// <summary>
/// ViewModel projetado para a listagem resumida de serviços, normalmente exibida em tabelas.
/// </summary>
public class ServicoListagemViewModel
{
    /// <summary>
    /// Obtém ou define o identificador do serviço.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Obtém ou define a descrição (nome) do serviço.
    /// </summary>
    [Display(Name = "Descrição")]
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o valor do serviço formatado (ex: R$ 50,00).
    /// </summary>
    [Display(Name = "Preço")]
    public string PrecoFormatado { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o tempo estimado de duração formatado como string.
    /// </summary>
    [Display(Name = "Duração")]
    public string TempoFormatado { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o status atual do serviço (Ativo, Inativo, etc.).
    /// </summary>
    [Display(Name = "Status")]
    public ServicoStatus Status { get; set; }
}
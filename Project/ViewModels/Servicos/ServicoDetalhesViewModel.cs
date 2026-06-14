using System.ComponentModel.DataAnnotations;
using FabysUnha.Enums;

namespace FabysUnha.ViewModels.Servicos;

/// <summary>
/// ViewModel utilizado para mostrar as informações detalhadas de um serviço.
/// </summary>
public class ServicoDetalhesViewModel
{
    /// <summary>
    /// Obtém ou define o identificador único do serviço.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Obtém ou define a descrição do serviço.
    /// </summary>
    [Display(Name = "Descrição do Serviço")]
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o preço do serviço formatado como string.
    /// </summary>
    [Display(Name = "Valor Cobrado")]
    public string PrecoFormatado { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o tempo estimado do serviço formatado (por exemplo, "HH:mm").
    /// </summary>
    [Display(Name = "Tempo Estimado")]
    public string TempoFormatado { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o status do serviço.
    /// </summary>
    [Display(Name = "Status")]
    public ServicoStatus Status { get; set; }
}
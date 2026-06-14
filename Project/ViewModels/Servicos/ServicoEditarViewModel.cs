using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using FabysUnha.Enums;

namespace FabysUnha.ViewModels.Servicos;

/// <summary>
/// ViewModel utilizado para edição dos dados de um serviço existente.
/// </summary>
public class ServicoEditarViewModel
{
    /// <summary>
    /// Obtém ou define o identificador do serviço. Escondido no formulário usando o atributo [HiddenInput].
    /// </summary>
    [HiddenInput]
    public int Id { get; set; }

    /// <summary>
    /// Obtém ou define a descrição do serviço.
    /// </summary>
    [Required(ErrorMessage = "O campo Descrição é obrigatório.")]
    [MinLength(3, ErrorMessage = "A descrição deve ter pelo menos 3 caracteres.")]
    [MaxLength(100, ErrorMessage = "A descrição não pode passar de 100 caracteres.")]
    [Display(Name = "Descrição do Serviço")]
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o preço cobrado pelo serviço.
    /// </summary>
    [Required(ErrorMessage = "O campo Preço é obrigatório.")]
    [DataType(DataType.Currency)]
    [Display(Name = "Preço")]
    public decimal Preco { get; set; }

    /// <summary>
    /// Obtém ou define o tempo estimado de duração do serviço.
    /// </summary>
    [Required(ErrorMessage = "O Tempo estimado é obrigatório.")]
    [DataType(DataType.Time)] 
    [Display(Name = "Tempo Estimado (HH:mm)")]
    public TimeSpan Tempo { get; set; }

    /// <summary>
    /// Obtém ou define o status do serviço, com valor inicial padrão Ativo.
    /// </summary>
    [Display(Name = "Status")]
    public ServicoStatus Status { get; set; } = ServicoStatus.Ativo;
}
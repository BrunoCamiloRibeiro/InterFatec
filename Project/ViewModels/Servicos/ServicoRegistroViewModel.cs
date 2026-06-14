using System.ComponentModel.DataAnnotations;
using FabysUnha.Enums;

namespace FabysUnha.ViewModels.Servicos;

/// <summary>
/// ViewModel utilizado para realizar o cadastro de novos serviços.
/// </summary>
public class ServicoRegistroViewModel
{
    /// <summary>
    /// Obtém ou define a descrição do novo serviço.
    /// </summary>
    [Required(ErrorMessage = "O campo Descrição é obrigatório.")]
    [MinLength(3, ErrorMessage = "A descrição deve ter pelo menos 3 caracteres.")]
    [MaxLength(100, ErrorMessage = "A descrição não pode passar de 100 caracteres.")]
    [Display(Name = "Descrição do Serviço")]
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o preço base do serviço.
    /// </summary>
    [Required(ErrorMessage = "O campo Preço é obrigatório.")]
    [DataType(DataType.Currency)]
    [Display(Name = "Preço")]
    public decimal Preco { get; set; }

    /// <summary>
    /// Obtém ou define a duração estimada para conclusão do serviço.
    /// </summary>
    [Required(ErrorMessage = "O Tempo estimado é obrigatório.")]
    [DataType(DataType.Time)] 
    [Display(Name = "Tempo Estimado (HH:mm)")]
    public TimeSpan Tempo { get; set; }

    /// <summary>
    /// Obtém ou define o status inicial do serviço ao ser cadastrado. Oculto por padrão como Ativo.
    /// </summary>
    [Display(Name = "Status")]
    public ServicoStatus Status { get; set; } = ServicoStatus.Ativo;
}
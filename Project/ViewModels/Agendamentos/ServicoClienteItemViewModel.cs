using System.ComponentModel.DataAnnotations;

namespace FabysUnha.ViewModels;

/// <summary>
/// ViewModel que representa um item de serviço selecionado pelo cliente em um agendamento.
/// Contém dados sobre o serviço, o funcionário alocado e informações adicionais de horário e observações.
/// </summary>
public class ServicoClienteItemViewModel
{
    /// <summary>
    /// Identificador do serviço selecionado.
    /// Deve ser um identificador válido e maior que zero.
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Selecione um serviço válido.")]
    [Display(Name = "Serviço")]
    public int ServicoId { get; set; }

    /// <summary>
    /// Identificador do funcionário (profissional) que realizará o serviço.
    /// Deve ser um identificador válido e maior que zero.
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Selecione um funcionário válido.")]
    [Display(Name = "Funcionário")]
    public int FuncionarioId { get; set; }

    /// <summary>
    /// Horário específico em que o serviço será executado.
    /// Este campo é de preenchimento obrigatório.
    /// </summary>
    [Required(ErrorMessage = "O horário é obrigatório.")]
    [Display(Name = "Horário")]
    public string Horario { get; set; } = string.Empty;

    /// <summary>
    /// Observações adicionais sobre o serviço a ser realizado.
    /// Limitado a 200 caracteres para evitar textos excessivamente longos.
    /// </summary>
    [Display(Name = "Observação")]
    [StringLength(200, ErrorMessage = "A observação não pode ultrapassar 200 caracteres.")]
    public string? Obs { get; set; } = string.Empty;

    /// <summary>
    /// Lista de códigos de produtos que serão utilizados ou consumidos durante a execução deste serviço.
    /// Pode conter valores nulos caso o produto não seja obrigatório ou não possua código.
    /// </summary>
    [Display(Name = "Produtos")]
    public List<int?> ProdutosCodigos { get; set; } = new();
}

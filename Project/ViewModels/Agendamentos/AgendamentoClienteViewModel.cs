using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FabysUnha.ViewModels;

public class AgendamentoClienteViewModel
{
    // Identificação do cliente
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
    [Display(Name = "Nome")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [StringLength(11, ErrorMessage = "O telefone não pode exceder 11 caracteres.")]
    [Display(Name = "Telefone")]
    public string Telefone { get; set; } = string.Empty;

    // Data do agendamento
    [Required(ErrorMessage = "A data do agendamento é obrigatória.")]
    [Display(Name = "Data")]
    [DataType(DataType.Date)]
    public DateTime Data { get; set; }

    // Serviços selecionados (containers)
    [Display(Name = "Serviços")]
    public List<ServicoClienteItemViewModel> Servicos { get; set; } = new();

    // Listas para popular os selects na view
    public IEnumerable<SelectListItem>? ServicosList { get; set; }
    public IEnumerable<SelectListItem>? FuncionariosList { get; set; }
    public IEnumerable<SelectListItem>? ProdutosList { get; set; }
}

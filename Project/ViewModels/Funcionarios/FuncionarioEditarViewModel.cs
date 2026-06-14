using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FabysUnha.ViewModels;

/// <summary>
/// ViewModel utilizado para editar as informações de um funcionário existente.
/// Herda de PessoasViewModel para incluir dados básicos da pessoa.
/// </summary>
public class FuncionarioEditarViewModel : PessoasViewModel
{
    /// <summary>
    /// Obtém ou define o identificador único do funcionário a ser editado.
    /// </summary>
    [Required]
    public int Id { get; set; }

    /// <summary>
    /// Obtém ou define o salário do funcionário.
    /// </summary>
    [Required(ErrorMessage = "O salário é obrigatório.")]
    public decimal Salario { get; set; }

    /// <summary>
    /// Obtém ou define o identificador da especialidade associada ao funcionário.
    /// </summary>
    [Required(ErrorMessage = "A especialidade é obrigatória.")]
    [Display(Name = "Especialidade")]
    public int EspecialidadeId { get; set; }

    /// <summary>
    /// Obtém ou define a lista de especialidades disponíveis para seleção no formulário de edição.
    /// Utiliza IEnumerable<SelectListItem> para facilitar a criação de um <select> na View.
    /// </summary>
    // Propriedade preenchida no Controller para popular o dropdown de especialidades
    public IEnumerable<SelectListItem>? EspecialidadesList { get; set; }
}
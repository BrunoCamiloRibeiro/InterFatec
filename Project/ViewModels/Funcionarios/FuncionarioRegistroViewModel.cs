using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FabysUnha.ViewModels;

/// <summary>
/// ViewModel utilizado para o registro de um novo funcionário no sistema.
/// Herda de PessoasViewModel para obter as propriedades básicas de uma pessoa (Nome, CPF, etc.).
/// </summary>
public class FuncionarioRegistroViewModel : PessoasViewModel
{
    /// <summary>
    /// Obtém ou define o salário do funcionário.
    /// Deve ser um valor positivo.
    /// </summary>
    [Required(ErrorMessage = "O salário é obrigatório.")]
    [Range(0, 99999, ErrorMessage = "O salário deve ser um valor positivo.")]
    public decimal Salario { get; set; }

    /// <summary>
    /// Obtém ou define o identificador da especialidade escolhida para o funcionário.
    /// </summary>
    [Required(ErrorMessage = "Selecione uma especialidade.")]
    [Display(Name = "Especialidade")]
    public int EspecialidadeId { get; set; }

    /// <summary>
    /// Obtém ou define a senha do funcionário, necessária para acessar o sistema.
    /// </summary>
    [Required(ErrorMessage = "O campo Senha é obrigatório.")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 20 caracteres.")]
    [DataType(DataType.Password)]
    public string Senha { get; set; } = string.Empty;
    
    /// <summary>
    /// Obtém ou define a lista de especialidades disponíveis para seleção.
    /// </summary>
    // Usada para renderizar o elemento <select> na View
    public IEnumerable<SelectListItem>? EspecialidadesList { get; set; }
}
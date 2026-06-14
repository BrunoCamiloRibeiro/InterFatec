using System.ComponentModel.DataAnnotations;

namespace FabysUnha.ViewModels;

/// <summary>
/// ViewModel utilizado para exibir informações resumidas dos funcionários em uma lista (por exemplo, em uma tabela).
/// Herda de PessoasViewModel para acessar os dados básicos da pessoa.
/// </summary>
public class FuncionarioListagemViewModel : PessoasViewModel
{
    /// <summary>
    /// Obtém ou define o identificador único do funcionário.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Obtém ou define o salário do funcionário, formatado para exibição se necessário.
    /// </summary>
    [Display(Name = "Salário")]
    public decimal Salario { get; set; }

    /// <summary>
    /// Obtém ou define o nome da especialidade associada ao funcionário.
    /// </summary>
    [Display(Name = "Especialidade")]
    public string EspecialidadeNome { get; set; } = string.Empty;
}
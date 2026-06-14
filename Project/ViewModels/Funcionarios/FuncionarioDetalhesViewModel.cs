using System.ComponentModel.DataAnnotations;

namespace FabysUnha.ViewModels;

/// <summary>
/// ViewModel utilizado para exibir os detalhes de um funcionário específico.
/// Herda de PessoasViewModel para reaproveitar propriedades comuns como Nome, CPF, etc.
/// </summary>
public class FuncionarioDetalhesViewModel : PessoasViewModel
{
    /// <summary>
    /// Obtém ou define o identificador único do funcionário.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Obtém ou define o salário do funcionário.
    /// </summary>
    [Display(Name = "Salário")]
    public decimal Salario { get; set; }

    /// <summary>
    /// Obtém ou define o nome da especialidade do funcionário.
    /// Exibido na interface como "Especialidade".
    /// </summary>
    [Display(Name = "Especialidade")]
    public string EspecialidadeNome { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define a coleção de serviços agendados para este funcionário.
    /// </summary>
    // Inicializa a lista para evitar NullReferenceException
    public ICollection<ServicoAgendadoViewModel> ServicosAgendados { get; set; } = new List<ServicoAgendadoViewModel>();
}
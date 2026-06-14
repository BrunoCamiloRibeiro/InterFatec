namespace FabysUnha.Models.SqlViews;

/// <summary>
/// Classe de modelo que representa a view de banco de dados para a produção dos funcionários.
/// Utilizada pelo Entity Framework Core como um Keyless Entity Type.
/// </summary>
public class FuncionarioProducaoView
{
    /// <summary>
    /// Obtém ou define o nome do funcionário.
    /// </summary>
    // Inicializa a string como vazia para evitar problemas de null reference.
    public string Funcionario { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o número total de serviços realizados pelo funcionário.
    /// </summary>
    // Representa a contagem de serviços associados ao funcionário na view.
    public int TotalServicos { get; set; }
}
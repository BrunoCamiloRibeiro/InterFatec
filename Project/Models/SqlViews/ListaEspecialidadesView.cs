namespace FabysUnha.Models.SqlViews;

/// <summary>
/// Classe de modelo utilizada para representar a view de especialidades agrupadas.
/// Mapeada no Entity Framework Core como Keyless Entity.
/// </summary>
public class ListaEspecialidadesView
{
    /// <summary>
    /// Obtém ou define o nome da especialidade.
    /// </summary>
    // Nome descritivo da especialidade (ex: Manicure, Pedicure).
    public string Especialidade { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define a quantidade total de funcionários que possuem esta especialidade.
    /// </summary>
    // Valor calculado na view do banco de dados, possivelmente usando COUNT e GROUP BY.
    public int TotalFuncionarios { get; set; }
}
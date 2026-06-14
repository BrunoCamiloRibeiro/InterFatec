namespace FabysUnha.Models.SqlViews;

/// <summary>
/// Entidade de visualização que representa dados agregados (quantidade de produtos por marca).
/// Configurável no EF Core como tipo de entidade sem chave para obter resultados de agregação SQL.
/// </summary>
public class ProdutosPorMarcaView
{
    /// <summary>
    /// Obtém ou define o nome da marca associada aos produtos.
    /// </summary>
    // A propriedade representa o grupo utilizado na cláusula GROUP BY da view.
    public string Marca { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define a contagem total de produtos que pertencem à marca correspondente.
    /// </summary>
    // Resultado da função de agregação COUNT() para apresentar em gráficos ou dashboards.
    public int TotalProdutos { get; set; }
}
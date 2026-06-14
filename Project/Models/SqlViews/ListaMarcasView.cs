namespace FabysUnha.Models.SqlViews;

/// <summary>
/// Modelo destinado a espelhar a view de listagem de marcas de produtos.
/// Tratado como entidade sem chave primária no mapeamento do Entity Framework Core.
/// </summary>
public class ListaMarcasView
{
    /// <summary>
    /// Obtém ou define o ID único da marca.
    /// </summary>
    // Identificador da marca gerado pelo banco de dados.
    public int Id { get; set; }

    /// <summary>
    /// Obtém ou define o nome descritivo da marca.
    /// </summary>
    // O nome comercial da marca do produto.
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o valor numérico que representa o status da marca.
    /// </summary>
    // Controle interno do sistema para verificar se a marca está ativa para uso.
    public int Status { get; set; }

    /// <summary>
    /// Obtém ou define a descrição do status da marca.
    /// </summary>
    // Texto amigável retornado pela view para apresentar ao usuário final.
    public string StatusDescricao { get; set; } = string.Empty;
}
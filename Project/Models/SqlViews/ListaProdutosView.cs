namespace FabysUnha.Models.SqlViews;

/// <summary>
/// Modelo de visualização para a lista de produtos disponíveis.
/// Reflete os dados de uma view SQL para consultas (Keyless Entity no EF Core).
/// </summary>
public class ListaProdutosView
{
    /// <summary>
    /// Obtém ou define o código único identificador do produto.
    /// </summary>
    // Representa a chave original do produto na tabela correspondente.
    public int Codigo { get; set; }

    /// <summary>
    /// Obtém ou define o nome do produto.
    /// </summary>
    // String vazia por padrão para evitar referências nulas indevidas.
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o nome da marca associada ao produto.
    /// </summary>
    // Já vem resolvido do relacionamento com a tabela de marcas na view.
    public string Marca { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o preço de venda ou custo unitário do produto.
    /// </summary>
    // Valor numérico em formato decimal adequado para operações de moeda.
    public decimal Preco { get; set; }

    /// <summary>
    /// Obtém ou define o caminho ou URL da imagem do produto.
    /// </summary>
    // Usado no front-end para exibir a foto ou ilustração do produto.
    public string PathImagem { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o ID do status do produto (ativo, inativo, fora de estoque).
    /// </summary>
    // Controle interno de disponibilidade do produto.
    public int StatusId { get; set; }

    /// <summary>
    /// Obtém ou define a string descritiva do status do produto.
    /// </summary>
    // Informação de status já traduzida para leitura humana.
    public string StatusDescricao { get; set; } = string.Empty;
}
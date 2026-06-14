namespace FabysUnha.Enums;

/// <summary>
/// Representa os estados que um produto pode assumir no controle de estoque.
/// </summary>
/// <remarks>
/// Uma enumeração (enum) fornece uma maneira tipada de trabalhar com opções predefinidas,
/// evitando o uso de strings que podem causar erros de digitação (ex: "ATIVO", "Ativo").
/// </remarks>
public enum ProdutoStatus
{
    /// <summary>
    /// O produto está disponível e pode ser utilizado ou comercializado.
    /// </summary>
    Ativo = 0, // O valor 0 é mapeado para o estado Ativo no banco de dados.

    /// <summary>
    /// O produto foi retirado de circulação, não devendo aparecer em novas listagens.
    /// </summary>
    Inativo = 1 // O valor 1 representa o status Inativo para o sistema.
}
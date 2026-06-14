namespace FabysUnha.Enums;

/// <summary>
/// Enumeração que indica se uma marca de produto está ativa ou inativa.
/// </summary>
/// <remarks>
/// Utilizar enums auxilia a padronizar os dados armazenados na base de dados 
/// e facilita o entendimento do código por outros desenvolvedores.
/// </remarks>
public enum MarcaStatus
{
    /// <summary>
    /// A marca está ativa e os produtos associados a ela podem ser movimentados.
    /// </summary>
    Ativo = 0, // 0 é o valor inicial padrão associado ao status Ativo.

    /// <summary>
    /// A marca está inativa, geralmente bloqueando a entrada de novos produtos da mesma.
    /// </summary>
    Inativo = 1 // 1 é o valor para quando a marca foi descontinuada ou arquivada.
}
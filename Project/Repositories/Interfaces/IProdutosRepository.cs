using FabysUnha.Models;

namespace FabysUnha.Repositories;

/// <summary>
/// Interface para manipulação e acesso aos dados de Produtos.
/// Fornece métodos padronizados para interagir com a tabela de produtos.
/// </summary>
public interface IProdutosRepository
{
    // Leitura de dados

    /// <summary>
    /// Adquire a lista de todos os produtos do inventário/estoque.
    /// </summary>
    /// <returns>Uma enumeração assíncrona dos produtos.</returns>
    Task<IEnumerable<Produtos>> ObterTodosProdutos();

    /// <summary>
    /// Pesquisa um produto específico com base no seu ID.
    /// </summary>
    /// <param name="id">O ID do produto que se quer encontrar.</param>
    /// <returns>Retorna o produto encontrado ou nulo se não localizado.</returns>
    Task<Produtos?> ObterProdutoPorId(int id);

    // Escrita de dados

    /// <summary>
    /// Grava as informações de um novo produto no banco de dados.
    /// </summary>
    /// <param name="produto">O objeto produto a ser armazenado.</param>
    /// <returns>Tarefa assíncrona executando a inserção.</returns>
    Task CriarProduto(Produtos produto);

    /// <summary>
    /// Atualiza o cadastro de um produto, como preço, quantidade em estoque ou descrição.
    /// </summary>
    /// <param name="produto">O produto com os dados devidamente alterados.</param>
    /// <returns>Tarefa assíncrona responsável pela atualização.</returns>
    Task AtualizarProduto(Produtos produto);

    /// <summary>
    /// Deleta o registro de um produto existente no banco de dados.
    /// </summary>
    /// <param name="produto">A entidade produto que deverá ser removida.</param>
    /// <returns>Tarefa assíncrona de exclusão de dados.</returns>
    Task ExcluirProduto(Produtos produto);
}
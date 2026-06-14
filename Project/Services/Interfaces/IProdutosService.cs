using FabysUnha.Models;

using Microsoft.AspNetCore.Http;

namespace FabysUnha.Services;

/// <summary>
/// Contrato de serviço para a gestão de Produtos.
/// Engloba operações de consulta, criação, edição e exclusão de produtos do sistema, 
/// incluindo suporte ao envio de imagens relacionadas aos produtos.
/// </summary>
public interface IProdutosService
{
    /// <summary>
    /// Obtém todos os produtos registrados.
    /// </summary>
    /// <returns>Uma coleção iterável contendo todos os <see cref="Produtos"/>.</returns>
    Task<IEnumerable<Produtos>> ObterTodosProdutos();

    /// <summary>
    /// Localiza um produto pelo seu número identificador (ID).
    /// </summary>
    /// <param name="id">O código único do produto.</param>
    /// <returns>O <see cref="Produtos"/> encontrado, ou nulo caso não exista na base.</returns>
    Task<Produtos?> ObterProdutoPorId(int id);

    /// <summary>
    /// Insere um novo produto no banco de dados, com a possibilidade de salvar uma imagem associada.
    /// </summary>
    /// <param name="produto">As informações base do produto a ser inserido.</param>
    /// <param name="imagemUpload">O arquivo de imagem enviado pelo usuário (opcional).</param>
    /// <returns>Uma tarefa representando a criação do produto.</returns>
    Task CriarProduto(Produtos produto, IFormFile? imagemUpload);

    /// <summary>
    /// Atualiza as características de um produto já cadastrado.
    /// </summary>
    /// <param name="produto">O produto com os dados novos.</param>
    /// <param name="imagemUpload">Um novo arquivo de imagem, caso seja necessário substituir a anterior.</param>
    /// <param name="hasStatusUpdate">Um indicador booleano determinando se o status de disponibilidade do produto foi alterado.</param>
    /// <returns>Uma tarefa que representa a atualização no sistema.</returns>
    Task AtualizarProduto(Produtos produto, IFormFile? imagemUpload, bool hasStatusUpdate);

    /// <summary>
    /// Remove um produto do banco de dados.
    /// </summary>
    /// <param name="id">O código identificador do produto a ser deletado.</param>
    /// <returns>Uma tarefa que cuida da remoção.</returns>
    Task ExcluirProduto(int id);
}
using FabysUnha.Models;

namespace FabysUnha.Services;

/// <summary>
/// Interface que estabelece as operações disponíveis para o gerenciamento de Marcas de produtos.
/// Define os métodos de CRUD (Criar, Ler, Atualizar, Excluir) para as marcas.
/// </summary>
public interface IMarcasService
{
    /// <summary>
    /// Retorna todas as marcas cadastradas na base de dados.
    /// </summary>
    /// <returns>Uma coleção de <see cref="Marcas"/> disponíveis.</returns>
    Task<IEnumerable<Marcas>> ObterTodasMarcas();

    /// <summary>
    /// Busca as informações de uma marca específica através do seu ID.
    /// </summary>
    /// <param name="id">O identificador único da marca a ser localizada.</param>
    /// <returns>A entidade <see cref="Marcas"/> correspondente ao ID informado, ou nulo se não existir.</returns>
    Task<Marcas?> ObterMarcaPorId(int id);

    /// <summary>
    /// Adiciona uma nova marca ao sistema.
    /// </summary>
    /// <param name="marca">A entidade de marca com as informações para criação.</param>
    /// <returns>Uma tarefa assíncrona que executa a operação de inserção.</returns>
    Task CriarMarca(Marcas marca);

    /// <summary>
    /// Modifica os dados de uma marca existente.
    /// </summary>
    /// <param name="marca">O objeto contendo os novos dados da marca.</param>
    /// <returns>Uma tarefa assíncrona representando a atualização.</returns>
    Task AtualizarMarca(Marcas marca);

    /// <summary>
    /// Remove permanentemente uma marca do banco de dados baseada em seu ID.
    /// </summary>
    /// <param name="id">O ID da marca a ser excluída.</param>
    /// <returns>Uma tarefa assíncrona de exclusão.</returns>
    Task ExcluirMarca(int id);
}
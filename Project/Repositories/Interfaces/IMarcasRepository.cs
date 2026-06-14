using FabysUnha.Models;

namespace FabysUnha.Repositories;

/// <summary>
/// Interface que define o repositório de dados para a entidade Marcas.
/// Contém a assinatura dos métodos que devem ser implementados para manipulação de marcas.
/// </summary>
public interface IMarcasRepository
{
    // Métodos de recuperação

    /// <summary>
    /// Retorna todas as marcas de produtos armazenadas no sistema.
    /// </summary>
    /// <returns>Coleção contendo todas as marcas.</returns>
    Task<IEnumerable<Marcas>> ObterTodasMarcas();

    /// <summary>
    /// Recupera uma marca específica utilizando o seu código identificador.
    /// </summary>
    /// <param name="id">O identificador da marca.</param>
    /// <returns>Objeto da marca localizada, caso contrário, nulo.</returns>
    Task<Marcas?> ObterMarcaPorId(int id);

    // Métodos de escrita

    /// <summary>
    /// Salva um novo registro de marca no banco de dados.
    /// </summary>
    /// <param name="marca">A entidade marca a ser adicionada.</param>
    /// <returns>Uma tarefa assíncrona associada à criação.</returns>
    Task CriarMarca(Marcas marca);

    /// <summary>
    /// Atualiza as propriedades de uma marca pré-existente.
    /// </summary>
    /// <param name="marca">A entidade marca contendo os dados atualizados.</param>
    /// <returns>Uma tarefa associada à atualização no banco de dados.</returns>
    Task AtualizarMarca(Marcas marca);

    /// <summary>
    /// Remove um registro de marca do banco de dados permanentemente.
    /// </summary>
    /// <param name="marca">O objeto de marca a ser excluído.</param>
    /// <returns>Uma tarefa indicando o término do processo de exclusão.</returns>
    Task ExcluirMarca(Marcas marca);
}
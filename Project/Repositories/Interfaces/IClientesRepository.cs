using FabysUnha.Models;

namespace FabysUnha.Repositories;

/// <summary>
/// Interface responsável por definir as operações de acesso a dados para a entidade Clientes.
/// </summary>
public interface IClientesRepository
{
    // Métodos de consulta

    /// <summary>
    /// Obtém uma lista contendo todos os clientes cadastrados.
    /// </summary>
    /// <returns>Uma coleção assíncrona com os clientes.</returns>
    Task<IEnumerable<Clientes>> ObterTodosClientes();

    /// <summary>
    /// Busca as informações de um cliente através do seu identificador (ID).
    /// </summary>
    /// <param name="id">O ID do cliente.</param>
    /// <returns>Retorna o cliente encontrado ou nulo se não existir.</returns>
    Task<Clientes?> ObterClientePorId(int id);

    /// <summary>
    /// Busca um cliente pelo seu número de telefone.
    /// Útil para validações antes de cadastrar um novo cliente.
    /// </summary>
    /// <param name="telefone">O telefone do cliente a ser pesquisado.</param>
    /// <returns>O cliente correspondente ao telefone, ou nulo se não houver registro.</returns>
    Task<Clientes?> ObterClientePorTelefone(string telefone);

    // Métodos de manipulação de dados

    /// <summary>
    /// Realiza o registro (cadastro) de um novo cliente no sistema.
    /// </summary>
    /// <param name="cliente">A entidade cliente que será persistida.</param>
    /// <returns>Uma tarefa assíncrona representando a operação de inserção.</returns>
    Task RegistrarCliente(Clientes cliente);

    /// <summary>
    /// Atualiza os dados de um cliente já existente.
    /// </summary>
    /// <param name="cliente">A entidade cliente contendo as alterações.</param>
    /// <returns>Uma tarefa assíncrona que conclui quando a atualização for salva.</returns>
    Task AtualizarCliente(Clientes cliente);

    /// <summary>
    /// Remove permanentemente um cliente do banco de dados.
    /// </summary>
    /// <param name="cliente">A entidade cliente que será removida.</param>
    /// <returns>Uma tarefa assíncrona indicando a conclusão da exclusão.</returns>
    Task ExcluirCliente(Clientes cliente);
}
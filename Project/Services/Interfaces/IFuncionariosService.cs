using FabysUnha.Models;

namespace FabysUnha.Services;

/// <summary>
/// Interface que define o contrato para os serviços relacionados aos Funcionários.
/// Esta interface especifica as operações básicas (CRUD) que podem ser realizadas com as entidades de Funcionários.
/// </summary>
public interface IFuncionariosService
{
    /// <summary>
    /// Obtém de forma assíncrona uma lista de todos os funcionários cadastrados no sistema.
    /// </summary>
    /// <returns>Uma coleção iterável de objetos <see cref="Funcionarios"/>.</returns>
    Task<IEnumerable<Funcionarios>> ObterTodosFuncionarios();

    /// <summary>
    /// Busca um funcionário específico pelo seu identificador (ID).
    /// </summary>
    /// <param name="id">O ID único do funcionário.</param>
    /// <returns>O objeto <see cref="Funcionarios"/> encontrado, ou nulo se não existir.</returns>
    Task<Funcionarios?> ObterFuncionarioPorId(int id);

    /// <summary>
    /// Registra um novo funcionário no sistema.
    /// </summary>
    /// <param name="funcionario">O objeto do funcionário contendo os dados a serem salvos.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona de inserção.</returns>
    Task RegistrarFuncionario(Funcionarios funcionario);

    /// <summary>
    /// Atualiza os dados de um funcionário existente.
    /// </summary>
    /// <param name="funcionario">O objeto do funcionário com as informações atualizadas.</param>
    /// <param name="hasStatusUpdate">Indica se a atualização inclui mudanças no status do funcionário.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona de atualização.</returns>
    Task AtualizarFuncionario(Funcionarios funcionario, bool hasStatusUpdate);

    /// <summary>
    /// Exclui um funcionário do sistema baseado no seu ID.
    /// </summary>
    /// <param name="id">O ID único do funcionário a ser removido.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona de exclusão.</returns>
    Task ExcluirFuncionario(int id);
}
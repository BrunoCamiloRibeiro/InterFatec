using FabysUnha.Models;

namespace FabysUnha.Repositories;

/// <summary>
/// Interface para definir os métodos de repositório relacionados à entidade Funcionarios.
/// Garante o isolamento entre o acesso a dados e a lógica de negócios da aplicação.
/// </summary>
public interface IFuncionariosRepository
{
    // Métodos de busca
    
    /// <summary>
    /// Recupera uma lista com todos os funcionários cadastrados no banco de dados.
    /// </summary>
    /// <returns>Uma coleção de entidades funcionário.</returns>
    Task<IEnumerable<Funcionarios>> ObterTodosFuncionarios();

    /// <summary>
    /// Procura e retorna um funcionário pelo seu identificador (ID).
    /// </summary>
    /// <param name="id">O ID numérico do funcionário.</param>
    /// <returns>O funcionário com o ID correspondente ou nulo.</returns>
    Task<Funcionarios?> ObterFuncionarioPorId(int id);

    // Operações de persistência

    /// <summary>
    /// Insere um novo registro de funcionário no sistema.
    /// </summary>
    /// <param name="funcionario">A entidade funcionário que será salva.</param>
    /// <returns>Uma tarefa que é concluída após a inserção.</returns>
    Task RegistrarFuncionario(Funcionarios funcionario);

    /// <summary>
    /// Aplica alterações aos dados de um funcionário que já existe.
    /// </summary>
    /// <param name="funcionario">Os novos dados do funcionário a serem salvos.</param>
    /// <returns>Uma tarefa assíncrona de atualização no banco de dados.</returns>
    Task AtualizarFuncionario(Funcionarios funcionario);

    /// <summary>
    /// Apaga um registro de funcionário do banco de dados.
    /// </summary>
    /// <param name="funcionario">A entidade correspondente ao funcionário a ser removido.</param>
    /// <returns>Uma tarefa assíncrona indicando a operação de exclusão concluída.</returns>
    Task ExcluirFuncionario(Funcionarios funcionario);
}
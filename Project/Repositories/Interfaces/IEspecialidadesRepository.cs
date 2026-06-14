using FabysUnha.Models;

namespace FabysUnha.Repositories;

/// <summary>
/// Interface que estabelece o contrato de persistência de dados para a entidade Especialidades.
/// </summary>
public interface IEspecialidadeRepository
{
    // Consultas
    
    /// <summary>
    /// Lista todas as especialidades registradas no sistema.
    /// </summary>
    /// <returns>Uma coleção enumerável contendo as especialidades.</returns>
    Task<IEnumerable<Especialidades>> ObterTodasEspecialidades();

    /// <summary>
    /// Busca uma especialidade específica pelo seu identificador único.
    /// </summary>
    /// <param name="id">O identificador da especialidade.</param>
    /// <returns>A especialidade solicitada, ou nulo caso não exista.</returns>
    Task<Especialidades?> ObterEspecialidadePorId(int id);

    // Modificações de estado

    /// <summary>
    /// Cria e armazena uma nova especialidade no banco de dados.
    /// </summary>
    /// <param name="especialidade">O objeto da especialidade a ser adicionada.</param>
    /// <returns>Uma tarefa representando a criação.</returns>
    Task CriarEspecialidade(Especialidades especialidade);

    /// <summary>
    /// Atualiza as informações de uma especialidade existente.
    /// </summary>
    /// <param name="especialidade">A entidade contendo os dados modificados.</param>
    /// <returns>Uma tarefa assíncrona de atualização.</returns>
    Task AtualizarEspecialidade(Especialidades especialidade);

    /// <summary>
    /// Exclui uma especialidade da base de dados.
    /// </summary>
    /// <param name="especialidade">A entidade especialidade a ser apagada.</param>
    /// <returns>Uma tarefa assíncrona de exclusão.</returns>
    Task ExcluirEspecialidade(Especialidades especialidade);
}
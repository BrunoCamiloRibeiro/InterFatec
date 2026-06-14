using FabysUnha.Models;

namespace FabysUnha.Services;

/// <summary>
/// Interface que define o contrato para o serviço de especialidades.
/// Permite o gerenciamento (CRUD e regras de negócio) de especialidades (ex: Manicure, Pedicure, Podologia, etc).
/// </summary>
public interface IEspecialidadeService
{
    /// <summary>
    /// Lista todas as especialidades cadastradas no sistema.
    /// </summary>
    /// <returns>Uma coleção contendo todos os registros de <see cref="Especialidades"/>.</returns>
    Task<IEnumerable<Especialidades>> ObterTodasEspecialidades();

    /// <summary>
    /// Obtém os dados detalhados de uma especialidade específica pelo seu identificador.
    /// </summary>
    /// <param name="id">O ID correspondente à especialidade.</param>
    /// <returns>A entidade <see cref="Especialidades"/> se encontrada, senão null.</returns>
    Task<Especialidades?> ObterEspecialidadePorId(int id);

    /// <summary>
    /// Insere uma nova especialidade na base de dados.
    /// </summary>
    /// <param name="especialidade">O objeto <see cref="Especialidades"/> a ser criado.</param>
    Task CriarEspecialidade(Especialidades especialidade);

    /// <summary>
    /// Atualiza as informações de uma especialidade já existente.
    /// </summary>
    /// <param name="especialidade">A entidade contendo as atualizações realizadas.</param>
    Task AtualizarEspecialidade(Especialidades especialidade);

    /// <summary>
    /// Remove uma especialidade da base de dados através do seu ID.
    /// </summary>
    /// <param name="id">O identificador da especialidade a ser deletada.</param>
    Task ExcluirEspecialidade(int id);
}
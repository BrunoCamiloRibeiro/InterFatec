using FabysUnha.Models;
using FabysUnha.Repositories;

namespace FabysUnha.Services;

/// <summary>
/// Serviço responsável por gerenciar as regras de negócio relacionadas às especialidades.
/// </summary>
public class EspecialidadeService : IEspecialidadeService
{
    private readonly IEspecialidadeRepository _especialidadeRepository;

    /// <summary>
    /// Construtor da classe <see cref="EspecialidadeService"/>.
    /// Inicializa a injeção de dependência do repositório de especialidades.
    /// </summary>
    /// <param name="especialidadeRepository">Instância do repositório de especialidades fornecida pelo contêiner de DI.</param>
    public EspecialidadeService(IEspecialidadeRepository especialidadeRepository)
    {
        // Atribui a instância injetada ao campo privado para uso nos métodos da classe.
        _especialidadeRepository = especialidadeRepository;
    }

    /// <summary>
    /// Obtém todas as especialidades cadastradas no sistema.
    /// </summary>
    /// <returns>Uma coleção de objetos <see cref="Especialidades"/>.</returns>
    public async Task<IEnumerable<Especialidades>> ObterTodasEspecialidades()
    {
        // Delega a busca de todas as especialidades para a camada de repositório e aguarda o resultado.
        return await _especialidadeRepository.ObterTodasEspecialidades();
    }

    /// <summary>
    /// Obtém uma especialidade específica baseada em seu identificador único.
    /// </summary>
    /// <param name="id">O identificador único da especialidade.</param>
    /// <returns>A especialidade encontrada ou nulo se não existir.</returns>
    public async Task<Especialidades?> ObterEspecialidadePorId(int id)
    {
        // Solicita ao repositório a especialidade pelo ID informado.
        return await _especialidadeRepository.ObterEspecialidadePorId(id);
    }

    /// <summary>
    /// Cria uma nova especialidade no banco de dados.
    /// </summary>
    /// <param name="especialidade">O objeto <see cref="Especialidades"/> a ser criado.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    public async Task CriarEspecialidade(Especialidades especialidade)
    {
        // Chama o repositório para persistir a nova especialidade.
        await _especialidadeRepository.CriarEspecialidade(especialidade);
    }

    /// <summary>
    /// Atualiza os dados de uma especialidade existente.
    /// </summary>
    /// <param name="especialidade">O objeto <see cref="Especialidades"/> contendo os dados atualizados.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    public async Task AtualizarEspecialidade(Especialidades especialidade)
    {
        // Passa a especialidade modificada para o repositório atualizar os registros correspondentes.
        await _especialidadeRepository.AtualizarEspecialidade(especialidade);
    }

    /// <summary>
    /// Exclui uma especialidade baseada em seu identificador único.
    /// </summary>
    /// <param name="id">O identificador da especialidade a ser excluída.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    public async Task ExcluirEspecialidade(int id)
    {
        // Primeiro, busca a especialidade pelo ID para garantir que ela existe antes de tentar excluir.
        var especialidade = await _especialidadeRepository.ObterEspecialidadePorId(id);
        
        // Verifica se a especialidade foi encontrada (diferente de nulo).
        if (especialidade != null)
        {
            // Caso exista, solicita ao repositório a exclusão do registro no banco de dados.
            await _especialidadeRepository.ExcluirEspecialidade(especialidade);
        }
    }
}
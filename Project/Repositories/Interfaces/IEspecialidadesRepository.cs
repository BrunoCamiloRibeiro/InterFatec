using FabysUnha.Models;

namespace FabysUnha.Repositories;

public interface IEspecialidadeRepository
{
    Task<IEnumerable<Especialidades>> ObterTodasEspecialidades();
    Task<Especialidades?> ObterEspecialidadePorId(int id);
    Task CriarEspecialidade(Especialidades especialidade);
    Task AtualizarEspecialidade(Especialidades especialidade);
    Task ExcluirEspecialidade(Especialidades especialidade);
}
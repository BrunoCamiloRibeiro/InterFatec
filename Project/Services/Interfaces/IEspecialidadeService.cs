using FabysUnha.Models;

namespace FabysUnha.Services;

public interface IEspecialidadeService
{
    Task<IEnumerable<Especialidades>> ObterTodasEspecialidades();
    Task<Especialidades?> ObterEspecialidadePorId(int id);
    Task CriarEspecialidade(Especialidades especialidade);
    Task AtualizarEspecialidade(Especialidades especialidade);
    Task ExcluirEspecialidade(int id);
}
using FabysUnha.Models;
using FabysUnha.Repositories;

namespace FabysUnha.Services;


public class EspecialidadeService : IEspecialidadeService
{
    private readonly IEspecialidadeRepository _especialidadeRepository;

    public EspecialidadeService(IEspecialidadeRepository especialidadeRepository)
    {
        _especialidadeRepository = especialidadeRepository;
    }

    public async Task<IEnumerable<Especialidades>> ObterTodasEspecialidades()
    {
        return await _especialidadeRepository.ObterTodasEspecialidades();
    }

    public async Task<Especialidades?> ObterEspecialidadePorId(int id)
    {
        return await _especialidadeRepository.ObterEspecialidadePorId(id);
    }

    public async Task CriarEspecialidade(Especialidades especialidade)
    {
        await _especialidadeRepository.CriarEspecialidade(especialidade);
    }

    public async Task AtualizarEspecialidade(Especialidades especialidade)
    {
        await _especialidadeRepository.AtualizarEspecialidade(especialidade);
    }

    public async Task ExcluirEspecialidade(int id)
    {
        var especialidade = await _especialidadeRepository.ObterEspecialidadePorId(id);
        if (especialidade != null)
        {
            await _especialidadeRepository.ExcluirEspecialidade(especialidade);
        }
    }
}
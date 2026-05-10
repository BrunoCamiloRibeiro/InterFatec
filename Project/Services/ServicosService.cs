using FabysUnha.Models;
using FabysUnha.Repositories;

namespace FabysUnha.Services;

public class ServicosService : IServicosService
{
    private readonly IServicosRepository _servicosRepository;

    public ServicosService(IServicosRepository servicosRepository)
    {
        _servicosRepository = servicosRepository;
    }

    public Task<IEnumerable<Servicos>> ObterTodosServicos()
    {
        return _servicosRepository.ObterTodosServicos();
    }

    public Task<Servicos?> ObterServicoPorId(int id)
    {
        return _servicosRepository.ObterServicoPorId(id);
    }

    public Task CriarServico(Servicos servico)
    {
        return _servicosRepository.CriarServico(servico);
    }

    public Task AtualizarServico(Servicos servico)
    {
        return _servicosRepository.AtualizarServico(servico);
    }

    public async Task ExcluirServico(int id)
    {
        var servico = await _servicosRepository.ObterServicoPorId(id);
        if (servico != null)
        {
            await _servicosRepository.ExcluirServico(servico);
        }
    }
}
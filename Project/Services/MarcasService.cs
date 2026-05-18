using FabysUnha.Models;
using FabysUnha.Repositories;

namespace FabysUnha.Services;

public class MarcasService : IMarcasService
{
    private readonly IMarcasRepository _marcasRepository;

    public MarcasService(IMarcasRepository marcasRepository)
    {
        _marcasRepository = marcasRepository;
    }

    public async Task<IEnumerable<Marcas>> ObterTodasMarcas()
    {
        return await _marcasRepository.ObterTodasMarcas();
    }

    public async Task<Marcas?> ObterMarcaPorId(int id)
    {
        return await _marcasRepository.ObterMarcaPorId(id);
    }

    public async Task CriarMarca(Marcas marca)
    {
        await _marcasRepository.CriarMarca(marca);
    }

    public async Task AtualizarMarca(Marcas marca)
    {
        await _marcasRepository.AtualizarMarca(marca);
    }

    public async Task ExcluirMarca(int id)
    {
        var marca = await _marcasRepository.ObterMarcaPorId(id);
        if (marca != null) await _marcasRepository.ExcluirMarca(marca);
    }
}
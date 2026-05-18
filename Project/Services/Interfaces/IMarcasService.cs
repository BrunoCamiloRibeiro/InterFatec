using FabysUnha.Models;

namespace FabysUnha.Services;

public interface IMarcasService
{
    Task<IEnumerable<Marcas>> ObterTodasMarcas();
    Task<Marcas?> ObterMarcaPorId(int id);
    Task CriarMarca(Marcas marca);
    Task AtualizarMarca(Marcas marca);
    Task ExcluirMarca(int id);
}
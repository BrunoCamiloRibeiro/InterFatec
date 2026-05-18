using FabysUnha.Models;

namespace FabysUnha.Repositories;

public interface IMarcasRepository
{
    Task<IEnumerable<Marcas>> ObterTodasMarcas();
    Task<Marcas?> ObterMarcaPorId(int id);
    Task CriarMarca(Marcas marca);
    Task AtualizarMarca(Marcas marca);
    Task ExcluirMarca(Marcas marca);
}
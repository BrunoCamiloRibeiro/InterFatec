using FabysUnha.Models;

namespace FabysUnha.Repositories;

public interface IClientesRepository
{
    Task<IEnumerable<Clientes>> ObterTodosClientes();
    Task<Clientes?> ObterClientePorId(int id);
    Task RegistrarCliente(Clientes cliente);
    Task AtualizarCliente(Clientes cliente);
    Task ExcluirCliente(Clientes cliente);
}
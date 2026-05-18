using FabysUnha.Models;

namespace FabysUnha.Services;

public interface IClientesService
{
    Task<IEnumerable<Clientes>> ObterTodosClientes();
    Task<Clientes?> ObterClientePorId(int id);
    Task RegistrarCliente(Clientes cliente);
    Task AtualizarCliente(Clientes cliente);
    Task ExcluirCliente(int id);
}
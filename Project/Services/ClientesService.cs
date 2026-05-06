using FabysUnha.Models;
using FabysUnha.Repositories;

namespace FabysUnha.Services;

public class ClientesService : IClientesService
{
    private readonly IClientesRepository _clientesRepository;

    public ClientesService(IClientesRepository clientesRepository)
    {
        _clientesRepository = clientesRepository;
    }

    public async Task<IEnumerable<Clientes>> ObterTodosClientes()
    {
        return await _clientesRepository.ObterTodosClientes();
    }

    public async Task<Clientes?> ObterClientePorId(int id)
    {
        return await _clientesRepository.ObterClientePorId(id);
    }

    public async Task RegistrarCliente(Clientes cliente)
    {
        await _clientesRepository.RegistrarCliente(cliente);
    }

    public async Task AtualizarCliente(Clientes cliente)
    {
        await _clientesRepository.AtualizarCliente(cliente);
    }

    public async Task ExcluirCliente(int id)
    {
        var cliente = await _clientesRepository.ObterClientePorId(id);
        if (cliente != null) await _clientesRepository.ExcluirCliente(cliente);
    }
}
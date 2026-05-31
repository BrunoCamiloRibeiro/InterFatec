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
        if (!string.IsNullOrWhiteSpace(cliente.Senha) && !cliente.Senha.StartsWith("$2a$") && !cliente.Senha.StartsWith("$2b$") && !cliente.Senha.StartsWith("$2x$") && !cliente.Senha.StartsWith("$2y$"))
            cliente.Senha = BCrypt.Net.BCrypt.HashPassword(cliente.Senha);

        await _clientesRepository.RegistrarCliente(cliente);
    }

    public async Task AtualizarCliente(Clientes cliente)
    {
        var clienteAtual = await _clientesRepository.ObterClientePorId(cliente.Id);
        if (clienteAtual != null)
        {
            if (string.IsNullOrWhiteSpace(cliente.Senha))
                cliente.Senha = clienteAtual.Senha;
            else if (!cliente.Senha.StartsWith("$2a$") && !cliente.Senha.StartsWith("$2b$") && !cliente.Senha.StartsWith("$2x$") && !cliente.Senha.StartsWith("$2y$"))
                cliente.Senha = BCrypt.Net.BCrypt.HashPassword(cliente.Senha);
        }

        await _clientesRepository.AtualizarCliente(cliente);
    }

    public async Task ExcluirCliente(int id)
    {
        var cliente = await _clientesRepository.ObterClientePorId(id);
        if (cliente != null) await _clientesRepository.ExcluirCliente(cliente);
    }

    public async Task<Clientes?> ObterClientePorTelefone(string telefone)
    {
        return await _clientesRepository.ObterClientePorTelefone(telefone);
    }
}
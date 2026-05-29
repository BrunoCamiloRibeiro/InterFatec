using FabysUnha.Models;

namespace FabysUnha.Services.Interfaces;

public interface IClienteAuthService
{
    /// <summary>
    /// Valida cliente usando Telefone + Senha
    /// </summary>
    Task<(bool Valido, Clientes? Cliente, List<Agendamentos>? Agendamentos)> AutenticarClientePorTelefoneESenha(
        string telefone, 
        string senha);

    /// <summary>
    /// Retorna lista de agendamentos para um cliente (validado por telefone)
    /// </summary>
    Task<List<Agendamentos>?> ObterAgendamentosCliente(string telefone, int clienteId);
}

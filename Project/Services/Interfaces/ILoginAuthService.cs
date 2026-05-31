using FabysUnha.Models;
using FabysUnha.ViewModels;

namespace FabysUnha.Services.Interfaces;

public interface ILoginAuthService
{
    Task<(bool Valido, Clientes? Cliente, List<Agendamentos>? Agendamentos)> AutenticarClientePorTelefoneESenha(string telefone, string senha);
    Task<List<Agendamentos>?> ObterAgendamentosCliente(string telefone, int clienteId);
    Task<Funcionarios?> AutenticarFuncionario(string telefone, string senha);
}

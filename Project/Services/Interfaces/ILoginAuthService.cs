using FabysUnha.Models;
using FabysUnha.ViewModels;

namespace FabysUnha.Services.Interfaces;

/// <summary>
/// Interface responsável por definir o contrato dos serviços de autenticação (Login) de clientes e funcionários.
/// Utilizada para garantir o acesso seguro ao sistema.
/// </summary>
public interface ILoginAuthService
{
    /// <summary>
    /// Autentica um cliente utilizando seu número de telefone e senha.
    /// </summary>
    /// <param name="telefone">O número de telefone registrado do cliente.</param>
    /// <param name="senha">A senha fornecida pelo cliente para verificação.</param>
    /// <returns>Uma tupla contendo um booleano indicando se a autenticação foi válida, os dados do cliente e a sua lista de agendamentos associados.</returns>
    Task<(bool Valido, Clientes? Cliente, List<Agendamentos>? Agendamentos)> AutenticarClientePorTelefoneESenha(string telefone, string senha);

    /// <summary>
    /// Recupera os agendamentos de um cliente específico após a autenticação.
    /// </summary>
    /// <param name="telefone">O telefone do cliente.</param>
    /// <param name="clienteId">O identificador único do cliente.</param>
    /// <returns>Uma lista de <see cref="Agendamentos"/> vinculada ao cliente, ou nulo se não houver registros.</returns>
    Task<List<Agendamentos>?> ObterAgendamentosCliente(string telefone, int clienteId);

    /// <summary>
    /// Autentica um funcionário no sistema validando seu telefone e senha.
    /// </summary>
    /// <param name="telefone">O número de telefone do funcionário.</param>
    /// <param name="senha">A senha fornecida para validação de acesso.</param>
    /// <returns>Os dados do <see cref="Funcionarios"/> caso a autenticação seja bem-sucedida, ou nulo se falhar.</returns>
    Task<Funcionarios?> AutenticarFuncionario(string telefone, string senha);
}

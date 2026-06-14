using FabysUnha.Models;

namespace FabysUnha.Services;

/// <summary>
/// Interface que define o contrato para o serviço de clientes.
/// Fornece abstração sobre as operações e regras de negócio para a entidade <see cref="Clientes"/>.
/// </summary>
public interface IClientesService
{
    /// <summary>
    /// Obtém a lista completa de todos os clientes cadastrados.
    /// </summary>
    /// <returns>Uma coleção de entidades <see cref="Clientes"/>.</returns>
    Task<IEnumerable<Clientes>> ObterTodosClientes();

    /// <summary>
    /// Busca um cliente através do seu identificador único.
    /// </summary>
    /// <param name="id">O ID do cliente a ser buscado.</param>
    /// <returns>O objeto <see cref="Clientes"/> se encontrado; caso contrário, null.</returns>
    Task<Clientes?> ObterClientePorId(int id);

    /// <summary>
    /// Registra e persiste um novo cliente no sistema após validar possíveis regras de negócio.
    /// </summary>
    /// <param name="cliente">A entidade <see cref="Clientes"/> com os dados do novo cliente.</param>
    Task RegistrarCliente(Clientes cliente);

    /// <summary>
    /// Atualiza os dados cadastrais de um cliente existente no sistema.
    /// </summary>
    /// <param name="cliente">A entidade <see cref="Clientes"/> contendo as modificações a serem salvas.</param>
    Task AtualizarCliente(Clientes cliente);

    /// <summary>
    /// Remove um cliente do sistema com base em seu ID.
    /// </summary>
    /// <param name="id">O identificador do cliente que será excluído.</param>
    Task ExcluirCliente(int id);

    /// <summary>
    /// Busca um cliente especificamente pelo seu número de telefone.
    /// Essa busca é útil para sistemas de login, recuperação de conta ou verificação de duplicidade.
    /// </summary>
    /// <param name="telefone">O telefone (formatado ou não, conforme regra do sistema) do cliente.</param>
    /// <returns>A entidade <see cref="Clientes"/> correspondente ou null se o telefone não existir na base.</returns>
    Task<Clientes?> ObterClientePorTelefone(string telefone);
}
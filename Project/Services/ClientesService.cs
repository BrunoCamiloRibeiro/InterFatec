using FabysUnha.Models;
using FabysUnha.Repositories;

namespace FabysUnha.Services;

/// <summary>
/// Serviço de negócios responsável por aplicar as regras inerentes à entidade Cliente.
/// Ele intermedeia a comunicação entre as requisições do sistema e os repositórios (acesso a dados).
/// </summary>
public class ClientesService : IClientesService
{
    private readonly IClientesRepository _clientesRepository;

    /// <summary>
    /// Construtor para injeção de dependência do repositório de clientes.
    /// </summary>
    /// <param name="clientesRepository">A interface do repositório responsável pelas operações no banco de dados.</param>
    public ClientesService(IClientesRepository clientesRepository)
    {
        // Atribui a instância injetada para a variável privada que será utilizada em toda a classe
        _clientesRepository = clientesRepository;
    }

    /// <summary>
    /// Solicita ao repositório a listagem completa dos clientes cadastrados.
    /// </summary>
    /// <returns>Uma lista iterável de todos os clientes no sistema.</returns>
    public async Task<IEnumerable<Clientes>> ObterTodosClientes()
    {
        // Delega a busca ao repositório e aguarda o resultado para retorná-lo
        return await _clientesRepository.ObterTodosClientes();
    }

    /// <summary>
    /// Realiza a busca de um cliente específico com base em seu ID.
    /// </summary>
    /// <param name="id">Identificador único do cliente.</param>
    /// <returns>O cliente correspondente, caso exista.</returns>
    public async Task<Clientes?> ObterClientePorId(int id)
    {
        // Retorna o cliente achado pelo repositório utilizando a chave primária
        return await _clientesRepository.ObterClientePorId(id);
    }

    /// <summary>
    /// Efetua o registro de um novo cliente após processar e proteger a senha informada.
    /// </summary>
    /// <param name="cliente">Objeto contendo as informações do cliente para ser salvo.</param>
    /// <returns>Uma tarefa de execução da gravação.</returns>
    public async Task RegistrarCliente(Clientes cliente)
    {
        // Verifica se a senha do cliente foi preenchida e se NÃO foi criptografada previamente (não inicia com o prefixo comum do BCrypt como $2a$, $2b$, etc)
        if (!string.IsNullOrWhiteSpace(cliente.Senha) && !cliente.Senha.StartsWith("$2a$") && !cliente.Senha.StartsWith("$2b$") && !cliente.Senha.StartsWith("$2x$") && !cliente.Senha.StartsWith("$2y$"))
            // Criptografa a senha em formato texto plano usando o algoritmo HashPassword do BCrypt.Net para segurança
            cliente.Senha = BCrypt.Net.BCrypt.HashPassword(cliente.Senha);

        // Repassa a entidade já segura para o repositório finalizar o processo de inserção no banco de dados
        await _clientesRepository.RegistrarCliente(cliente);
    }

    /// <summary>
    /// Processa a edição dos dados de um cliente existente. Mantém a senha antiga se não for alterada ou aplica hash à nova senha.
    /// </summary>
    /// <param name="cliente">Entidade de cliente vinda com dados alterados (normalmente da view de edição).</param>
    /// <returns>Tarefa de atualização.</returns>
    public async Task AtualizarCliente(Clientes cliente)
    {
        // Busca os dados atuais do cliente antes das alterações para fins de comparação, especialmente em relação à senha
        var clienteAtual = await _clientesRepository.ObterClientePorId(cliente.Id);
        
        // Assegura que o cliente que estamos tentando atualizar realmente existe
        if (clienteAtual != null)
        {
            // Lógica para controle da senha: se a senha não for informada na atualização, mantemos a antiga
            if (string.IsNullOrWhiteSpace(cliente.Senha))
                cliente.Senha = clienteAtual.Senha;
            // Caso contrário, se uma senha foi fornecida e não aparenta ter hash BCrypt aplicado (verificando os prefixos padrão)
            else if (!cliente.Senha.StartsWith("$2a$") && !cliente.Senha.StartsWith("$2b$") && !cliente.Senha.StartsWith("$2x$") && !cliente.Senha.StartsWith("$2y$"))
                // Gera o hash de proteção para a nova senha em texto plano digitada pelo usuário
                cliente.Senha = BCrypt.Net.BCrypt.HashPassword(cliente.Senha);
        }

        // Chama o repositório para efetivar as atualizações da entidade no banco de dados
        await _clientesRepository.AtualizarCliente(cliente);
    }

    /// <summary>
    /// Solicita a exclusão de um registro de cliente pelo seu ID.
    /// </summary>
    /// <param name="id">O identificador do cliente a ser removido do banco.</param>
    /// <returns>Tarefa que executa a remoção.</returns>
    public async Task ExcluirCliente(int id)
    {
        // Antes de tentar deletar, precisamos obter o registro do cliente usando seu ID
        var cliente = await _clientesRepository.ObterClientePorId(id);
        
        // Se o cliente for encontrado com sucesso, procedemos mandando o repositório excluir o objeto
        if (cliente != null) 
            await _clientesRepository.ExcluirCliente(cliente);
    }

    /// <summary>
    /// Obtém os dados de um cliente buscando-o através do seu número de telefone.
    /// Muito utilizado em fluxos de autenticação (login) ou validação de usuários.
    /// </summary>
    /// <param name="telefone">Telefone de busca cadastrado do cliente.</param>
    /// <returns>Retorna o cliente que possuir este número de telefone ou nulo.</returns>
    public async Task<Clientes?> ObterClientePorTelefone(string telefone)
    {
        // Transfere o trabalho da query de busca por telefone ao respectivo método no repositório de clientes
        return await _clientesRepository.ObterClientePorTelefone(telefone);
    }
}
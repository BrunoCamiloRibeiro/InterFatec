using FabysUnha.Models;
using FabysUnha.Repositories;

namespace FabysUnha.Services;

/// <summary>
/// Serviço responsável pela lógica de negócios envolvendo os funcionários.
/// </summary>
public class FuncionariosService : IFuncionariosService
{
    private readonly IFuncionariosRepository _funcionariosRepository;

    /// <summary>
    /// Construtor que recebe a injeção de dependência do repositório de funcionários.
    /// </summary>
    /// <param name="funcionariosRepository">Repositório injetado de funcionários.</param>
    public FuncionariosService(IFuncionariosRepository funcionariosRepository)
    {
        // Associa o repositório recebido à variável local.
        _funcionariosRepository = funcionariosRepository;
    }

    /// <summary>
    /// Retorna todos os funcionários cadastrados.
    /// </summary>
    /// <returns>Uma lista iterável de <see cref="Funcionarios"/>.</returns>
    public async Task<IEnumerable<Funcionarios>> ObterTodosFuncionarios()
    {
        // Aciona o repositório para realizar a consulta no banco de dados.
        return await _funcionariosRepository.ObterTodosFuncionarios();
    }

    /// <summary>
    /// Busca um funcionário específico pelo seu identificador (ID).
    /// </summary>
    /// <param name="id">ID numérico do funcionário.</param>
    /// <returns>A entidade do funcionário, se encontrada; caso contrário, nulo.</returns>
    public async Task<Funcionarios?> ObterFuncionarioPorId(int id)
    {
        // Executa a busca através do repositório.
        return await _funcionariosRepository.ObterFuncionarioPorId(id);
    }

    /// <summary>
    /// Registra um novo funcionário, aplicando as regras de negócio necessárias, como hash de senha.
    /// </summary>
    /// <param name="funcionario">O objeto do funcionário a ser inserido.</param>
    public async Task RegistrarFuncionario(Funcionarios funcionario)
    {
        // Verifica se a senha não é nula/vazia e também checa se já não está codificada pelo formato do BCrypt.
        if (!string.IsNullOrWhiteSpace(funcionario.Senha) && 
            !funcionario.Senha.StartsWith("$2a$") && 
            !funcionario.Senha.StartsWith("$2b$") && 
            !funcionario.Senha.StartsWith("$2x$") && 
            !funcionario.Senha.StartsWith("$2y$"))
        {
            // Cria um hash (criptografia) da senha antes de salvá-la no banco de dados para garantir a segurança.
            funcionario.Senha = BCrypt.Net.BCrypt.HashPassword(funcionario.Senha);
        }

        // Chama o repositório para salvar efetivamente o funcionário.
        await _funcionariosRepository.RegistrarFuncionario(funcionario);
    }

    /// <summary>
    /// Atualiza as informações de um funcionário existente aplicando regras de validação.
    /// </summary>
    /// <param name="funcionario">Objeto do funcionário com os dados modificados.</param>
    /// <param name="hasStatusUpdate">Indicador de permissão para atualização do status (Ativo/Inativo).</param>
    public async Task AtualizarFuncionario(Funcionarios funcionario, bool hasStatusUpdate)
    {
        // Validação de negócio: o salário deve ser maior que zero.
        if(funcionario.Salario <= 0) throw new ArgumentException("O salário deve ser um valor positivo.");
        
        // Validação de negócio: o salário não pode ser inferior ao salário mínimo estabelecido.
        if(funcionario.Salario < 1412.00m) throw new ArgumentException("O salário deve ser no mínimo o valor do salário mínimo.");

        // Busca o funcionário atual no banco de dados para comparar as informações antigas com as novas.
        var funcionarioAtual = await _funcionariosRepository.ObterFuncionarioPorId(funcionario.Id);
        if (funcionarioAtual == null) throw new ArgumentException("Funcionário não encontrado.");

        // Se a operação não contemplar uma atualização de status, mantém o status original do banco de dados.
        if (!hasStatusUpdate)
        {
            funcionario.Status = funcionarioAtual.Status;
        }

        // Tratamento da senha durante a atualização.
        if (string.IsNullOrWhiteSpace(funcionario.Senha))
        {
            // Se a senha não foi informada na atualização, mantém a senha antiga que já estava no banco.
            funcionario.Senha = funcionarioAtual.Senha;
        }
        else if (!funcionario.Senha.StartsWith("$2a$") && 
                 !funcionario.Senha.StartsWith("$2b$") && 
                 !funcionario.Senha.StartsWith("$2x$") && 
                 !funcionario.Senha.StartsWith("$2y$"))
        {
            // Se uma nova senha em texto claro foi informada, aplica o hash antes de persistir.
            funcionario.Senha = BCrypt.Net.BCrypt.HashPassword(funcionario.Senha);
        }

        // Envia o objeto validado para ser atualizado pelo repositório.
        await _funcionariosRepository.AtualizarFuncionario(funcionario);
    }

    /// <summary>
    /// Exclui um funcionário pelo seu ID.
    /// </summary>
    /// <param name="id">O identificador do funcionário.</param>
    public async Task ExcluirFuncionario(int id)
    {
        // Localiza o funcionário antes de tentar excluí-lo.
        var funcionario = await _funcionariosRepository.ObterFuncionarioPorId(id);
        
        // Se encontrou o funcionário no banco de dados, prossegue com a exclusão.
        if (funcionario != null)  
        {
            await _funcionariosRepository.ExcluirFuncionario(funcionario);        
        }
    }
}
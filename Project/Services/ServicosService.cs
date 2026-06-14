using FabysUnha.Models;
using FabysUnha.Repositories;

namespace FabysUnha.Services;

/// <summary>
/// Serviço de gerenciamento da lógica de negócio ligada aos serviços prestados (Ex.: manicure, pedicure, etc.).
/// </summary>
public class ServicosService : IServicosService
{
    private readonly IServicosRepository _servicosRepository;

    /// <summary>
    /// Construtor que possibilita a injeção do repositório da entidade Serviço.
    /// </summary>
    /// <param name="servicosRepository">A interface injetada do repositório responsável pelo acesso a dados.</param>
    public ServicosService(IServicosRepository servicosRepository)
    {
        // Salva a injeção do repositório em uma variável de escopo global para consumo da classe.
        _servicosRepository = servicosRepository;
    }

    /// <summary>
    /// Lista todos os tipos de serviços oferecidos pelo estabelecimento.
    /// </summary>
    /// <returns>Uma <see cref="IEnumerable{T}"/> contendo a relação dos serviços.</returns>
    public Task<IEnumerable<Servicos>> ObterTodosServicos()
    {
        // Neste caso a resposta da camada de dados não está com await explícito porque a assinatura devolve diretamente a própria Task.
        return _servicosRepository.ObterTodosServicos();
    }

    /// <summary>
    /// Obtém os dados de um serviço em específico conforme seu ID.
    /// </summary>
    /// <param name="id">A chave primária do serviço a ser localizado.</param>
    /// <returns>Retorna a entidade <see cref="Servicos"/> caso encontrado ou nulo.</returns>
    public Task<Servicos?> ObterServicoPorId(int id)
    {
        // Repassa a busca do ID para o repositório tratar no SQL.
        return _servicosRepository.ObterServicoPorId(id);
    }

    /// <summary>
    /// Armazena um novo serviço de forma assíncrona.
    /// </summary>
    /// <param name="servico">A entidade com as configurações do serviço desejado a ser cadastrado.</param>
    /// <returns>Uma <see cref="Task"/> finalizada após a instrução no repositório concluir.</returns>
    public Task CriarServico(Servicos servico)
    {
        // Concede ao repositório a responsabilidade da inserção no banco de dados.
        return _servicosRepository.CriarServico(servico);
    }

    /// <summary>
    /// Atualiza as definições, preço, ou descrições de um serviço previamente cadastrado.
    /// </summary>
    /// <param name="servico">A entidade alterada vinda do cliente/front-end.</param>
    /// <returns>Uma tarefa assíncrona indicando conclusão do update.</returns>
    public Task AtualizarServico(Servicos servico)
    {
        // Transmite o objeto alterado para salvar no contexto do banco.
        return _servicosRepository.AtualizarServico(servico);
    }

    /// <summary>
    /// Efetua a remoção de um serviço pelo ID.
    /// </summary>
    /// <param name="id">O ID correspondente ao serviço que o usuário optou por deletar.</param>
    public async Task ExcluirServico(int id)
    {
        // Consulta ao banco se o elemento alvo de fato ainda existe no contexto.
        var servico = await _servicosRepository.ObterServicoPorId(id);
        
        // Bloqueia tentativas de excluir o que não existe, ignorando ou prevenindo excepções nulas.
        if (servico != null)
        {
            // Aciona o comportamento de exclusão física ou lógica implementada no Entity Framework.
            await _servicosRepository.ExcluirServico(servico);
        }
    }
}
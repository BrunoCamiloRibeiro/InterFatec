using FabysUnha.Models;

namespace FabysUnha.Repositories;

/// <summary>
/// Interface responsável por definir os contratos de acesso a dados para a entidade Agendamentos.
/// Fornece métodos para realizar as operações de CRUD (Criar, Ler, Atualizar e Excluir)
/// e buscas específicas relacionadas aos agendamentos.
/// </summary>
public interface IAgendamentosRepository
{
    // Métodos de leitura (Read)
    
    /// <summary>
    /// Obtém todos os agendamentos registrados no sistema.
    /// </summary>
    /// <returns>Uma coleção assíncrona contendo todos os agendamentos.</returns>
    Task<IEnumerable<Agendamentos>> ObterTodosAgendamentos();

    /// <summary>
    /// Busca um agendamento específico com base no seu identificador único.
    /// </summary>
    /// <param name="id">O ID do agendamento a ser buscado.</param>
    /// <returns>O agendamento correspondente ou nulo se não for encontrado.</returns>
    Task<Agendamentos?> ObterAgendamentoPorId(int id);

    /// <summary>
    /// Obtém a lista de agendamentos associados a um determinado cliente.
    /// </summary>
    /// <param name="clienteId">O identificador único do cliente.</param>
    /// <returns>Uma coleção de agendamentos do cliente informado.</returns>
    Task<IEnumerable<Agendamentos>> ObterAgendamentosPorCliente(int clienteId);

    // Métodos de gravação (Create, Update, Delete)

    /// <summary>
    /// Cria e salva um novo agendamento no banco de dados.
    /// </summary>
    /// <param name="agendamento">A entidade agendamento contendo os dados a serem salvos.</param>
    /// <returns>Uma tarefa assíncrona representando a operação de criação.</returns>
    Task CriarAgendamento(Agendamentos agendamento);

    /// <summary>
    /// Atualiza as informações de um agendamento existente no banco de dados.
    /// </summary>
    /// <param name="agendamento">A entidade agendamento com os dados atualizados.</param>
    /// <returns>Uma tarefa assíncrona representando a operação de atualização.</returns>
    Task AtualizarAgendamento(Agendamentos agendamento);

    /// <summary>
    /// Remove um agendamento do banco de dados.
    /// </summary>
    /// <param name="agendamento">A entidade agendamento a ser excluída.</param>
    /// <returns>Uma tarefa assíncrona representando a operação de exclusão.</returns>
    Task ExcluirAgendamento(Agendamentos agendamento);

    // Métodos de utilidade
    
    /// <summary>
    /// Obtém uma lista de horários (TimeSpan) que já estão ocupados por um determinado funcionário em uma data específica.
    /// Utilizado para evitar agendamentos simultâneos para o mesmo profissional.
    /// </summary>
    /// <param name="funcionarioId">O identificador único do funcionário.</param>
    /// <param name="data">A data em que se deseja verificar os horários ocupados.</param>
    /// <returns>Uma lista de horários ocupados no dia especificado para o funcionário.</returns>
    Task<List<TimeSpan>> ObterHorariosOcupados(int funcionarioId, DateTime data);
}
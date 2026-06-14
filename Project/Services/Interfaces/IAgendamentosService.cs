using FabysUnha.Models;
using FabysUnha.ViewModels;

namespace FabysUnha.Services;

/// <summary>
/// Interface que define o contrato para o serviço de agendamentos.
/// Responsável por centralizar as regras de negócio e orquestração de operações relacionadas a agendamentos.
/// </summary>
public interface IAgendamentosService
{
    /// <summary>
    /// Obtém a lista de todos os agendamentos registrados no sistema.
    /// </summary>
    /// <returns>Uma coleção de entidades <see cref="Agendamentos"/>.</returns>
    Task<IEnumerable<Agendamentos>> ObterTodosAgendamentos();

    /// <summary>
    /// Busca um agendamento específico com base no seu número identificador (NR).
    /// </summary>
    /// <param name="nr">O número identificador único do agendamento.</param>
    /// <returns>O <see cref="Agendamentos"/> correspondente ou null se não for encontrado.</returns>
    Task<Agendamentos?> ObterAgendamentoPorId(int nr);

    /// <summary>
    /// Retorna todos os agendamentos vinculados a um cliente específico.
    /// </summary>
    /// <param name="clienteId">O identificador do cliente.</param>
    /// <returns>Uma coleção de agendamentos associados ao cliente especificado.</returns>
    Task<IEnumerable<Agendamentos>> ObterAgendamentosPorCliente(int clienteId);

    /// <summary>
    /// Cria um novo agendamento com base nos dados fornecidos pela ViewModel.
    /// </summary>
    /// <param name="viewModel">Objeto contendo os dados necessários para o registro do agendamento.</param>
    Task CriarAgendamento(AgendamentoRegistroViewModel viewModel);

    /// <summary>
    /// Atualiza as informações de um agendamento existente utilizando uma ViewModel específica para edição.
    /// </summary>
    /// <param name="viewModel">Objeto contendo os dados atualizados do agendamento.</param>
    Task AtualizarAgendamento(AgendamentoEditarViewModel viewModel);
    
    /// <summary>
    /// Cancela um agendamento, modificando seu status em vez de excluí-lo permanentemente.
    /// </summary>
    /// <param name="nr">O número identificador do agendamento a ser cancelado.</param>
    Task CancelarAgendamento(int nr);

    /// <summary>
    /// Marca o agendamento como finalizado ou concluído.
    /// </summary>
    /// <param name="nr">O número identificador do agendamento a ser finalizado.</param>
    Task FinalizarAgendamento(int nr);

    /// <summary>
    /// Exclui um agendamento fisicamente do banco de dados.
    /// </summary>
    /// <param name="nr">O número identificador do agendamento a ser excluído.</param>
    Task ExcluirAgendamento(int nr);

    /// <summary>
    /// Registra um agendamento especificamente para o fluxo de uso de um cliente do sistema.
    /// </summary>
    /// <param name="viewModel">Os dados de agendamento selecionados pelo cliente.</param>
    /// <param name="clienteId">O ID do cliente autenticado realizando a ação.</param>
    Task CriarAgendamentoCliente(AgendamentoClienteViewModel viewModel, int clienteId);

    /// <summary>
    /// Consulta os horários de trabalho disponíveis para um determinado funcionário em uma data específica.
    /// </summary>
    /// <param name="funcionarioId">O identificador do funcionário.</param>
    /// <param name="data">A data a ser verificada.</param>
    /// <returns>Uma lista de objetos <see cref="TimeSpan"/> representando os horários livres na agenda do funcionário.</returns>
    Task<List<TimeSpan>> ObterHorariosDisponiveis(int funcionarioId, DateTime data);
}
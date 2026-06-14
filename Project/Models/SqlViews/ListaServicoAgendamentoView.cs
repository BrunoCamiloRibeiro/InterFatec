namespace FabysUnha.Models.SqlViews;

/// <summary>
/// Classe responsável por mapear a view do banco de dados que lista os serviços dentro de cada agendamento.
/// Entidade de configuração Keyless para exibição de relatórios e detalhes.
/// </summary>
public class ListaServicoAgendamentoView
{
    /// <summary>
    /// Obtém ou define o número do agendamento principal.
    /// </summary>
    // Associa este registro de serviço a uma reserva ou atendimento específico.
    public int NumeroAgendamento { get; set; }

    /// <summary>
    /// Obtém ou define o nome ou título do serviço prestado.
    /// </summary>
    // Descreve qual procedimento foi agendado (ex: Manicure, Spa dos pés).
    public string NomeServico { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define uma observação particular referente a este serviço.
    /// </summary>
    // Notas de atendimento inseridas pelo profissional ou recepcionista.
    public string Observacao { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o horário em que o serviço está programado para acontecer.
    /// </summary>
    // Utiliza TimeSpan para armazenar a fração de tempo correspondente à hora do dia.
    public TimeSpan Horario { get; set; }

    /// <summary>
    /// Obtém ou define o nome do funcionário responsável por executar o serviço.
    /// </summary>
    // Informação já cruzada a partir da tabela de funcionários na view.
    public string Funcionario { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o valor financeiro estipulado para o serviço.
    /// </summary>
    // Custo individual deste serviço dentro do total do agendamento.
    public decimal Valor { get; set; }
}
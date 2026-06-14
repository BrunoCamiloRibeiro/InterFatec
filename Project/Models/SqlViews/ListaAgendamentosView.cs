namespace FabysUnha.Models.SqlViews;

/// <summary>
/// Classe de modelo que representa a view de listagem de agendamentos.
/// Utilizada pelo Entity Framework Core como um Keyless Entity Type para mapear a view correspondente no banco.
/// </summary>
public class ListaAgendamentosView
{
    /// <summary>
    /// Obtém ou define o número identificador do agendamento.
    /// </summary>
    // Usado como chave visual ou identificador primário no contexto da view.
    public int NumeroAgendamento { get; set; }

    /// <summary>
    /// Obtém ou define a data em que o agendamento está marcado.
    /// </summary>
    // Armazena a data e a hora do agendamento.
    public DateTime Data { get; set; }

    /// <summary>
    /// Obtém ou define o valor total do agendamento.
    /// </summary>
    // Representa a soma dos valores dos serviços/produtos do agendamento.
    public decimal Total { get; set; }

    /// <summary>
    /// Obtém ou define o nome do cliente associado ao agendamento.
    /// </summary>
    // Inicializa a string como vazia para manter a consistência e evitar valores nulos.
    public string Cliente { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o código do status do agendamento.
    /// </summary>
    // Representa o status interno (ex: 1 para Pendente, 2 para Concluído, etc).
    public int Status { get; set; }

    /// <summary>
    /// Obtém ou define a descrição amigável do status do agendamento.
    /// </summary>
    // Texto descritivo que será exibido para o usuário final.
    public string StatusDescricao { get; set; } = string.Empty;
}
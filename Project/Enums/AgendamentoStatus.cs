namespace FabysUnha.Enums;

/// <summary>
/// Enumerador que define os possíveis estados de um agendamento no sistema.
/// </summary>
/// <remarks>
/// Enums (enumerações) são tipos de valor que consistem em um conjunto de constantes nomeadas.
/// Eles tornam o código mais legível e evitam erros de "números mágicos" no código.
/// </remarks>
public enum AgendamentoStatus
{
    /// <summary>
    /// Indica que o agendamento foi realizado, mas o serviço ainda não aconteceu.
    /// </summary>
    Pendente = 0, // 0 é o valor padrão (default) atribuído ao primeiro item, facilitando sua inicialização.

    /// <summary>
    /// Indica que o agendamento foi cancelado (pelo cliente ou pelo estabelecimento).
    /// </summary>
    Cancelado = 1, // O valor 1 é associado para representar o cancelamento no banco de dados.

    /// <summary>
    /// Indica que o serviço já foi concluído e o agendamento encerrado.
    /// </summary>
    Finalizado = 2 // O valor 2 representa o fluxo finalizado com sucesso do agendamento.
}
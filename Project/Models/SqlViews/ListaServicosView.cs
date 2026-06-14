namespace FabysUnha.Models.SqlViews;

/// <summary>
/// Modelo da view SQL de listagem geral de serviços prestados pelo salão.
/// Mapeado no DbContext do EF Core como uma entidade que não possui chave primária (Keyless).
/// </summary>
public class ListaServicosView
{
    /// <summary>
    /// Obtém ou define o identificador do serviço.
    /// </summary>
    // Código único vindo da tabela base de serviços.
    public int Id { get; set; }

    /// <summary>
    /// Obtém ou define a descrição detalhada do serviço oferecido.
    /// </summary>
    // Nome e possivelmente o detalhamento do procedimento (ex: Unha em Gel).
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o preço cobrado pelo serviço.
    /// </summary>
    // Valor base comercial utilizado como referência no agendamento.
    public decimal Preco { get; set; }

    /// <summary>
    /// Obtém ou define o tempo estimado de duração para a execução do serviço.
    /// </summary>
    // TimeSpan é usado para facilitar o cálculo da agenda dos funcionários.
    public TimeSpan Tempo { get; set; }

    /// <summary>
    /// Obtém ou define o código correspondente ao status do serviço.
    /// </summary>
    // Verifica se o serviço ainda é oferecido (ex: 1 para Ativo, 2 para Inativo).
    public int StatusId { get; set; }

    /// <summary>
    /// Obtém ou define a descrição formatada do status.
    /// </summary>
    // Status retornado como texto pronto para a visualização nas tabelas de interface.
    public string StatusDescricao { get; set; } = string.Empty;
}
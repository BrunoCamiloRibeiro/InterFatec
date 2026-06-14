namespace FabysUnha.Models.SqlViews;

/// <summary>
/// Representa a view SQL que une informações de agendamentos e os produtos utilizados neles.
/// Mapeada no EF Core como uma entidade sem chave para consultas read-only.
/// </summary>
public class ListaProdutoAgendamentoView
{
    /// <summary>
    /// Obtém ou define o número do agendamento ao qual o produto está vinculado.
    /// </summary>
    // Utilizado para agrupar e listar os produtos consumidos em um mesmo agendamento.
    public int NumeroAgendamento { get; set; }

    /// <summary>
    /// Obtém ou define o nome do serviço realizado que consumiu o produto.
    /// </summary>
    // Traz o contexto do serviço atrelado a este agendamento.
    public string NomeServico { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define observações extras sobre o serviço ou uso do produto.
    /// </summary>
    // Detalhes adicionais informados durante o agendamento ou execução do serviço.
    public string Observacao { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o nome comercial do produto utilizado.
    /// </summary>
    // Descrição do produto (ex: Esmalte, Base, Removedor).
    public string NomeProduto { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define a marca do produto consumido.
    /// </summary>
    // Importante para controle de qualidade e preferência do cliente.
    public string Marca { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o preço cobrado ou custo do produto naquele momento.
    /// </summary>
    // Valor financeiro que impacta o total do agendamento ou controle de estoque.
    public decimal Preco { get; set; }
}
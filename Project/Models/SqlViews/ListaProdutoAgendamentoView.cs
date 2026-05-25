namespace FabysUnha.Models.SqlViews;

public class ListaProdutoAgendamentoView
{
    public int NumeroAgendamento { get; set; }
    public string NomeServico { get; set; } = string.Empty;
    public string Observacao { get; set; } = string.Empty;
    public string NomeProduto { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public decimal Preco { get; set; }
}
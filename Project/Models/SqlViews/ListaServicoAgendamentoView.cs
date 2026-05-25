namespace FabysUnha.Models.SqlViews;

public class ListaServicoAgendamentoView
{
    public int NumeroAgendamento { get; set; }
    public string NomeServico { get; set; } = string.Empty;
    public string Observacao { get; set; } = string.Empty;
    public TimeSpan Horario { get; set; }
    public string Funcionario { get; set; } = string.Empty;
    public decimal Valor { get; set; }
}
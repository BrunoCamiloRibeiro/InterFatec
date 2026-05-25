namespace FabysUnha.Models.SqlViews;

public class ListaAgendamentosView
{
    public int NumeroAgendamento { get; set; }
    public DateTime Data { get; set; }
    public decimal Total { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public int Status { get; set; }
    public string StatusDescricao { get; set; } = string.Empty;
}
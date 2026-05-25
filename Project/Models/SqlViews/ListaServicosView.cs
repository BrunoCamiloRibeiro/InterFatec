namespace FabysUnha.Models.SqlViews;

public class ListaServicosView
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public TimeSpan Tempo { get; set; }
    public int StatusId { get; set; }
    public string StatusDescricao { get; set; } = string.Empty;
}
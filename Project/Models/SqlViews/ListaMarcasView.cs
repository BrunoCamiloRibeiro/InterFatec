namespace FabysUnha.Models.SqlViews;

public class ListaMarcasView
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Status { get; set; }
    public string StatusDescricao { get; set; } = string.Empty;
}
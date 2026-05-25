namespace FabysUnha.Models.SqlViews;

public class ListaClientesView
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public int StatusId { get; set; }
    public string StatusDescricao { get; set; } = string.Empty;
}
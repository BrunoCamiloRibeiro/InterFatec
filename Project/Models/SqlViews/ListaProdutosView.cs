namespace FabysUnha.Models.SqlViews;

public class ListaProdutosView
{
    public int Codigo { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string PathImagem { get; set; } = string.Empty;
    public int StatusId { get; set; }
    public string StatusDescricao { get; set; } = string.Empty;
}
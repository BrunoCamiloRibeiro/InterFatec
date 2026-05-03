namespace FabysUnha.Models;

public class Marcas
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;

    // FK
    public ICollection<Produtos> Produtos { get; set; } = new List<Produtos>();

}
using System.ComponentModel.DataAnnotations;

namespace FabysUnha.Models;

public class Produtos
{
    [Key]
    public int Codigo { get; set; }
    public string Nome { get; set; } = string.Empty;

    // FK
    public int MarcaId { get; set; }
    public Marcas? Marca { get; set; }

    public decimal Preco { get; set; }
    public string PathImagem { get; set; } = string.Empty;

    public ICollection<Produtos_Agendados> Produtos_Agendados { get; set; } = new List<Produtos_Agendados>();
}
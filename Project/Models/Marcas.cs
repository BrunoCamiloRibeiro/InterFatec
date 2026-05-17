using FabysUnha.Enums;

namespace FabysUnha.Models;

public class Marcas
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public MarcaStatus Status { get; set; } = MarcaStatus.Ativo;

    // FK
    public ICollection<Produtos> Produtos { get; set; } = new List<Produtos>();

}
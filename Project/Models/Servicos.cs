namespace FabysUnha.Models; 

public class Servicos
{
    public int Id { get; set; }
    public decimal Preco { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public TimeSpan Tempo { get; set; }

    // FK
    public ICollection<Servicos_Agendados> Servicos_Agendados { get; set; } = new List<Servicos_Agendados>();
    public ICollection<Produtos_Agendados> Produtos_Agendados { get; set; } = new List<Produtos_Agendados>();
}
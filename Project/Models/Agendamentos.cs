namespace FabysUnha.Models;

public class Agendamentos
{
    public int Nr { get; set; }
    public DateTime Data { get; set; }
    public decimal Total { get; set; }

    // FK
    public int ClienteId { get; set; }
    public Clientes? Cliente { get; set; }

    // Não sei oq fazer com a tabela STATUS ainda

    // public int StatusId { get; set; }
    // public Status? Status { get; set; }
    public ICollection<Servicos_Agendados> Servicos_Agendados { get; set; } = new List<Servicos_Agendados>();
    public ICollection<Produtos_Agendados> Produtos_Agendados { get; set; } = new List<Produtos_Agendados>();
}
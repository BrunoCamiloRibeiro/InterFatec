using System.ComponentModel.DataAnnotations;
using FabysUnha.Enums;

namespace FabysUnha.Models;

public class Agendamentos
{
    [Key]
    public int Nr { get; set; }
    public DateTime Data { get; set; }
    public decimal Total { get; set; }
    public AgendamentoStatus Status { get; set; } = AgendamentoStatus.Pendente;

    // FK
    public int ClienteId { get; set; }
    public Clientes? Cliente { get; set; }
    public ICollection<Servicos_Agendados> Servicos_Agendados { get; set; } = new List<Servicos_Agendados>();
    public ICollection<Produtos_Agendados> Produtos_Agendados { get; set; } = new List<Produtos_Agendados>();
}
using FabysUnha.Enums;

namespace FabysUnha.Models; 

public class Servicos
{
    public int Id { get; set; }
    public decimal Preco { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public TimeSpan Tempo { get; set; }
    public ServicoStatus Status { get; set; } = ServicoStatus.Ativo;

    // FK
    public ICollection<Servicos_Agendados> Servicos_Agendados { get; set; } = new List<Servicos_Agendados>();
}
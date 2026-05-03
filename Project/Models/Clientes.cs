namespace FabysUnha.Models;

public class Clientes : Pessoas
{
    public ICollection<Agendamentos> Agendamentos { get; set; } = new List<Agendamentos>();
}
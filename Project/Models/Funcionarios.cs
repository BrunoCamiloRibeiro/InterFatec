namespace FabysUnha.Models;

public class Funcionarios : Pessoas
{
    public decimal Salario { get; set; }

    // FK
    public int EspecialidadeId { get; set; }
    public Especialidades? Especialidade { get; set; }
    public string Senha { get; set; } = String.Empty;

    public ICollection<Servicos_Agendados> Servicos_Agendados { get; set; } = new List<Servicos_Agendados>();
}
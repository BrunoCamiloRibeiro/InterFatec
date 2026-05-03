namespace FabysUnha.Models;

public class Especialidades
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;

    // FK
    public ICollection<Funcionarios> Funcionarios { get; set; } = new List<Funcionarios>();
}
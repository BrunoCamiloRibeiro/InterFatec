using FabysUnha.Enums;

namespace FabysUnha.Models;

public class Especialidades
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public EspecialidadeStatus Status { get; set; } = EspecialidadeStatus.Ativo;

    // FK
    public ICollection<Funcionarios> Funcionarios { get; set; } = new List<Funcionarios>();
}
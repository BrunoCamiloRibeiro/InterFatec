using FabysUnha.Enums;

namespace FabysUnha.Models;

public abstract class Pessoas
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public PessoaStatus Status { get; set; } = PessoaStatus.Ativo;
}

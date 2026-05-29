namespace FabysUnha.Models.SqlViews;

public class ListaFuncionariosView
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Telefone { get; set; }
    public string? Especialidade { get; set; }
    public decimal Salario { get; set; }
    public int StatusId { get; set; }
    public string? StatusDescricao { get; set; }
    public string? Senha { get; set; }
}
namespace FabysUnha.Models.SqlViews;

public class ListaFuncionariosView
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Especialidade { get; set; } = string.Empty;
    public decimal Salario { get; set; }
    public int StatusId { get; set; }
    public string StatusDescricao { get; set; } = string.Empty;
    public string Senha {get; set;} = string.Empty;
}
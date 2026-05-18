namespace FabysUnha.Models;

public class Servicos_Agendados
{
    public string Obs { get; set; } = string.Empty;
    public TimeSpan Horario { get; set; }
    public decimal Valor { get; set; }

    // FK
    public int AgendamentoNr { get; set; }
    public Agendamentos? Agendamento { get; set; }

    public int ServicoId { get; set; }
    public Servicos? Servico { get; set; }

    public int FuncionarioId { get; set; }
    public Funcionarios? Funcionario { get; set; }

    public ICollection<Produtos_Agendados> Produtos_Agendados { get; set; } = new List<Produtos_Agendados>();
}
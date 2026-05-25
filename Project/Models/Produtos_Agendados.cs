namespace FabysUnha.Models;

public class Produtos_Agendados
{
    public decimal Preco { get; set; }

    // FK
    public int AgendamentoNr { get; set; }
    public Agendamentos? Agendamento { get; set; }

    public int ServicoId { get; set; }
    public Servicos_Agendados? ServicoAgendado { get; set; }

    public int ProdutoCodigo { get; set; }
    public Produtos? Produto { get; set; }
}

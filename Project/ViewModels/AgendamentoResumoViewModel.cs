using System.ComponentModel.DataAnnotations;

namespace FabysUnha.ViewModels;

public class AgendamentoResumoViewModel
{
    public int Id { get; set; }
    public DateTime DataHora { get; set; }
    public string NomeServico { get; set; } = string.Empty; 
    public string Status { get; set; } = string.Empty;
}
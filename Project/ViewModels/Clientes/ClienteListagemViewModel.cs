using System.ComponentModel.DataAnnotations;

namespace FabysUnha.ViewModels;

public class ClienteListagemViewModel : PessoasViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "Total de Agendamentos")]
    public int TotalAgendamentos { get; set; }
    

    [Display(Name = "Último Agendamento")]
    public DateTime? DataUltimoAgendamento { get; set; }
}
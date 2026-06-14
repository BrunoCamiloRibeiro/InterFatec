using System.ComponentModel.DataAnnotations;

namespace FabysUnha.ViewModels;

/// <summary>
/// ViewModel utilizado para representar de forma simplificada um cliente em telas de listagem.
/// Herda de PessoasViewModel para aproveitar os dados básicos da pessoa (ex: Nome).
/// </summary>
public class ClienteListagemViewModel : PessoasViewModel
{
    /// <summary>
    /// Identificador único do cliente.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Quantidade total de agendamentos associados a este cliente.
    /// </summary>
    [Display(Name = "Total de Agendamentos")]
    public int TotalAgendamentos { get; set; }
    

    /// <summary>
    /// Data e hora do último agendamento realizado por este cliente.
    /// O valor pode ser nulo caso não haja agendamentos.
    /// </summary>
    [Display(Name = "Último Agendamento")]
    public DateTime? DataUltimoAgendamento { get; set; }
}
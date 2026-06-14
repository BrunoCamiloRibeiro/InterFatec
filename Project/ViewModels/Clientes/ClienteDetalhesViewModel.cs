using System.ComponentModel.DataAnnotations;

namespace FabysUnha.ViewModels;

/// <summary>
/// ViewModel utilizado para exibir os detalhes completos de um cliente.
/// Herda de PessoasViewModel para aproveitar propriedades comuns a pessoas (nome, email, etc.).
/// </summary>
public class ClienteDetalhesViewModel : PessoasViewModel
{
    /// <summary>
    /// Identificador único do cliente.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Quantidade total de agendamentos já realizados ou associados a este cliente.
    /// </summary>
    [Display(Name = "Total de Agendamentos")]
    public int TotalAgendamentos { get; set; }

    /// <summary>
    /// Data e hora do agendamento mais recente associado a este cliente.
    /// Pode ser nulo se o cliente ainda não possuir nenhum agendamento.
    /// </summary>
    [Display(Name = "Último Agendamento")]
    public DateTime? DataUltimoAgendamento { get; set; }

}
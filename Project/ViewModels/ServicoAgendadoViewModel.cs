using System.ComponentModel.DataAnnotations;

namespace FabysUnha.ViewModels;

public class ServicoAgendadoViewModel
{
	[Range(1, int.MaxValue, ErrorMessage = "Selecione um serviço válido.")]
	[Display(Name = "Serviço")]
	public int ServicoId { get; set; }

	[Range(1, int.MaxValue, ErrorMessage = "Selecione um funcionário válido.")]
	[Display(Name = "Funcionário")]
	public int FuncionarioId { get; set; }

	[Display(Name = "Horário")]
	[DataType(DataType.Time)]
	public TimeSpan Horario { get; set; }

	[Display(Name = "Observação")]
	[StringLength(200, ErrorMessage = "A observação não pode ultrapassar 200 caracteres.")]
	public string Obs { get; set; } = string.Empty;

	[Display(Name = "Valor")]
	[DataType(DataType.Currency)]
	public decimal Valor { get; set; }

	[Display(Name = "Serviço")]
	public string ServicoNome { get; set; } = string.Empty;

	[Display(Name = "Funcionário")]
	public string FuncionarioNome { get; set; } = string.Empty;
}
using System.ComponentModel.DataAnnotations;

namespace FabysUnha.ViewModels;

/// <summary>
/// Modelo de visualização (ViewModel) que representa os dados de um serviço agendado.
/// Utilizado para coletar e validar as informações de um item de serviço que faz parte de um agendamento.
/// </summary>
public class ServicoAgendadoViewModel
{
	/// <summary>
	/// Identificador único do serviço selecionado.
	/// </summary>
	// A anotação [Range] exige que um ID positivo seja enviado, evitando o envio de IDs nulos (0) padrão ou negativos.
	[Range(1, int.MaxValue, ErrorMessage = "Selecione um serviço válido.")]
	[Display(Name = "Serviço")]
	public int ServicoId { get; set; }

	/// <summary>
	/// Identificador único do funcionário que realizará o serviço.
	/// </summary>
	// Segue a mesma lógica do ServicoId, garantindo a seleção de um profissional válido.
	[Range(1, int.MaxValue, ErrorMessage = "Selecione um funcionário válido.")]
	[Display(Name = "Funcionário")]
	public int FuncionarioId { get; set; }

	/// <summary>
	/// Horário programado para o início do serviço.
	/// </summary>
	// Campo de preenchimento obrigatório para gerenciar as janelas de disponibilidade da agenda.
	[Required(ErrorMessage = "O horário é obrigatório.")]
	[Display(Name = "Horário")]
	public string Horario { get; set; } = string.Empty;

	/// <summary>
	/// Observações ou necessidades especiais solicitadas para a realização do serviço.
	/// </summary>
	// Como as observações são opcionais, são permitidas strings vazias ou nulas. O limite é 200 caracteres para não sobrecarregar o layout.
	[Display(Name = "Observação")]
	[StringLength(200, ErrorMessage = "A observação não pode ultrapassar 200 caracteres.")]
	public string? Obs { get; set; } = string.Empty;

	/// <summary>
	/// Valor financeiro do serviço no momento em que ele foi agendado.
	/// </summary>
	// A anotação DataType.Currency informa à interface que este número deve ser exibido como valor monetário (ex: R$ 0,00).
	[Display(Name = "Valor")]
	[DataType(DataType.Currency)]
	public decimal Valor { get; set; }

	/// <summary>
	/// Nome do serviço. Utilizado primariamente para apresentação (somente leitura) ao usuário na tela.
	/// </summary>
	[Display(Name = "Serviço")]
	public string ServicoNome { get; set; } = string.Empty;

	/// <summary>
	/// Nome do funcionário. Utilizado primariamente para exibir na interface quem fará o atendimento.
	/// </summary>
	[Display(Name = "Funcionário")]
	public string FuncionarioNome { get; set; } = string.Empty;
}
using System.ComponentModel.DataAnnotations;

namespace FabysUnha.ViewModels;

public class ServicoClienteItemViewModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Selecione um serviço válido.")]
    [Display(Name = "Serviço")]
    public int ServicoId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Selecione um funcionário válido.")]
    [Display(Name = "Funcionário")]
    public int FuncionarioId { get; set; }

    [Required(ErrorMessage = "O horário é obrigatório.")]
    [Display(Name = "Horário")]
    public string Horario { get; set; } = string.Empty;

    [Display(Name = "Observação")]
    [StringLength(200, ErrorMessage = "A observação não pode ultrapassar 200 caracteres.")]
    public string? Obs { get; set; } = string.Empty;

    [Display(Name = "Produtos")]
    public List<int?> ProdutosCodigos { get; set; } = new();
}

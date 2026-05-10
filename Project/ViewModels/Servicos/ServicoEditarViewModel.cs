using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace FabysUnha.ViewModels.Servicos;

public class ServicoEditarViewModel
{
    [HiddenInput]
    public int Id { get; set; }

    [Required(ErrorMessage = "O campo Descrição é obrigatório.")]
    [MinLength(3, ErrorMessage = "A descrição deve ter pelo menos 3 caracteres.")]
    [MaxLength(100, ErrorMessage = "A descrição não pode passar de 100 caracteres.")]
    [Display(Name = "Descrição do Serviço")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo Preço é obrigatório.")]
    [DataType(DataType.Currency)]
    [Display(Name = "Preço")]
    public decimal Preco { get; set; }

    [Required(ErrorMessage = "O Tempo estimado é obrigatório.")]
    [DataType(DataType.Time)] 
    [Display(Name = "Tempo Estimado (HH:mm)")]
    public TimeSpan Tempo { get; set; }
}
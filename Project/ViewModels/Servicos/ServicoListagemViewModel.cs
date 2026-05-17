using System.ComponentModel.DataAnnotations;
using FabysUnha.Enums;

namespace FabysUnha.ViewModels.Servicos;

public class ServicoListagemViewModel
{
    public int Id { get; set; }

    [Display(Name = "Descrição")]
    public string Descricao { get; set; } = string.Empty;

    [Display(Name = "Preço")]
    public string PrecoFormatado { get; set; } = string.Empty;

    [Display(Name = "Duração")]
    public string TempoFormatado { get; set; } = string.Empty;

    [Display(Name = "Status")]
    public ServicoStatus Status { get; set; }
}
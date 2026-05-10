using System.ComponentModel.DataAnnotations;

namespace FabysUnha.ViewModels.Servicos;

public class ServicoDetalhesViewModel
{
    public int Id { get; set; }

    [Display(Name = "Descrição do Serviço")]
    public string Descricao { get; set; } = string.Empty;

    [Display(Name = "Valor Cobrado")]
    public string PrecoFormatado { get; set; } = string.Empty;

    [Display(Name = "Tempo Estimado")]
    public string TempoFormatado { get; set; } = string.Empty;
}
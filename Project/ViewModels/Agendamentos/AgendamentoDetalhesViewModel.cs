using System.ComponentModel.DataAnnotations;
using FabysUnha.Enums;

namespace FabysUnha.ViewModels;

/// <summary>
/// Modelo de visualização (ViewModel) focado na exibição dos detalhes completos de um agendamento.
/// Projetado especificamente para a tela de 'Visualizar Detalhes', servindo para a exibição (somente leitura) de todas as informações.
/// </summary>
public class AgendamentoDetalhesViewModel
{
    /// <summary>
    /// Número de identificação primária (ID) do agendamento.
    /// </summary>
    public int Nr { get; set; }

    /// <summary>
    /// Nome completo do cliente que reservou o horário.
    /// </summary>
    [Display(Name = "Cliente")]
    public string ClienteNome { get; set; } = string.Empty;

    /// <summary>
    /// Data e hora do compromisso marcado.
    /// </summary>
    [Display(Name = "Data e Hora")]
    public DateTime DataHora { get; set; }

    /// <summary>
    /// Estado ou fase atual do agendamento (por exemplo: Pendente, Realizado, Cancelado).
    /// </summary>
    [Display(Name = "Status")]
    public AgendamentoStatus Status { get; set; }

    /// <summary>
    /// Somatório financeiro correspondente a todos os serviços contratados e produtos consumidos.
    /// </summary>
    // A instrução de DataType.Currency formata o campo de acordo com o padrão monetário do sistema operacional do servidor/usuário (ex: R$).
    [Display(Name = "Total")]
    [DataType(DataType.Currency)]
    public decimal Total { get; set; }

    /// <summary>
    /// Relação dos serviços previstos no agendamento, especificando quem executará e qual o custo de cada um.
    /// </summary>
    // Instanciado de fábrica para impedir o clássico erro de 'NullReferenceException' ao iterar na tela caso a lista esteja vazia.
    public List<ServicoAgendadoViewModel> ServicosAgendados { get; set; } = new();

    /// <summary>
    /// Relação dos produtos vendidos ou consumidos associados a este mesmo número de agendamento.
    /// </summary>
    // Segue a mesma boa prática de inicialização de coleções vazias na própria definição da propriedade.
    public List<ProdutoAgendadoViewModel> ProdutosAgendados { get; set; } = new();
}
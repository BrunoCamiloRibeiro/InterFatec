using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FabysUnha.ViewModels;

/// <summary>
/// Modelo de visualização (ViewModel) desenhado para a tela onde o próprio cliente realiza o seu agendamento.
/// Agrupa e valida os dados de identificação, a data do compromisso e as listas de suporte para a interface gráfica.
/// </summary>
public class AgendamentoClienteViewModel
{
    // ==========================================
    // Identificação do cliente
    // ==========================================

    /// <summary>
    /// Nome completo do cliente.
    /// </summary>
    // Validação restritiva para obrigar o preenchimento e evitar o envio de textos demasiadamente longos ao banco de dados.
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
    [Display(Name = "Nome")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Número de telefone para contato, frequentemente usado como principal canal de comunicação e identificação do cliente.
    /// </summary>
    // O comprimento máximo é 11 (ex: DDD + 9 dígitos). Assegura um formato enxuto.
    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [StringLength(11, ErrorMessage = "O telefone não pode exceder 11 caracteres.")]
    [Display(Name = "Telefone")]
    public string Telefone { get; set; } = string.Empty;

    /// <summary>
    /// Senha do cliente, caso o formulário englobe autenticação ou cadastro expresso durante o agendamento.
    /// </summary>
    // DataType.Password garante que a entrada de dados não exiba a senha em texto claro na tela do usuário.
    [Display(Name = "Senha")]
    [StringLength(50)]
    [DataType(DataType.Password)]
    public string? Senha { get; set; }

    // ==========================================
    // Data do agendamento
    // ==========================================

    /// <summary>
    /// Data em que o cliente deseja realizar os serviços.
    /// </summary>
    // DataType.Date informa aos renderizadores web para usarem o calendário nativo (input type="date") do navegador.
    [Required(ErrorMessage = "A data do agendamento é obrigatória.")]
    [Display(Name = "Data")]
    [DataType(DataType.Date)]
    public DateTime Data { get; set; }

    // ==========================================
    // Serviços selecionados (containers)
    // ==========================================

    /// <summary>
    /// Uma coleção dos serviços que o cliente quer agendar.
    /// </summary>
    // Instanciada como lista vazia, protegendo o código de erros de referência nula (NullReferenceException) caso nenhum serviço seja preenchido de imediato.
    [Display(Name = "Serviços")]
    public List<ServicoClienteItemViewModel> Servicos { get; set; } = new();

    // ==========================================
    // Listas para popular os selects na view
    // ==========================================

    /// <summary>
    /// Opções carregadas do banco de dados para popular a lista suspensa (dropdown/select) de Serviços.
    /// </summary>
    public IEnumerable<SelectListItem>? ServicosList { get; set; }

    /// <summary>
    /// Opções de profissionais para popular a lista suspensa de Funcionários disponíveis.
    /// </summary>
    public IEnumerable<SelectListItem>? FuncionariosList { get; set; }

    /// <summary>
    /// Opções de produtos para permitir ao cliente incluir alguma compra no momento do agendamento.
    /// </summary>
    public IEnumerable<SelectListItem>? ProdutosList { get; set; }
}

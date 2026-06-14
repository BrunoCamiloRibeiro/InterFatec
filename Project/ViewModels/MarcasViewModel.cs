using System.ComponentModel.DataAnnotations;
using FabysUnha.Enums;

namespace FabysUnha.ViewModels;

/// <summary>
/// Modelo de visualização (ViewModel) utilizado para o transporte e validação dos dados de uma Marca.
/// Gerencia as informações que serão exibidas e capturadas nas telas de cadastro e edição de marcas de produtos.
/// </summary>
public class MarcasViewModel
{
    /// <summary>
    /// Identificador único da marca.
    /// </summary>
    // É utilizado internamente pelo sistema como chave para localizar a marca nas operações de atualização ou exclusão.
    public int Id { get; set; }

    /// <summary>
    /// Nome descritivo da marca de produto.
    /// </summary>
    // A anotação [Required] impede que o formulário seja submetido se o campo nome estiver vazio.
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Indica a situação atual da marca no sistema (ex: Ativa ou Inativa).
    /// </summary>
    // Por padrão, uma nova marca é inicializada com o status 'Ativo', indicando que já pode ser utilizada em produtos.
    [Display(Name = "Status")]
    public MarcaStatus Status { get; set; } = MarcaStatus.Ativo;
}
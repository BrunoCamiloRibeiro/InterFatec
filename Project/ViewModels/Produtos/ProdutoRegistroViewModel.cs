using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using FabysUnha.Enums;
using Microsoft.AspNetCore.Http;

namespace FabysUnha.ViewModels;

/// <summary>
/// ViewModel utilizado para o cadastro de um novo produto no sistema.
/// </summary>
public class ProdutoRegistroViewModel
{
    /// <summary>
    /// Obtém ou define o nome do produto.
    /// </summary>
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [MinLength(2, ErrorMessage = "O campo Nome deve conter pelo menos 2 caracteres.")]
    [MaxLength(100, ErrorMessage = "O campo Nome deve conter no máximo 100 caracteres.")]
    [Display(Name = "Nome do Produto")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o identificador da marca selecionada.
    /// </summary>
    [Required(ErrorMessage = "Selecione uma marca.")]
    [Display(Name = "Marca")]
    public int MarcaId { get; set; }

    /// <summary>
    /// Obtém ou define o preço de venda do produto.
    /// </summary>
    [Required(ErrorMessage = "O campo Preço é obrigatório.")]
    [DataType(DataType.Currency)]
    [Display(Name = "Preço de Venda")]
    public decimal Preco { get; set; }

    /// <summary>
    /// Obtém ou define o caminho da imagem caso a imagem venha via URL.
    /// </summary>
    [Display(Name = "Caminho ou URL da Imagem")]
    public string? PathImagem { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o arquivo de imagem caso seja feito upload pelo formulário.
    /// </summary>
    [Display(Name = "Upload de Imagem (Opcional)")]
    public IFormFile? ImagemUpload { get; set; }

    /// <summary>
    /// Obtém ou define o status do produto, com o valor padrão sendo "Ativo".
    /// </summary>
    [Display(Name = "Status")]
    public ProdutoStatus Status { get; set; } = ProdutoStatus.Ativo;

    /// <summary>
    /// Obtém ou define a lista de marcas disponíveis para seleção no formulário de registro.
    /// </summary>
    // Carregada no Controller para criar as opções de marca na View
    public IEnumerable<SelectListItem>? MarcasList { get; set; }
}
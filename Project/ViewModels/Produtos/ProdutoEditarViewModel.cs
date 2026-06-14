using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using FabysUnha.Enums;

namespace FabysUnha.ViewModels;

/// <summary>
/// ViewModel utilizado para gerenciar a edição de um produto existente.
/// Inclui validações para garantir a consistência dos dados inseridos.
/// </summary>
public class ProdutoEditarViewModel
{
    /// <summary>
    /// Obtém ou define o código do produto. 
    /// Ocultado na interface usando o atributo [HiddenInput].
    /// </summary>
    [HiddenInput]
    public int Codigo { get; set; }

    /// <summary>
    /// Obtém ou define o nome do produto.
    /// </summary>
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [MinLength(2, ErrorMessage = "O campo Nome deve conter pelo menos 2 caracteres.")]
    [MaxLength(100, ErrorMessage = "O campo Nome deve conter no máximo 100 caracteres.")]
    [Display(Name = "Nome do Produto")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o identificador da marca associada ao produto.
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
    /// Obtém ou define o caminho ou a URL da imagem atual do produto.
    /// </summary>
    [Display(Name = "Caminho ou URL da Imagem")]
    public string? PathImagem { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o arquivo de imagem para upload (opcional, para substituir a imagem existente).
    /// </summary>
    [Display(Name = "Upload de Nova Imagem (Substituir)")]
    public IFormFile? ImagemUpload { get; set; }

    /// <summary>
    /// Obtém ou define o status do produto. Padrão inicial é Ativo.
    /// </summary>
    [Display(Name = "Status")]
    public ProdutoStatus Status { get; set; } = ProdutoStatus.Ativo;

    /// <summary>
    /// Propriedade auxiliar para montar o <select> no HTML.
    /// </summary>
    // Inicializada pelo controlador com os dados do banco
    public IEnumerable<SelectListItem>? MarcasList { get; set; }
}
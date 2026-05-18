using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FabysUnha.Enums;

namespace FabysUnha.ViewModels;

public class ProdutoEditarViewModel
{
    [HiddenInput]
    public int Codigo { get; set; }

    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [MinLength(2, ErrorMessage = "O campo Nome deve conter pelo menos 2 caracteres.")]
    [MaxLength(100, ErrorMessage = "O campo Nome deve conter no máximo 100 caracteres.")]
    [Display(Name = "Nome do Produto")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Selecione uma marca.")]
    [Display(Name = "Marca")]
    public int MarcaId { get; set; }

    [Required(ErrorMessage = "O campo Preço é obrigatório.")]
    [DataType(DataType.Currency)]
    [Display(Name = "Preço de Venda")]
    public decimal Preco { get; set; }

    [Display(Name = "Caminho ou URL da Imagem")]
    public string PathImagem { get; set; } = string.Empty;

    [Display(Name = "Status")]
    public ProdutoStatus Status { get; set; } = ProdutoStatus.Ativo;

    // Propriedade auxiliar para montar o <select> no HTML.
    public IEnumerable<SelectListItem>? MarcasList { get; set; }
}
using System.ComponentModel.DataAnnotations;
using FabysUnha.Enums;

namespace FabysUnha.ViewModels;

/// <summary>
/// Modelo de visualização base (ViewModel abstrata) para entidades que representam pessoas no sistema.
/// Serve como estrutura inicial para outras classes (como Clientes ou Funcionários), centralizando regras de validação comuns.
/// </summary>
public abstract class PessoasViewModel
{
    /// <summary>
    /// Nome completo da pessoa.
    /// </summary>
    // A anotação [Required] garante o preenchimento, enquanto [StringLength] limita o tamanho para não estourar o limite do banco de dados.
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Número de telefone para contato.
    /// </summary>
    // Além de obrigatório, o atributo [Phone] aplica uma validação de formato para evitar a inserção de textos que não sejam números de telefone.
    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [Phone(ErrorMessage = "Telefone em formato inválido.")]
    public string Telefone { get; set; } = string.Empty;

    /// <summary>
    /// Status ou situação cadastral da pessoa.
    /// </summary>
    // Mantém a padronização definindo novos cadastros automaticamente com o estado 'Ativo'.
    [Display(Name = "Status")]
    public PessoaStatus Status { get; set; } = PessoaStatus.Ativo;
}
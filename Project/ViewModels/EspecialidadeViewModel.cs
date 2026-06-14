using System.ComponentModel.DataAnnotations;
using FabysUnha.Enums;

namespace FabysUnha.ViewModels;

/// <summary>
/// Modelo de visualização (ViewModel) responsável por transferir e validar os dados de uma Especialidade.
/// Utilizado nas telas de criação, edição e exibição de especialidades dos funcionários.
/// </summary>
public class EspecialidadeViewModel
{
    /// <summary>
    /// Identificador único da especialidade.
    /// </summary>
    // Utilizado internamente para identificar o registro no banco de dados nas operações de atualização ou exclusão.
    public int Id { get; set; }

    /// <summary>
    /// Descrição ou nome da especialidade.
    /// </summary>
    // As anotações de validação garantem que o usuário informe o nome obrigatoriamente e que o tamanho esteja entre 3 e 25 caracteres.
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [MinLength(3, ErrorMessage = "O campo Nome deve conter pelo menos 3 caracteres.")]
    [MaxLength(25, ErrorMessage = "O campo Nome deve conter no máximo 25 caracteres.")]
    [Display(Name = "Nome da Especialidade")]
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Situação atual da especialidade.
    /// </summary>
    // Define, por padrão, o status inicial como 'Ativo' na criação de uma nova especialidade.
    [Display(Name = "Status")]
    public EspecialidadeStatus Status { get; set; } = EspecialidadeStatus.Ativo;
}
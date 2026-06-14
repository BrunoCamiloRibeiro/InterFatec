using FabysUnha.Enums;

namespace FabysUnha.Models;

/// <summary>
/// Representa as especialidades de atuação dos funcionários no sistema (ex: Manicure, Cabeleireiro).
/// É usada para categorizar o escopo de serviço que um determinado funcionário pode prestar.
/// </summary>
public class Especialidades
{
    /// <summary>
    /// Identificador único da especialidade (Chave Primária).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Texto descritivo contendo o nome ou título da especialidade.
    /// </summary>
    // Inicializado como string vazia (string.Empty) em vez de null, seguindo boas práticas para evitar exceções em tempo de execução
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Status da especialidade, para fins de exclusão lógica (Ativa ou Inativa).
    /// </summary>
    // O valor padrão no momento da criação é 'Ativo'
    public EspecialidadeStatus Status { get; set; } = EspecialidadeStatus.Ativo;

    /// <summary>
    /// Relação de funcionários que estão vinculados a esta especialidade.
    /// Representa o lado "muitos" no relacionamento um-para-muitos com a classe Funcionarios.
    /// </summary>
    // FK (Chave Estrangeira virtual/propriedade de navegação)
    // Uma lista vazia é instanciada para que seja seguro adicionar itens sem precisar verificar se a lista é nula
    public ICollection<Funcionarios> Funcionarios { get; set; } = new List<Funcionarios>();
}
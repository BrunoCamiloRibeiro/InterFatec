using FabysUnha.Enums;

namespace FabysUnha.Models; 

/// <summary>
/// Classe que representa um serviço oferecido pelo salão (Ex: Manicure, Pedicure).
/// </summary>
public class Servicos
{
    /// <summary>
    /// Obtém ou define o identificador único do serviço.
    /// </summary>
    // Por padrão, propriedades com o nome 'Id' são tratadas como Chave Primária pelo Entity Framework.
    public int Id { get; set; }

    /// <summary>
    /// Obtém ou define o preço base cobrado pelo serviço.
    /// </summary>
    // Utilizamos 'decimal' para armazenar valores monetários de forma segura e sem perda de precisão.
    public decimal Preco { get; set; }

    /// <summary>
    /// Obtém ou define a descrição detalhada do serviço.
    /// </summary>
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o tempo estimado de duração do serviço.
    /// </summary>
    // 'TimeSpan' é o tipo ideal no C# para representar intervalos de tempo, como a duração de uma atividade.
    public TimeSpan Tempo { get; set; }

    /// <summary>
    /// Obtém ou define o status atual do serviço.
    /// </summary>
    // Inicializado como 'ServicoStatus.Ativo' por padrão, para que novos serviços estejam disponíveis imediatamente.
    public ServicoStatus Status { get; set; } = ServicoStatus.Ativo;

    // // Propriedades de Navegação (Relacionamentos)

    /// <summary>
    /// Coleção de instâncias onde este serviço foi agendado.
    /// </summary>
    // Uma relação de um-para-muitos (Um Serviço pode estar em vários Agendamentos).
    // Inicializar com uma nova Lista evita a exceção 'NullReferenceException' ao manipular a coleção.
    public ICollection<Servicos_Agendados> Servicos_Agendados { get; set; } = new List<Servicos_Agendados>();
}
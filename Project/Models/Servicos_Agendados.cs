namespace FabysUnha.Models;

/// <summary>
/// Classe associativa que detalha os serviços escolhidos dentro de um agendamento específico.
/// </summary>
public class Servicos_Agendados
{
    /// <summary>
    /// Obtém ou define observações adicionais sobre a execução deste serviço.
    /// </summary>
    // Exemplo de uso: "A cliente prefere que o esmalte seja hipoalergênico".
    public string Obs { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o horário exato em que o serviço será iniciado dentro do agendamento.
    /// </summary>
    // Diferente de DateTime (que guarda data e hora), o TimeSpan guarda apenas a parte do tempo.
    public TimeSpan Horario { get; set; }

    /// <summary>
    /// Obtém ou define o valor cobrado por este serviço específico neste agendamento.
    /// </summary>
    // Guardar o valor aqui mantém o histórico intacto, mesmo se o valor do serviço base (na classe Servicos) mudar no futuro.
    public decimal Valor { get; set; }

    // // Relacionamentos - Chaves Estrangeiras (FK) e Propriedades de Navegação

    /// <summary>
    /// Obtém ou define o número do agendamento principal ao qual este serviço pertence.
    /// </summary>
    public int AgendamentoNr { get; set; }

    /// <summary>
    /// Propriedade de navegação para acessar o Agendamento principal de forma orientada a objetos.
    /// </summary>
    public Agendamentos? Agendamento { get; set; }

    /// <summary>
    /// Obtém ou define o ID do serviço base (catálogo de serviços) que será executado.
    /// </summary>
    public int ServicoId { get; set; }

    /// <summary>
    /// Propriedade de navegação para recuperar os detalhes gerais do Serviço base.
    /// </summary>
    public Servicos? Servico { get; set; }

    /// <summary>
    /// Obtém ou define o ID do funcionário responsável por realizar este serviço.
    /// </summary>
    public int FuncionarioId { get; set; }

    /// <summary>
    /// Propriedade de navegação para acessar os dados do Funcionário que foi escalado.
    /// </summary>
    public Funcionarios? Funcionario { get; set; }

    /// <summary>
    /// Obtém ou define a lista de produtos que serão utilizados durante a execução deste serviço.
    /// </summary>
    // Relacionamento um-para-muitos: Um serviço agendado pode utilizar vários produtos (Ex: Base, Esmalte, Algodão).
    public ICollection<Produtos_Agendados> Produtos_Agendados { get; set; } = new List<Produtos_Agendados>();
}
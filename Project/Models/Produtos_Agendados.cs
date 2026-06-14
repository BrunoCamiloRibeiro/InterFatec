namespace FabysUnha.Models;

/// <summary>
/// Classe associativa que representa a relação entre os produtos e os serviços agendados.
/// Indica quais produtos foram utilizados em um determinado serviço de um agendamento.
/// Essa classe costuma ser chamada de tabela de junção (Join Table) em bancos relacionais.
/// </summary>
public class Produtos_Agendados
{
    /// <summary>
    /// Obtém ou define o preço do produto no momento em que foi agendado.
    /// </summary>
    // Armazenar o preço aqui garante que mudanças futuras no preço do produto não alterem o histórico deste agendamento.
    public decimal Preco { get; set; }

    // // Relacionamento com Agendamentos

    /// <summary>
    /// Chave Estrangeira: Obtém ou define o número identificador do agendamento vinculado.
    /// </summary>
    public int AgendamentoNr { get; set; }

    /// <summary>
    /// Propriedade de navegação para o objeto Agendamento.
    /// </summary>
    public Agendamentos? Agendamento { get; set; }

    // // Relacionamento com Servicos_Agendados

    /// <summary>
    /// Chave Estrangeira: Obtém ou define o ID do serviço agendado onde o produto foi consumido.
    /// </summary>
    public int ServicoId { get; set; }

    /// <summary>
    /// Propriedade de navegação para o objeto Serviço Agendado específico.
    /// </summary>
    public Servicos_Agendados? ServicoAgendado { get; set; }

    // // Relacionamento com Produtos

    /// <summary>
    /// Chave Estrangeira: Obtém ou define o código do produto que está sendo utilizado.
    /// </summary>
    public int ProdutoCodigo { get; set; }

    /// <summary>
    /// Propriedade de navegação para acessar as informações completas do Produto vinculado.
    /// </summary>
    public Produtos? Produto { get; set; }
}

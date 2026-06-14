namespace FabysUnha.Models;

/// <summary>
/// Classe que representa um Cliente do salão.
/// Herda de 'Pessoas', o que significa que possui os atributos básicos de uma pessoa (como Nome e CPF),
/// além de ter dados e comportamentos específicos do domínio do cliente.
/// </summary>
public class Clientes : Pessoas
{
    /// <summary>
    /// Coleção dos agendamentos realizados por este cliente.
    /// Configura um relacionamento de um-para-muitos (um cliente possui vários agendamentos).
    /// </summary>
    // Instanciamos uma lista vazia (new List) para evitar exceções de referência nula
    // ao iterar ou adicionar novos itens em instâncias recém-criadas
    public ICollection<Agendamentos> Agendamentos { get; set; } = new List<Agendamentos>();
}
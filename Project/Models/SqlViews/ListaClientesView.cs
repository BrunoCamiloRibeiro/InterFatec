namespace FabysUnha.Models.SqlViews;

/// <summary>
/// Classe de modelo que mapeia a view do banco de dados contendo a lista de clientes.
/// Configurada como uma entidade sem chave (Keyless Entity Type) no Entity Framework Core.
/// </summary>
public class ListaClientesView
{
    /// <summary>
    /// Obtém ou define o identificador único do cliente.
    /// </summary>
    // ID gerado pelo banco de dados para identificar o cliente.
    public int Id { get; set; }

    /// <summary>
    /// Obtém ou define o nome completo do cliente.
    /// </summary>
    // Garante que a propriedade não seja nula através da inicialização com string.Empty.
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o número de telefone para contato do cliente.
    /// </summary>
    // Armazena o telefone formatado ou apenas os dígitos numéricos.
    public string Telefone { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o identificador numérico do status do cliente (ex: Ativo, Inativo).
    /// </summary>
    // Código de referência para a tabela ou lógica de domínio de status.
    public int StatusId { get; set; }

    /// <summary>
    /// Obtém ou define a descrição em texto do status atual do cliente.
    /// </summary>
    // Facilita a exibição do status na interface de usuário.
    public string StatusDescricao { get; set; } = string.Empty;
}
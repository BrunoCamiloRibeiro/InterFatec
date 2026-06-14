namespace FabysUnha.Models.SqlViews;

/// <summary>
/// Modelo de visualização que representa os dados retornados pela view de lista de funcionários no SQL.
/// Projetada para leitura via EF Core usando a abordagem de Keyless Entity.
/// </summary>
public class ListaFuncionariosView
{
    /// <summary>
    /// Obtém ou define o código identificador do funcionário.
    /// </summary>
    // Usado para rastrear o funcionário dentro do sistema.
    public int Id { get; set; }

    /// <summary>
    /// Obtém ou define o nome do funcionário.
    /// </summary>
    // Propriedade anulável (nullable) para acomodar possíveis retornos nulos do banco.
    public string? Nome { get; set; }

    /// <summary>
    /// Obtém ou define o número de telefone do funcionário.
    /// </summary>
    // Representa o contato direto com o funcionário.
    public string? Telefone { get; set; }

    /// <summary>
    /// Obtém ou define a especialidade principal vinculada a este funcionário.
    /// </summary>
    // Exibe a área de atuação do funcionário em formato de texto.
    public string? Especialidade { get; set; }

    /// <summary>
    /// Obtém ou define o valor do salário atual do funcionário.
    /// </summary>
    // Mapeia para um tipo decimal adequado para valores financeiros.
    public decimal Salario { get; set; }

    /// <summary>
    /// Obtém ou define o identificador do status de vínculo do funcionário (ex: Ativo, Desligado).
    /// </summary>
    // Chave estrangeira conceitual para o domínio de status.
    public int StatusId { get; set; }

    /// <summary>
    /// Obtém ou define a representação textual do status do funcionário.
    /// </summary>
    // Texto já resolvido pela view para exibição em telas e relatórios.
    public string? StatusDescricao { get; set; }

    /// <summary>
    /// Obtém ou define a senha do funcionário.
    /// </summary>
    // Geralmente armazenada como hash no banco, mas mapeada como string na view.
    public string? Senha { get; set; }
}
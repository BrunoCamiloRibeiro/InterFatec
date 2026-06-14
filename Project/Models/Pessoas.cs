using FabysUnha.Enums;

namespace FabysUnha.Models;

/// <summary>
/// Classe abstrata que representa uma pessoa genérica no sistema (Pessoa Física ou Jurídica).
/// Serve como base para outras entidades como Clientes e Funcionários.
/// </summary>
public abstract class Pessoas
{
    /// <summary>
    /// Obtém ou define o identificador único da pessoa.
    /// </summary>
    // A propriedade 'Id' é geralmente mapeada como a chave primária (Primary Key) no banco de dados.
    public int Id { get; set; }

    /// <summary>
    /// Obtém ou define o nome completo da pessoa.
    /// </summary>
    // Inicializamos com 'string.Empty' para evitar referências nulas (null reference) caso o nome não seja preenchido.
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o telefone de contato da pessoa.
    /// </summary>
    public string Telefone { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o status atual da pessoa (Ex: Ativo, Inativo).
    /// </summary>
    // Utiliza um Enum 'PessoaStatus' e define 'Ativo' como o valor padrão no momento da criação do objeto.
    public PessoaStatus Status { get; set; } = PessoaStatus.Ativo;

    /// <summary>
    /// Obtém ou define a senha de acesso da pessoa ao sistema.
    /// </summary>
    public string Senha { get; set; } = string.Empty;
}

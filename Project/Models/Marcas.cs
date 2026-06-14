using FabysUnha.Enums;

namespace FabysUnha.Models;

/// <summary>
/// Representa as marcas comerciais dos produtos cadastrados no sistema.
/// </summary>
public class Marcas
{
    /// <summary>
    /// Chave primária que identifica unicamente a marca no banco de dados.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nome descritivo comercial da marca (ex: Risqué, Colorama).
    /// </summary>
    // Por segurança e boas práticas de codificação C#, iniciamos a string vazia ao invés de deixar nulo (null)
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Indicador se a marca está ativa ou inativa no sistema.
    /// Utilizado geralmente para esconder marcas que não são mais usadas sem precisar excluir o registro do banco.
    /// </summary>
    // Por padrão, toda nova marca instanciada começa com status ativo
    public MarcaStatus Status { get; set; } = MarcaStatus.Ativo;

    /// <summary>
    /// Relacionamento informando todos os produtos que são pertencentes a esta marca.
    /// </summary>
    // FK (propriedade de navegação configurando um para muitos)
    // Inicializar coleções com List vazias evita NullReferenceException quando há tentativas de iterar (ex: foreach) logo após a criação da instância
    public ICollection<Produtos> Produtos { get; set; } = new List<Produtos>();

}
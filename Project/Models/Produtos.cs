using System.ComponentModel.DataAnnotations;
using FabysUnha.Enums;

namespace FabysUnha.Models;

/// <summary>
/// Classe que representa um produto disponível no sistema, como esmaltes, cremes, etc.
/// </summary>
public class Produtos
{
    /// <summary>
    /// Obtém ou define o código identificador único do produto.
    /// </summary>
    // A anotação [Key] indica explicitamente ao Entity Framework que esta propriedade é a chave primária da tabela.
    [Key]
    public int Codigo { get; set; }

    /// <summary>
    /// Obtém ou define o nome do produto.
    /// </summary>
    // string.Empty previne avisos de nulabilidade do compilador e inicializa a propriedade com uma string vazia.
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o status do produto (Ex: Ativo, Inativo).
    /// </summary>
    public ProdutoStatus Status { get; set; } = ProdutoStatus.Ativo;

    // // A seção abaixo define o relacionamento com a entidade Marcas (Chave Estrangeira - FK)

    /// <summary>
    /// Obtém ou define o ID da marca associada ao produto.
    /// Atua como chave estrangeira no banco de dados.
    /// </summary>
    public int MarcaId { get; set; }

    /// <summary>
    /// Propriedade de navegação. 
    /// Permite acessar o objeto Marca relacionado a este produto diretamente.
    /// </summary>
    // O operador '?' indica que a Marca pode ser nula, caso não haja marca vinculada.
    public Marcas? Marca { get; set; }

    /// <summary>
    /// Obtém ou define o preço de venda ou custo do produto.
    /// </summary>
    // O tipo 'decimal' é altamente recomendado para valores monetários devido a sua precisão.
    public decimal Preco { get; set; }

    /// <summary>
    /// Obtém ou define o caminho (URL ou diretório local) da imagem do produto.
    /// </summary>
    public string PathImagem { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define a coleção de agendamentos nos quais este produto foi utilizado.
    /// Representa um relacionamento de um-para-muitos.
    /// </summary>
    // Inicializamos a lista com 'new List<Produtos_Agendados>()' para garantir que a coleção não seja nula
    // ao tentarmos adicionar novos itens a ela.
    public ICollection<Produtos_Agendados> Produtos_Agendados { get; set; } = new List<Produtos_Agendados>();
}
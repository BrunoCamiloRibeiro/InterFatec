using Microsoft.EntityFrameworkCore;
using FabysUnha.Models;
using FabysUnha.Data;
using FabysUnha.Enums;
using FabysUnha.Models.SqlViews;

namespace FabysUnha.Repositories;

/// <summary>
/// Repositório responsável pelas operações de acesso a dados da entidade Produtos.
/// Implementa a interface <see cref="IProdutosRepository"/> fornecendo a lógica de persistência e leitura.
/// </summary>
public class ProdutoRepository : IProdutosRepository
{
    /// <summary>
    /// Contexto do banco de dados utilizado para interagir com o Entity Framework Core.
    /// </summary>
    private readonly AppDbContext _context;

    /// <summary>
    /// Construtor do repositório de produtos.
    /// </summary>
    /// <param name="context">Instância do <see cref="AppDbContext"/> injetada via injeção de dependência.</param>
    public ProdutoRepository(AppDbContext context)
    {
        // Atribui o contexto injetado à variável privada para ser utilizado nos métodos do repositório
        _context = context;
    }

    /// <summary>
    /// Obtém todos os produtos cadastrados no sistema utilizando uma view otimizada do banco de dados.
    /// </summary>
    /// <returns>Uma lista assíncrona contendo todos os <see cref="Produtos"/>.</returns>
    public async Task<IEnumerable<Produtos>> ObterTodosProdutos()
    {
        // Consulta a view ListaProdutosView mapeada no banco de dados
        // Utiliza o AsNoTracking para melhorar a performance, pois não precisamos rastrear alterações nessas entidades
        // Ordena os resultados pelo nome do produto em ordem alfabética
        var produtosView = await _context.Set<ListaProdutosView>()
            .AsNoTracking()
            .OrderBy(produto => produto.Nome)
            .ToListAsync();

        // Mapeia os dados retornados da view para a entidade principal Produtos
        return produtosView.Select(produto => new Produtos
        {
            Codigo = produto.Codigo,
            Nome = produto.Nome,
            Marca = new Marcas
            {
                Nome = produto.Marca
            },
            Preco = produto.Preco,
            PathImagem = produto.PathImagem,
            Status = (ProdutoStatus)produto.StatusId
        }).ToList();
    }

    /// <summary>
    /// Obtém um produto específico com base no seu código identificador.
    /// </summary>
    /// <param name="id">O código do produto a ser buscado.</param>
    /// <returns>O <see cref="Produtos"/> correspondente ao ID informado, ou null caso não seja encontrado.</returns>
    public async Task<Produtos?> ObterProdutoPorId(int id)
    {
        // Busca o produto no banco de dados, incluindo os dados da marca associada
        // Utiliza o AsNoTracking para evitar rastreamento desnecessário, já que é apenas uma consulta
        return await _context.Produtos
            .Include(p => p.Marca)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Codigo == id);
    }

    /// <summary>
    /// Cria um novo produto no banco de dados utilizando uma stored procedure.
    /// </summary>
    /// <param name="produto">A entidade <see cref="Produtos"/> contendo os dados a serem inseridos.</param>
    /// <returns>Uma tarefa assíncrona representando a operação de inserção.</returns>
    public async Task CriarProduto(Produtos produto)
    {
        // Executa a stored procedure sp_InsertProduto no banco de dados passando os parâmetros do produto
        // A interpolação de string com $ e parâmetros evita ataques de injeção de SQL e mapeia os tipos
        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_InsertProduto {produto.Nome}, {produto.MarcaId}, {produto.Preco}, {produto.PathImagem}, {(int)produto.Status}");
    }

    /// <summary>
    /// Atualiza os dados de um produto existente utilizando uma stored procedure.
    /// </summary>
    /// <param name="produto">A entidade <see cref="Produtos"/> com os dados atualizados.</param>
    /// <returns>Uma tarefa assíncrona representando a operação de atualização.</returns>
    public async Task AtualizarProduto(Produtos produto)
    {
        // Executa a stored procedure sp_UpdateProduto para atualizar o registro no banco de dados
        // Passa o código do produto para identificar qual registro será alterado, junto com os novos valores
        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_UpdateProduto {produto.Codigo}, {produto.Nome}, {produto.MarcaId}, {produto.Preco}, {produto.PathImagem}, {(int)produto.Status}");
    }

    /// <summary>
    /// Exclui um produto do banco de dados.
    /// </summary>
    /// <param name="produto">A entidade <see cref="Produtos"/> a ser removida.</param>
    /// <returns>Uma tarefa assíncrona representando a operação de exclusão.</returns>
    public async Task ExcluirProduto(Produtos produto)
    {
        // Marca a entidade produto para ser removida no contexto do Entity Framework
        _context.Produtos.Remove(produto);
        
        // Efetiva as alterações (neste caso, o comando de DELETE) no banco de dados de forma assíncrona
        await _context.SaveChangesAsync();
    }
}
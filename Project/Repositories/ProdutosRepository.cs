using Microsoft.EntityFrameworkCore;
using FabysUnha.Models;
using FabysUnha.Data;
using FabysUnha.Enums;
using FabysUnha.Models.SqlViews;

namespace FabysUnha.Repositories;

public class ProdutoRepository : IProdutosRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Produtos>> ObterTodosProdutos()
    {
        var produtosView = await _context.Set<ListaProdutosView>()
            .AsNoTracking()
            .OrderBy(produto => produto.Nome)
            .ToListAsync();

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

    public async Task<Produtos?> ObterProdutoPorId(int id)
    {
        return await _context.Produtos
            .Include(p => p.Marca)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Codigo == id);
    }

    public async Task CriarProduto(Produtos produto)
    {
        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_InsertProduto {produto.Nome}, {produto.MarcaId}, {produto.Preco}, {produto.PathImagem}, {(int)produto.Status}");
    }

    public async Task AtualizarProduto(Produtos produto)
    {
        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_UpdateProduto {produto.Codigo}, {produto.Nome}, {produto.MarcaId}, {produto.Preco}, {produto.PathImagem}, {(int)produto.Status}");
    }

    public async Task ExcluirProduto(Produtos produto)
    {
        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();
    }
}
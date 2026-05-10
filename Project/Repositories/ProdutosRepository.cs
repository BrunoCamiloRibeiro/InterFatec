using Microsoft.EntityFrameworkCore;
using FabysUnha.Models;
using FabysUnha.Data;

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
        return await _context.Produtos.ToListAsync();
    }

    public async Task<Produtos?> ObterProdutoPorId(int id)
    {
        return await _context.Produtos.FindAsync(id);
    }

    public async Task CriarProduto(Produtos produto)
    {
        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarProduto(Produtos produto)
    {
        _context.Produtos.Update(produto);
        await _context.SaveChangesAsync();
    }

    public async Task ExcluirProduto(Produtos produto)
    {
        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();
    }
}
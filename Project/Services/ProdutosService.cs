using FabysUnha.Models;
using FabysUnha.Repositories;

namespace FabysUnha.Services;

public class ProdutosService : IProdutosService
{
    private readonly IProdutosRepository _produtoRepository;

    public ProdutosService(IProdutosRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<IEnumerable<Produtos>> ObterTodosProdutos()
    {
        return await _produtoRepository.ObterTodosProdutos();
    }

    public async Task<Produtos?> ObterProdutoPorId(int id)
    {
        return await _produtoRepository.ObterProdutoPorId(id);
    }

    public async Task CriarProduto(Produtos produto)
    {
        if(produto.Preco <= 0) throw new ArgumentException("O preço do produto deve ser maior que zero.");
        if(produto.MarcaId <= 0) throw new ArgumentException("O produto deve estar associado a uma marca.");
        if(string.IsNullOrWhiteSpace(produto.PathImagem)) produto.PathImagem = "/images/placeholder.jpg";

        await _produtoRepository.CriarProduto(produto);
    }

    public async Task AtualizarProduto(Produtos produto)
    {
        if(produto.Preco <= 0) throw new ArgumentException("O preço do produto deve ser maior que zero.");
        if(produto.MarcaId <= 0) throw new ArgumentException("O produto deve estar associado a uma marca.");
        if(string.IsNullOrWhiteSpace(produto.PathImagem)) produto.PathImagem = "/images/placeholder.jpg";

        await _produtoRepository.AtualizarProduto(produto);
    }

    public async Task ExcluirProduto(int id)
    {
        var produto = await _produtoRepository.ObterProdutoPorId(id);
        if (produto != null)
        {
            await _produtoRepository.ExcluirProduto(produto);
        }
    }
}
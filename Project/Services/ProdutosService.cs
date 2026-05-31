using FabysUnha.Models;
using FabysUnha.Repositories;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace FabysUnha.Services;

public class ProdutosService : IProdutosService
{
    private readonly IProdutosRepository _produtoRepository;
    private readonly IWebHostEnvironment _env;

    public ProdutosService(IProdutosRepository produtoRepository, IWebHostEnvironment env)
    {
        _produtoRepository = produtoRepository;
        _env = env;
    }

    public async Task<IEnumerable<Produtos>> ObterTodosProdutos()
    {
        return await _produtoRepository.ObterTodosProdutos();
    }

    public async Task<Produtos?> ObterProdutoPorId(int id)
    {
        return await _produtoRepository.ObterProdutoPorId(id);
    }

    public async Task CriarProduto(Produtos produto, IFormFile? imagemUpload)
    {
        if(produto.Preco <= 0) throw new ArgumentException("O preço do produto deve ser maior que zero.");
        if(produto.MarcaId <= 0) throw new ArgumentException("O produto deve estar associado a uma marca.");
        
        produto.PathImagem = await SalvarImagemUploadAsync(imagemUpload, produto.PathImagem);
        if(string.IsNullOrWhiteSpace(produto.PathImagem)) produto.PathImagem = "/images/placeholder.jpg";

        await _produtoRepository.CriarProduto(produto);
    }

    public async Task AtualizarProduto(Produtos produto, IFormFile? imagemUpload, bool hasStatusUpdate)
    {
        if(produto.Preco <= 0) throw new ArgumentException("O preço do produto deve ser maior que zero.");
        if(produto.MarcaId <= 0) throw new ArgumentException("O produto deve estar associado a uma marca.");

        var produtoAtual = await _produtoRepository.ObterProdutoPorId(produto.Codigo);
        if (produtoAtual == null) throw new ArgumentException("Produto não encontrado.");

        if (!hasStatusUpdate)
            produto.Status = produtoAtual.Status;

        if (imagemUpload != null && imagemUpload.Length > 0)
        {
            produto.PathImagem = await SalvarImagemUploadAsync(imagemUpload, null);
        }
        else if (string.IsNullOrWhiteSpace(produto.PathImagem))
        {
            produto.PathImagem = produtoAtual.PathImagem;
        }

        if(string.IsNullOrWhiteSpace(produto.PathImagem)) produto.PathImagem = "/images/placeholder.jpg";

        await _produtoRepository.AtualizarProduto(produto);
    }

    private async Task<string?> SalvarImagemUploadAsync(IFormFile? imagemUpload, string? fallbackPath)
    {
        if (imagemUpload != null && imagemUpload.Length > 0)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "produtos");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + imagemUpload.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imagemUpload.CopyToAsync(fileStream);
            }
            return "/images/produtos/" + uniqueFileName;
        }
        return fallbackPath;
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
using FabysUnha.Models;

using Microsoft.AspNetCore.Http;

namespace FabysUnha.Services;

public interface IProdutosService
{
    Task<IEnumerable<Produtos>> ObterTodosProdutos();
    Task<Produtos?> ObterProdutoPorId(int id);
    Task CriarProduto(Produtos produto, IFormFile? imagemUpload);
    Task AtualizarProduto(Produtos produto, IFormFile? imagemUpload, bool hasStatusUpdate);
    Task ExcluirProduto(int id);
}
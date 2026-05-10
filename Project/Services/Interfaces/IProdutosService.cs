using FabysUnha.Models;

namespace FabysUnha.Services;

public interface IProdutosService
{
    Task<IEnumerable<Produtos>> ObterTodosProdutos();
    Task<Produtos?> ObterProdutoPorId(int id);
    Task CriarProduto(Produtos produto);
    Task AtualizarProduto(Produtos produto);
    Task ExcluirProduto(int id);
}
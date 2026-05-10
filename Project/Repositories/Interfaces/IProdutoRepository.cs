using FabysUnha.Models;

namespace FabysUnha.Repositories;

public interface IProdutosRepository
{
    Task<IEnumerable<Produtos>> ObterTodosProdutos();
    Task<Produtos?> ObterProdutoPorId(int id);
    Task CriarProduto(Produtos produto);
    Task AtualizarProduto(Produtos produto);
    Task ExcluirProduto(Produtos produto);
}
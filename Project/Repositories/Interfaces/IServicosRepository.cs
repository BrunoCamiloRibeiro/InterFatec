using FabysUnha.Models;

namespace FabysUnha.Repositories;

public interface IServicosRepository
{
    Task<IEnumerable<Servicos>> ObterTodosServicos();
    Task<Servicos?> ObterServicoPorId(int id);
    Task CriarServico(Servicos servico);
    Task AtualizarServico(Servicos servico);
    Task ExcluirServico(Servicos servico);
}
using FabysUnha.Models;

namespace FabysUnha.Services;

public interface IServicosService
{
    Task<IEnumerable<Servicos>> ObterTodosServicos();
    Task<Servicos?> ObterServicoPorId(int id);
    Task CriarServico(Servicos servico);
    Task AtualizarServico(Servicos servico);
    Task ExcluirServico(int id);
}
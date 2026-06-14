using FabysUnha.Models;

namespace FabysUnha.Services;

/// <summary>
/// Define os métodos obrigatórios para a classe de serviço que manipula os Serviços oferecidos no salão/clínica.
/// Facilita as operações do tipo CRUD sobre as informações dos serviços.
/// </summary>
public interface IServicosService
{
    /// <summary>
    /// Lista todos os serviços prestados que estão armazenados no sistema.
    /// </summary>
    /// <returns>Uma coleção com os objetos do tipo <see cref="Servicos"/>.</returns>
    Task<IEnumerable<Servicos>> ObterTodosServicos();

    /// <summary>
    /// Recupera os detalhes de um serviço específico mediante seu identificador.
    /// </summary>
    /// <param name="id">A chave primária (ID) correspondente ao serviço.</param>
    /// <returns>O objeto do serviço requisitado ou nulo se não houver um serviço com o ID providenciado.</returns>
    Task<Servicos?> ObterServicoPorId(int id);

    /// <summary>
    /// Cadastra as informações relativas a um novo serviço no banco.
    /// </summary>
    /// <param name="servico">A instância de <see cref="Servicos"/> que deve ser guardada.</param>
    /// <returns>A tarefa de gravação assíncrona.</returns>
    Task CriarServico(Servicos servico);

    /// <summary>
    /// Atualiza os campos de um serviço existente (como nome, preço, duração, etc).
    /// </summary>
    /// <param name="servico">O modelo do serviço contendo os dados modificados.</param>
    /// <returns>Ação assíncrona referente à edição de dados do serviço.</returns>
    Task AtualizarServico(Servicos servico);

    /// <summary>
    /// Exclui permanentemente um serviço a partir do seu ID.
    /// </summary>
    /// <param name="id">Identificador do serviço que não será mais ofertado.</param>
    /// <returns>Tarefa assíncrona responsável por deletar o registro.</returns>
    Task ExcluirServico(int id);
}
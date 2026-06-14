using FabysUnha.Models;

namespace FabysUnha.Repositories;

/// <summary>
/// Contrato do repositório de Serviços oferecidos pelo estabelecimento.
/// Gerencia os métodos para o CRUD de serviços, como manicure, pedicure, etc.
/// </summary>
public interface IServicosRepository
{
    // Recuperação de informações

    /// <summary>
    /// Extrai do banco de dados a listagem completa dos serviços ofertados.
    /// </summary>
    /// <returns>Coleção de serviços disponíveis.</returns>
    Task<IEnumerable<Servicos>> ObterTodosServicos();

    /// <summary>
    /// Encontra um serviço isolado através de seu código identificador único (ID).
    /// </summary>
    /// <param name="id">Código do serviço.</param>
    /// <returns>Objeto contendo os detalhes do serviço ou nulo caso não encontre.</returns>
    Task<Servicos?> ObterServicoPorId(int id);

    // Manipulação de registros

    /// <summary>
    /// Insere um novo serviço na base de dados, permitindo a expansão do catálogo de serviços.
    /// </summary>
    /// <param name="servico">A entidade com as informações do novo serviço.</param>
    /// <returns>Ação assíncrona de inclusão de registro.</returns>
    Task CriarServico(Servicos servico);

    /// <summary>
    /// Efetua a atualização de atributos de um serviço (ex: preço, duração, descrição).
    /// </summary>
    /// <param name="servico">O serviço preenchido com as novas informações a serem salvas.</param>
    /// <returns>Ação assíncrona de atualização de registro.</returns>
    Task AtualizarServico(Servicos servico);

    /// <summary>
    /// Remove um serviço do sistema caso ele não seja mais prestado.
    /// </summary>
    /// <param name="servico">A entidade do serviço correspondente à exclusão.</param>
    /// <returns>Ação assíncrona responsável por excluir a entidade.</returns>
    Task ExcluirServico(Servicos servico);
}
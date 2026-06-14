using FabysUnha.Models;
using FabysUnha.Repositories;

namespace FabysUnha.Services;

/// <summary>
/// Serviço encarregado de implementar as regras de negócio para as marcas dos produtos.
/// </summary>
public class MarcasService : IMarcasService
{
    private readonly IMarcasRepository _marcasRepository;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="MarcasService"/> injetando as dependências de repositório.
    /// </summary>
    /// <param name="marcasRepository">Instância do repositório responsável pelo acesso aos dados das marcas.</param>
    public MarcasService(IMarcasRepository marcasRepository)
    {
        // Atribui o repositório instanciado à variável global da classe.
        _marcasRepository = marcasRepository;
    }

    /// <summary>
    /// Recupera todas as marcas registradas no sistema.
    /// </summary>
    /// <returns>Uma lista enumerável de objetos <see cref="Marcas"/>.</returns>
    public async Task<IEnumerable<Marcas>> ObterTodasMarcas()
    {
        // Invoca o método correspondente no repositório para a busca de dados.
        return await _marcasRepository.ObterTodasMarcas();
    }

    /// <summary>
    /// Recupera os detalhes de uma marca específica utilizando o seu ID.
    /// </summary>
    /// <param name="id">O identificador da marca procurada.</param>
    /// <returns>A marca localizada ou null caso o registro não exista.</returns>
    public async Task<Marcas?> ObterMarcaPorId(int id)
    {
        // Busca os dados passando a chave primária para a camada de infraestrutura.
        return await _marcasRepository.ObterMarcaPorId(id);
    }

    /// <summary>
    /// Cadastra uma nova marca no banco de dados.
    /// </summary>
    /// <param name="marca">A entidade preenchida com as informações da nova marca.</param>
    public async Task CriarMarca(Marcas marca)
    {
        // Executa a persistência dos dados acionando o repositório.
        await _marcasRepository.CriarMarca(marca);
    }

    /// <summary>
    /// Atualiza as propriedades de uma marca já existente.
    /// </summary>
    /// <param name="marca">A entidade com os valores novos/modificados.</param>
    public async Task AtualizarMarca(Marcas marca)
    {
        // Solicita ao repositório para salvar as alterações da entidade fornecida.
        await _marcasRepository.AtualizarMarca(marca);
    }

    /// <summary>
    /// Remove uma marca da base de dados se ela existir.
    /// </summary>
    /// <param name="id">O ID correspondente à marca que será deletada.</param>
    public async Task ExcluirMarca(int id)
    {
        // Busca a entidade completa no banco antes da remoção, certificando-se de que a mesma existe.
        var marca = await _marcasRepository.ObterMarcaPorId(id);
        
        // Verifica se a marca existe. Caso seja diferente de nulo, efetua a exclusão.
        if (marca != null) 
        {
            await _marcasRepository.ExcluirMarca(marca);
        }
    }
}
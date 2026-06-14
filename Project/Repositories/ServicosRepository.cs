using Microsoft.EntityFrameworkCore;
using FabysUnha.Models;
using FabysUnha.Data;
using FabysUnha.Enums;
using FabysUnha.Models.SqlViews;

namespace FabysUnha.Repositories;

/// <summary>
/// Repositório responsável pelas operações de acesso a dados da entidade Servicos.
/// Implementa a interface <see cref="IServicosRepository"/> fornecendo a lógica de persistência e leitura.
/// </summary>
public class ServicosRepository : IServicosRepository
{
    /// <summary>
    /// Contexto do banco de dados utilizado para interagir com o Entity Framework Core.
    /// </summary>
    private readonly AppDbContext _context;

    /// <summary>
    /// Construtor do repositório de serviços.
    /// </summary>
    /// <param name="context">Instância do <see cref="AppDbContext"/> injetada via injeção de dependência.</param>
    public ServicosRepository(AppDbContext context)
    {
        // Atribui o contexto injetado à variável privada para ser utilizado nos métodos do repositório
        _context = context;
    }

    /// <summary>
    /// Obtém todos os serviços cadastrados no sistema utilizando uma view otimizada do banco de dados.
    /// </summary>
    /// <returns>Uma lista assíncrona contendo todos os <see cref="Servicos"/>.</returns>
    public async Task<IEnumerable<Servicos>> ObterTodosServicos()
    {
        // Consulta a view ListaServicosView mapeada no banco de dados
        // Utiliza o AsNoTracking para melhorar a performance, pois não precisamos rastrear alterações nessas entidades
        // Ordena os resultados pela descrição do serviço em ordem alfabética
        var servicosView = await _context.Set<ListaServicosView>()
            .AsNoTracking()
            .OrderBy(servico => servico.Descricao)
            .ToListAsync();

        // Mapeia os dados retornados da view para a entidade principal Servicos
        return servicosView.Select(servico => new Servicos
        {
            Id = servico.Id,
            Descricao = servico.Descricao,
            Preco = servico.Preco,
            Tempo = servico.Tempo,
            Status = (ServicoStatus)servico.StatusId
        }).ToList();
    }

    /// <summary>
    /// Obtém um serviço específico com base no seu ID identificador.
    /// </summary>
    /// <param name="id">O ID do serviço a ser buscado.</param>
    /// <returns>O <see cref="Servicos"/> correspondente ao ID informado, ou null caso não seja encontrado.</returns>
    public async Task<Servicos?> ObterServicoPorId(int id)
    {
        // Procura a entidade pela chave primária de forma assíncrona e rápida utilizando o método FindAsync
        return await _context.Servicos.FindAsync(id);
    }

    /// <summary>
    /// Cria um novo serviço no banco de dados utilizando uma stored procedure.
    /// </summary>
    /// <param name="servico">A entidade <see cref="Servicos"/> contendo os dados a serem inseridos.</param>
    /// <returns>Uma tarefa assíncrona representando a operação de inserção.</returns>
    public async Task CriarServico(Servicos servico)
    {
        // Executa a stored procedure sp_InsertServico no banco de dados passando os parâmetros do serviço
        // A interpolação de string protege contra injeção SQL no Entity Framework Core
        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_InsertServico {servico.Preco}, {servico.Descricao}, {servico.Tempo}, {(int)servico.Status}");
    }

    /// <summary>
    /// Atualiza os dados de um serviço existente utilizando uma stored procedure.
    /// </summary>
    /// <param name="servico">A entidade <see cref="Servicos"/> com os dados atualizados.</param>
    /// <returns>Uma tarefa assíncrona representando a operação de atualização.</returns>
    public async Task AtualizarServico(Servicos servico)
    {
        // Executa a stored procedure sp_UpdateServico para atualizar o registro no banco de dados
        // Passa o ID do serviço para identificar o registro a ser modificado
        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_UpdateServico {servico.Id}, {servico.Preco}, {servico.Descricao}, {servico.Tempo}, {(int)servico.Status}");
    }

    /// <summary>
    /// Exclui um serviço do banco de dados.
    /// </summary>
    /// <param name="servico">A entidade <see cref="Servicos"/> a ser removida.</param>
    /// <returns>Uma tarefa assíncrona representando a operação de exclusão.</returns>
    public async Task ExcluirServico(Servicos servico)
    {
        // Remove a entidade serviço no contexto de memória do Entity Framework
        _context.Servicos.Remove(servico);
        
        // Aplica e salva as mudanças (executando o DELETE) no banco de dados
        await _context.SaveChangesAsync();
    }
}
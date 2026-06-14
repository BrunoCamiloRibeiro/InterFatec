using Microsoft.EntityFrameworkCore;
using FabysUnha.Models;
using FabysUnha.Data;
using FabysUnha.Enums;
using FabysUnha.Models.SqlViews;

namespace FabysUnha.Repositories;

/// <summary>
/// Repositório para o gerenciamento de Marcas no banco de dados.
/// Implementa a interface <see cref="IMarcasRepository"/>.
/// </summary>
public class MarcasRepository : IMarcasRepository
{
    private readonly AppDbContext _db;

    /// <summary>
    /// Construtor da classe de repositório.
    /// </summary>
    /// <param name="db">O contexto utilizado para se comunicar com o banco de dados.</param>
    public MarcasRepository(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Retorna todas as marcas ativas ou inativas ordenadas pelo nome.
    /// </summary>
    /// <returns>Uma lista de instâncias da entidade <see cref="Marcas"/>.</returns>
    public async Task<IEnumerable<Marcas>> ObterTodasMarcas()
    {
        // Utiliza uma view do banco de dados para recuperar as marcas e garantir otimização
        var marcasView = await _db.Set<ListaMarcasView>()
            .AsNoTracking()
            .OrderBy(marca => marca.Nome)
            .ToListAsync();

        // Converte o modelo visualizado (View) para a entidade do sistema
        return marcasView.Select(marca => new Marcas
        {
            Id = marca.Id,
            Nome = marca.Nome,
            Status = (MarcaStatus)marca.Status
        }).ToList();
    }

    /// <summary>
    /// Retorna uma marca específica procurando pelo seu código identificador.
    /// </summary>
    /// <param name="id">O ID numérico da marca.</param>
    /// <returns>A marca localizada ou null caso não seja encontrada.</returns>
    public async Task<Marcas?> ObterMarcaPorId(int id)
    {
        // Encontra a marca diretamente pela chave primária na tabela
        return await _db.Marcas.FindAsync(id);
    }

    /// <summary>
    /// Registra uma nova marca executando um procedimento armazenado.
    /// </summary>
    /// <param name="marca">A marca contendo os dados a serem criados.</param>
    public async Task CriarMarca(Marcas marca)
    {
        // Chama a Stored Procedure para inserir uma marca
        await _db.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_InsertMarca {marca.Nome}, {(int)marca.Status}");
    }

    /// <summary>
    /// Altera os dados de uma marca existente.
    /// </summary>
    /// <param name="marca">A marca com as novas informações preenchidas.</param>
    public async Task AtualizarMarca(Marcas marca)
    {
        // Executa a procedure de atualização repassando as novas informações
        await _db.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_UpdateMarca {marca.Id}, {marca.Nome}, {(int)marca.Status}");
    }

    /// <summary>
    /// Exclui logicamente a marca do sistema (inativa a marca).
    /// </summary>
    /// <param name="marca">A marca que deve ser inativada.</param>
    public async Task ExcluirMarca(Marcas marca)
    {
        // Exclusão do tipo Soft Delete (mantém o dado no banco, mas inativo)
        marca.Status = MarcaStatus.Inativo;
        
        // Dispara a procedure para salvar o novo status
        await _db.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_UpdateMarca {marca.Id}, {marca.Nome}, {(int)marca.Status}");
    }
}
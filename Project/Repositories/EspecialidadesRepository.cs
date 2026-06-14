using Microsoft.EntityFrameworkCore;
using FabysUnha.Models;
using FabysUnha.Data;
using FabysUnha.Enums;

namespace FabysUnha.Repositories;

/// <summary>
/// Repositório para gerenciar o acesso a dados da entidade Especialidades.
/// Implementa a interface <see cref="IEspecialidadeRepository"/>.
/// </summary>
public class EspecialidadesRepository : IEspecialidadeRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Inicializa o repositório com o contexto do banco de dados.
    /// </summary>
    /// <param name="context">O contexto do Entity Framework.</param>
    public EspecialidadesRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtém todas as especialidades cadastradas.
    /// </summary>
    /// <returns>Uma coleção de especialidades.</returns>
    public async Task<IEnumerable<Especialidades>> ObterTodasEspecialidades()
    {
        // Retorna todos os registros da tabela de especialidades de forma assíncrona
        return await _context.Especialidades.ToListAsync();
    }

    /// <summary>
    /// Busca uma especialidade específica pelo seu identificador (ID).
    /// </summary>
    /// <param name="id">O ID da especialidade a ser buscada.</param>
    /// <returns>A especialidade encontrada ou nulo se não existir.</returns>
    public async Task<Especialidades?> ObterEspecialidadePorId(int id)
    {
        // Procura a especialidade pela chave primária
        return await _context.Especialidades.FindAsync(id);
    }

    /// <summary>
    /// Insere uma nova especialidade no banco de dados.
    /// </summary>
    /// <param name="especialidade">Os dados da especialidade a ser criada.</param>
    public async Task CriarEspecialidade(Especialidades especialidade)
    {
        // Utiliza uma stored procedure via interpolação segura para salvar os dados
        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_InsertEspecialidade {especialidade.Descricao}, {(int)especialidade.Status}");
    }

    /// <summary>
    /// Atualiza uma especialidade existente.
    /// </summary>
    /// <param name="especialidade">A especialidade com as modificações desejadas.</param>
    public async Task AtualizarEspecialidade(Especialidades especialidade)
    {
        // Executa a stored procedure de atualização enviando o ID e os novos dados
        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_UpdateEspecialidade {especialidade.Id}, {especialidade.Descricao}, {(int)especialidade.Status}");
    }

    /// <summary>
    /// Realiza a exclusão lógica da especialidade (alterando o status para Inativo).
    /// </summary>
    /// <param name="especialidade">A especialidade a ser excluída logicamente.</param>
    public async Task ExcluirEspecialidade(Especialidades especialidade)
    {
        // Marca o status como inativo para não excluir definitivamente (soft delete)
        especialidade.Status = EspecialidadeStatus.Inativo;

        // Persiste a mudança de status usando a procedure de atualização
        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_UpdateEspecialidade {especialidade.Id}, {especialidade.Descricao}, {(int)especialidade.Status}");
    }
}
using Microsoft.EntityFrameworkCore;
using FabysUnha.Models;
using FabysUnha.Data;
using FabysUnha.Enums;
using FabysUnha.Models.SqlViews;

namespace FabysUnha.Repositories;

/// <summary>
/// Repositório responsável pelo acesso a dados da entidade Funcionarios.
/// Implementa as operações definidas em <see cref="IFuncionariosRepository"/>.
/// </summary>
public class FuncionariosRepository : IFuncionariosRepository
{
    private readonly AppDbContext _db;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="FuncionariosRepository"/>.
    /// </summary>
    /// <param name="db">O contexto de banco de dados do Entity Framework.</param>
    public FuncionariosRepository(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Obtém a lista de todos os funcionários ordenados alfabeticamente pelo nome.
    /// </summary>
    /// <returns>Uma lista de objetos <see cref="Funcionarios"/>.</returns>
    public async Task<IEnumerable<Funcionarios>> ObterTodosFuncionarios()
    {
        // Busca a lista a partir de uma view específica para evitar joins complexos repetitivos
        var funcionariosView = await _db.Set<ListaFuncionariosView>()
            .AsNoTracking()
            .OrderBy(funcionario => funcionario.Nome)
            .ToListAsync();

        // Converte a projeção da view para o objeto de domínio completo
        return funcionariosView.Select(funcionario => new Funcionarios
        {
            Id = funcionario.Id,
            Nome = funcionario.Nome ?? string.Empty,
            Telefone = funcionario.Telefone ?? string.Empty,
            Status = (PessoaStatus)funcionario.StatusId,
            Salario = funcionario.Salario,
            Senha = funcionario.Senha ?? string.Empty,
            // Preenche o objeto de especialidade apenas se a descrição existir
            Especialidade = !string.IsNullOrEmpty(funcionario.Especialidade)
                ? new Especialidades { Descricao = funcionario.Especialidade }
                : null
        }).ToList();
    }

    /// <summary>
    /// Busca as informações de um funcionário detalhado pelo seu ID.
    /// </summary>
    /// <param name="id">O identificador único do funcionário.</param>
    /// <returns>O funcionário encontrado ou null se não for localizado.</returns>
    public async Task<Funcionarios?> ObterFuncionarioPorId(int id)
    {
        // Busca os dados básicos do funcionário pela view
        var funcionarioView = await _db.Set<ListaFuncionariosView>()
            .AsNoTracking()
            .FirstOrDefaultAsync(funcionario => funcionario.Id == id);

        // Retorna null caso o funcionário não seja encontrado
        if (funcionarioView == null)
            return null;

        // Mapeia os dados da view para uma nova instância do modelo de Funcionarios
        var funcionario = new Funcionarios
        {
            Id = funcionarioView.Id,
            Nome = funcionarioView.Nome ?? string.Empty,
            Telefone = funcionarioView.Telefone ?? string.Empty,
            Status = (PessoaStatus)funcionarioView.StatusId,
            Salario = funcionarioView.Salario,
            Senha = funcionarioView.Senha ?? string.Empty,
            // Define a especialidade baseada na descrição retornada pela view
            Especialidade = !string.IsNullOrEmpty(funcionarioView.Especialidade)
                ? new Especialidades { Descricao = funcionarioView.Especialidade }
                : null
        };

        // Busca todos os serviços que este funcionário já tem agendados
        var servicosAgendados = await _db.Servicos_Agendados
            .AsNoTracking()
            // Traz também as informações do próprio serviço (tabela de domínio Serviço)
            .Include(servicoAgendado => servicoAgendado.Servico)
            .Where(servicoAgendado => servicoAgendado.FuncionarioId == id)
            .ToListAsync();

        // Faz o vínculo de volta com a instância atual do funcionário para manter as referências corretas
        foreach (var servicoAgendado in servicosAgendados)
        {
            servicoAgendado.Funcionario = funcionario;
        }

        // Associa a lista preenchida à propriedade do funcionário
        funcionario.Servicos_Agendados = servicosAgendados;
        return funcionario;
    }

    /// <summary>
    /// Registra um novo funcionário chamando a procedure no banco de dados.
    /// </summary>
    /// <param name="funcionario">As informações do funcionário a ser salvo.</param>
    public async Task RegistrarFuncionario(Funcionarios funcionario)
    {
        // Executa a stored procedure sp_InsertFuncionario para adicionar um novo registro
        await _db.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_InsertFuncionario {funcionario.Nome}, {funcionario.Telefone}, {(int)funcionario.Status}, {funcionario.Salario}, {funcionario.EspecialidadeId}, {funcionario.Senha}");
    }

    /// <summary>
    /// Atualiza as informações de um funcionário.
    /// </summary>
    /// <param name="funcionario">A entidade com as modificações que devem ser salvas.</param>
    public async Task AtualizarFuncionario(Funcionarios funcionario)
    {
        // Executa a stored procedure passando os dados do funcionário, inclusive seu ID para a cláusula WHERE interna
        await _db.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_UpdateFuncionario {funcionario.Id}, {funcionario.Nome}, {funcionario.Telefone}, {(int)funcionario.Status}, {funcionario.Salario}, {funcionario.EspecialidadeId}, {funcionario.Senha}");
    }

    /// <summary>
    /// Desativa um funcionário (Exclusão lógica).
    /// </summary>
    /// <param name="funcionario">O funcionário a ser inativado.</param>
    public async Task ExcluirFuncionario(Funcionarios funcionario)
    {
        // Altera o status para Inativo, garantindo que o histórico de agendamentos não se perca (soft delete)
        funcionario.Status = PessoaStatus.Inativo;
        
        // Salva a alteração de status através da procedure padrão de update
        await _db.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_UpdateFuncionario {funcionario.Id}, {funcionario.Nome}, {funcionario.Telefone}, {(int)funcionario.Status}, {funcionario.Salario}, {funcionario.EspecialidadeId}, {funcionario.Senha}");
    }
}
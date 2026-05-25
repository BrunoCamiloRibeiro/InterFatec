using Microsoft.EntityFrameworkCore;
using FabysUnha.Models;
using FabysUnha.Data;
using FabysUnha.Enums;
using FabysUnha.Models.SqlViews;

namespace FabysUnha.Repositories;

public class FuncionariosRepository : IFuncionariosRepository
{
    private readonly AppDbContext _db;

    public FuncionariosRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Funcionarios>> ObterTodosFuncionarios()
    {
        var funcionariosView = await _db.Set<ListaFuncionariosView>()
            .AsNoTracking()
            .OrderBy(funcionario => funcionario.Nome)
            .ToListAsync();

        return funcionariosView.Select(funcionario => new Funcionarios
        {
            Id = funcionario.Id,
            Nome = funcionario.Nome,
            Telefone = funcionario.Telefone,
            Status = (PessoaStatus)funcionario.StatusId,
            Salario = funcionario.Salario,
            Especialidade = new Especialidades
            {
                Descricao = funcionario.Especialidade
            }
        }).ToList();
    }

    public async Task<Funcionarios?> ObterFuncionarioPorId(int id)
    {
        var funcionarioView = await _db.Set<ListaFuncionariosView>()
            .AsNoTracking()
            .FirstOrDefaultAsync(funcionario => funcionario.Id == id);

        if (funcionarioView == null)
            return null;

        var funcionario = new Funcionarios
        {
            Id = funcionarioView.Id,
            Nome = funcionarioView.Nome,
            Telefone = funcionarioView.Telefone,
            Status = (PessoaStatus)funcionarioView.StatusId,
            Salario = funcionarioView.Salario,
            Especialidade = new Especialidades
            {
                Descricao = funcionarioView.Especialidade
            }
        };

        var servicosAgendados = await _db.Servicos_Agendados
            .AsNoTracking()
            .Include(servicoAgendado => servicoAgendado.Servico)
            .Where(servicoAgendado => servicoAgendado.FuncionarioId == id)
            .ToListAsync();

        foreach (var servicoAgendado in servicosAgendados)
        {
            servicoAgendado.Funcionario = funcionario;
        }

        funcionario.Servicos_Agendados = servicosAgendados;
        return funcionario;
    }

    public async Task RegistrarFuncionario(Funcionarios funcionario)
    {
        await _db.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_InsertFuncionario {funcionario.Nome}, {funcionario.Telefone}, {(int)funcionario.Status}, {funcionario.Salario}, {funcionario.EspecialidadeId}");
    }

    public async Task AtualizarFuncionario(Funcionarios funcionario)
    {
        await _db.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC sp_UpdateFuncionario {funcionario.Id}, {funcionario.Nome}, {funcionario.Telefone}, {(int)funcionario.Status}, {funcionario.Salario}, {funcionario.EspecialidadeId}");
    }

    public async Task ExcluirFuncionario(Funcionarios funcionario)
    {
        _db.Funcionarios.Remove(funcionario);
        await _db.SaveChangesAsync();
    }
}
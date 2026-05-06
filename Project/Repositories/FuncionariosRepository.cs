using Microsoft.EntityFrameworkCore;
using FabysUnha.Models;
using FabysUnha.Data;

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
        return await _db.Funcionarios
            .Include(f => f.Especialidade) 
            .ToListAsync();
    }

    public async Task<Funcionarios?> ObterFuncionarioPorId(int id)
    {
        return await _db.Funcionarios
            .Include(f => f.Especialidade)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task RegistrarFuncionario(Funcionarios funcionario)
    {
        _db.Funcionarios.Add(funcionario);
        await _db.SaveChangesAsync();
    }

    public async Task AtualizarFuncionario(Funcionarios funcionario)
    {
        _db.Funcionarios.Update(funcionario);
        await _db.SaveChangesAsync();
    }

    public async Task ExcluirFuncionario(Funcionarios funcionario)
    {
        _db.Funcionarios.Remove(funcionario);
        await _db.SaveChangesAsync();
    }
}
using FabysUnha.Models;

namespace FabysUnha.Repositories;

public interface IFuncionariosRepository
{
    Task<IEnumerable<Funcionarios>> ObterTodosFuncionarios();
    Task<Funcionarios?> ObterFuncionarioPorId(int id);
    Task RegistrarFuncionario(Funcionarios funcionario);
    Task AtualizarFuncionario(Funcionarios funcionario);
    Task ExcluirFuncionario(Funcionarios funcionario);
}
using FabysUnha.Models;

namespace FabysUnha.Services;

public interface IFuncionariosService
{
    Task<IEnumerable<Funcionarios>> ObterTodosFuncionarios();
    Task<Funcionarios?> ObterFuncionarioPorId(int id);
    Task RegistrarFuncionario(Funcionarios funcionario);
    Task AtualizarFuncionario(Funcionarios funcionario  );
    Task ExcluirFuncionario(int id);
}
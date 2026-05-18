using FabysUnha.Models;
using FabysUnha.Repositories;

namespace FabysUnha.Services;

public class FuncionariosService : IFuncionariosService
{
    private readonly IFuncionariosRepository _funcionariosRepository;

    public FuncionariosService(IFuncionariosRepository funcionariosRepository)
    {
        _funcionariosRepository = funcionariosRepository;
    }

    public async Task<IEnumerable<Funcionarios>> ObterTodosFuncionarios()
    {
        return await _funcionariosRepository.ObterTodosFuncionarios();
    }

    public async Task<Funcionarios?> ObterFuncionarioPorId(int id)
    {
        return await _funcionariosRepository.ObterFuncionarioPorId(id);
    }

    public async Task RegistrarFuncionario(Funcionarios funcionario)
    {
        if(funcionario.Salario <= 0) throw new ArgumentException("O salário deve ser um valor positivo.");
        if(funcionario.Salario < 1412.00m) throw new ArgumentException("O salário deve ser no mínimo o valor do salário mínimo.");
        
        await _funcionariosRepository.RegistrarFuncionario(funcionario);
    }

    public async Task AtualizarFuncionario(Funcionarios funcionario)
    {
        if(funcionario.Salario <= 0) throw new ArgumentException("O salário deve ser um valor positivo.");
        if(funcionario.Salario < 1412.00m) throw new ArgumentException("O salário deve ser no mínimo o valor do salário mínimo.");

        await _funcionariosRepository.AtualizarFuncionario(funcionario);
    }

    public async Task ExcluirFuncionario(int id)
    {
        var funcionario = await _funcionariosRepository.ObterFuncionarioPorId(id);
        if (funcionario != null)  await _funcionariosRepository.ExcluirFuncionario(funcionario);        
    }
}
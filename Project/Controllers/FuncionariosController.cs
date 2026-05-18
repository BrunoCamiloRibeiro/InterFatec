using Microsoft.AspNetCore.Mvc;
using FabysUnha.Services;
using FabysUnha.ViewModels;
using AutoMapper;

namespace FabysUnha.Controllers;

public class FuncionariosController : Controller
{
    private readonly IFuncionariosService _funcionariosService;
    private readonly IMapper _mapper;

    public FuncionariosController(IFuncionariosService funcionariosService, IMapper mapper)
    {
        _funcionariosService = funcionariosService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var funcionarios = await _funcionariosService.ObterTodosFuncionarios();
        var funcionariosViewModel = _mapper.Map<IEnumerable<FuncionarioListagemViewModel>>(funcionarios);
        return View(funcionariosViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Detalhes(int id)
    {
        var funcionario = await _funcionariosService.ObterFuncionarioPorId(id);
        if (funcionario == null) return NotFound();

        var funcionarioViewModel = _mapper.Map<FuncionarioDetalhesViewModel>(funcionario);
        return View(funcionarioViewModel);
    }

    [HttpGet]
    public IActionResult Criar()
    {
        return View(new FuncionarioRegistroViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Criar(FuncionarioRegistroViewModel funcionarioViewModel)
    {
        if (!ModelState.IsValid) return View(funcionarioViewModel);

        try
        {
            var funcionario = _mapper.Map<Models.Funcionarios>(funcionarioViewModel);
            await _funcionariosService.RegistrarFuncionario(funcionario);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro ao registrar funcionário: {ex.Message}");
            return View(funcionarioViewModel);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var funcionario = await _funcionariosService.ObterFuncionarioPorId(id);
        if (funcionario == null) return NotFound();

        var funcionarioViewModel = _mapper.Map<FuncionarioEditarViewModel>(funcionario);
        return View(funcionarioViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, FuncionarioEditarViewModel funcionarioViewModel)
    {
        if (id != funcionarioViewModel.Id) return BadRequest();
        if (!ModelState.IsValid) return View(funcionarioViewModel);

        try
        {
            var funcionarioAtual = await _funcionariosService.ObterFuncionarioPorId(id);
            if (funcionarioAtual == null) return NotFound();

            if (!Request.HasFormContentType || !Request.Form.ContainsKey(nameof(funcionarioViewModel.Status)))
                funcionarioViewModel.Status = funcionarioAtual.Status;

            var funcionario = _mapper.Map<Models.Funcionarios>(funcionarioViewModel);
            await _funcionariosService.AtualizarFuncionario(funcionario);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro ao atualizar funcionário: {ex.Message}");
            return View(funcionarioViewModel);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Excluir(int id)
    {
        var funcionario = await _funcionariosService.ObterFuncionarioPorId(id);
        if (funcionario == null) return NotFound();

        var funcionarioViewModel = _mapper.Map<FuncionarioDetalhesViewModel>(funcionario);
        return View(funcionarioViewModel);
    }

    [HttpPost, ActionName("Excluir")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExcluirConfirmado(int id)
    {
        await _funcionariosService.ExcluirFuncionario(id);
        return RedirectToAction(nameof(Index));
    }
}
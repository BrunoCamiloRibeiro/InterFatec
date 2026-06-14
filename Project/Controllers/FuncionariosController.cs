using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using FabysUnha.Services;
using FabysUnha.ViewModels;
using AutoMapper;

namespace FabysUnha.Controllers;

public class FuncionariosController : Controller
{
    private readonly IFuncionariosService _funcionariosService;
    private readonly IEspecialidadeService _especialidadeService;
    private readonly IMapper _mapper;

    public FuncionariosController(
        IFuncionariosService funcionariosService,
        IEspecialidadeService especialidadeService,
        IMapper mapper)
    {
        _funcionariosService = funcionariosService;
        _especialidadeService = especialidadeService;
        _mapper = mapper;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var tipoUsuario = HttpContext.Session.GetString("UsuarioTipo");
        if (tipoUsuario != "Funcionario")
        {
            context.Result = new RedirectToActionResult("Index", "Login", null);
        }
        base.OnActionExecuting(context);
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
    public async Task<IActionResult> Criar()
    {
        var funcionarioViewModel = new FuncionarioRegistroViewModel();

        // Alimenta a lista de especialidades logo no primeiro carregamento
        await PrepararEspecialidadesAsync(funcionarioViewModel);

        return View(funcionarioViewModel);
    }

    [HttpPost]

    public async Task<IActionResult> Criar(FuncionarioRegistroViewModel funcionarioViewModel)
    {
        await PrepararEspecialidadesAsync(funcionarioViewModel);

        if (!ModelState.IsValid) return View(funcionarioViewModel);

        try
        {

            if (string.IsNullOrWhiteSpace(funcionarioViewModel.Senha))
        {
            ModelState.AddModelError(nameof(funcionarioViewModel.Senha), "A senha é obrigatória.");
            return View(funcionarioViewModel);
        }

            var funcionario = _mapper.Map<Models.Funcionarios>(funcionarioViewModel);
            await _funcionariosService.RegistrarFuncionario(funcionario);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro ao registrar funcionário: {ex.Message}");
            await PrepararEspecialidadesAsync(funcionarioViewModel);
            return View(funcionarioViewModel);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var funcionario = await _funcionariosService.ObterFuncionarioPorId(id);
        if (funcionario == null) return NotFound();

        var funcionarioViewModel = _mapper.Map<FuncionarioEditarViewModel>(funcionario);

        await PrepararEspecialidadesAsync(funcionarioViewModel);
        return View(funcionarioViewModel);
    }

    [HttpPost]

    public async Task<IActionResult> Editar(int id, FuncionarioEditarViewModel funcionarioViewModel)
    {
        if (id != funcionarioViewModel.Id) return BadRequest();

        await PrepararEspecialidadesAsync(funcionarioViewModel);

        if (!ModelState.IsValid) return View(funcionarioViewModel);

        try
        {
            bool hasStatusUpdate = Request.HasFormContentType && Request.Form.ContainsKey(nameof(funcionarioViewModel.Status));

            var funcionario = _mapper.Map<Models.Funcionarios>(funcionarioViewModel);
            
            await _funcionariosService.AtualizarFuncionario(funcionario, hasStatusUpdate);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro ao atualizar funcionário: {ex.Message}");
            await PrepararEspecialidadesAsync(funcionarioViewModel);
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

    public async Task<IActionResult> ExcluirConfirmado(int id)
    {
        await _funcionariosService.ExcluirFuncionario(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task PrepararEspecialidadesAsync(FuncionarioRegistroViewModel viewModel)
    {
        var ativas = (await _especialidadeService.ObterTodasEspecialidades())
            .Where(e => e.Status == FabysUnha.Enums.EspecialidadeStatus.Ativo)
            .ToList();

        viewModel.EspecialidadesList = new SelectList(
            ativas,
            nameof(Models.Especialidades.Id),
            nameof(Models.Especialidades.Descricao),
            viewModel.EspecialidadeId);
    }

    private async Task PrepararEspecialidadesAsync(FuncionarioEditarViewModel viewModel)
    {
        var ativasEAtual = (await _especialidadeService.ObterTodasEspecialidades())
            .Where(e => e.Status == FabysUnha.Enums.EspecialidadeStatus.Ativo || e.Id == viewModel.EspecialidadeId)
            .ToList();

        viewModel.EspecialidadesList = new SelectList(
            ativasEAtual,
            nameof(Models.Especialidades.Id),
            nameof(Models.Especialidades.Descricao),
            viewModel.EspecialidadeId);
    }
}
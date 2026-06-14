using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using FabysUnha.Services;
using FabysUnha.ViewModels;
using AutoMapper;

namespace FabysUnha.Controllers;


public class EspecialidadesController : Controller
{
    private readonly IEspecialidadeService _especialidadeService;
    private readonly IMapper _mapper;

    public EspecialidadesController(IEspecialidadeService especialidadeService, IMapper mapper)
    {
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
        var especialidades = await _especialidadeService.ObterTodasEspecialidades();
        var especialidadesViewModel = _mapper.Map<IEnumerable<EspecialidadeViewModel>>(especialidades);
        return View(especialidadesViewModel);
    }

    [HttpGet]
    public IActionResult Criar()
    {
        return View(new EspecialidadeViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Criar(EspecialidadeViewModel especialidadeViewModel)
    {
        if (!ModelState.IsValid) return View(especialidadeViewModel);

        var especialidade = _mapper.Map<Models.Especialidades>(especialidadeViewModel);
        await _especialidadeService.CriarEspecialidade(especialidade);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var especialidade = await _especialidadeService.ObterEspecialidadePorId(id);
        if (especialidade == null) return NotFound();

        var viewModel = _mapper.Map<EspecialidadeViewModel>(especialidade);
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(int id, EspecialidadeViewModel viewModel)
    {
        if (id != viewModel.Id) return BadRequest();
        if (!ModelState.IsValid) return View(viewModel);

        var especialidade = _mapper.Map<Models.Especialidades>(viewModel);
        await _especialidadeService.AtualizarEspecialidade(especialidade);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Excluir(int id)
    {
        var especialidade = await _especialidadeService.ObterEspecialidadePorId(id);
        if (especialidade == null) return NotFound();

        var viewModel = _mapper.Map<EspecialidadeViewModel>(especialidade);
        return View(viewModel);
    }

    [HttpPost, ActionName("Excluir")]
    public async Task<IActionResult> ExcluirConfirmado(int id)
    {
        await _especialidadeService.ExcluirEspecialidade(id);
        return RedirectToAction(nameof(Index));
    }
}
using Microsoft.AspNetCore.Mvc;
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

    public async Task<IActionResult> Index()
    {
        var especialidades = await _especialidadeService.ObterTodasEspecialidades();
        var especialidadesViewModel = _mapper.Map<IEnumerable<EspecialidadeViewModel>>(especialidades);
        return View(especialidadesViewModel);
    }

    [HttpGet]
    public IActionResult Criar()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Criar(EspecialidadeViewModel especialidadeViewModel)
    {
        if (!ModelState.IsValid) return View(especialidadeViewModel);

        var especialidade = _mapper.Map<Models.Especialidades>(especialidadeViewModel);
        await _especialidadeService.CriarEspecialidade(especialidade);
        return RedirectToAction(nameof(Index));
    }
}
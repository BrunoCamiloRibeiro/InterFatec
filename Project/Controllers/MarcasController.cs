using Microsoft.AspNetCore.Mvc;
using FabysUnha.Services;
using FabysUnha.ViewModels;
using AutoMapper;

namespace FabysUnha.Controllers;

public class MarcasController : Controller
{
    private readonly IMarcaService _marcaService;
    private readonly IMapper _mapper;
    public MarcasController(IMarcaService marcaService, IMapper mapper)
    {
        _marcaService = marcaService;
        _mapper = mapper;
    }

    public IActionResult Index()
    {
        var marcas = _marcaService.ObterTodasMarcas().Result;
        var marcasViewModel = _mapper.Map<IEnumerable<MarcasViewModel>>(marcas);
        return View(marcasViewModel);
    }

    [HttpGet]
    public IActionResult Criar()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Criar(MarcasViewModel marcaViewModel)
    {
        if (!ModelState.IsValid) return View(marcaViewModel);

        var marca = _mapper.Map<Models.Marcas>(marcaViewModel);
        await _marcaService.CriarMarca(marca);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var marca = await _marcaService.ObterMarcaPorId(id);

        if (marca == null) return NotFound();

        var marcaViewModel = _mapper.Map<MarcasViewModel>(marca);
        return View(marcaViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(MarcasViewModel marcaViewModel)
    {
        if (!ModelState.IsValid) return View(marcaViewModel);

        var marca = _mapper.Map<Models.Marcas>(marcaViewModel);
        await _marcaService.AtualizarMarca(marca);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Excluir(int id)
    {
        var marca = await _marcaService.ObterMarcaPorId(id);

        if (marca == null) return NotFound();

        var marcaViewModel = _mapper.Map<MarcasViewModel>(marca);
        return View(marcaViewModel);
    }

    [HttpPost, ActionName("Excluir")]
    public async Task<IActionResult> ConfirmarExclusao(int id)
    {
        await _marcaService.ExcluirMarca(id);
        return RedirectToAction(nameof(Index));
    }
}
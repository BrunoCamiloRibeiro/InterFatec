using Microsoft.AspNetCore.Mvc;
using FabysUnha.Services;
using FabysUnha.ViewModels;
using AutoMapper;

namespace FabysUnha.Controllers;

public class MarcasController : Controller
{
    private readonly IMarcasService _MarcasService;
    private readonly IMapper _mapper;
    public MarcasController(IMarcasService MarcasService, IMapper mapper)
    {
        _MarcasService = MarcasService;
        _mapper = mapper;
    }

    public IActionResult Index()
    {
        var marcas = _MarcasService.ObterTodasMarcas().Result;
        var marcasViewModel = _mapper.Map<IEnumerable<MarcasViewModel>>(marcas);
        return View(marcasViewModel);
    }

    [HttpGet]
    public IActionResult Criar()
    {
        return View(new MarcasViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Criar(MarcasViewModel marcaViewModel)
    {
        if (!ModelState.IsValid) return View(marcaViewModel);

        var marca = _mapper.Map<Models.Marcas>(marcaViewModel);
        await _MarcasService.CriarMarca(marca);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var marca = await _MarcasService.ObterMarcaPorId(id);

        if (marca == null) return NotFound();

        var marcaViewModel = _mapper.Map<MarcasViewModel>(marca);
        return View(marcaViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(MarcasViewModel marcaViewModel)
    {
        if (!ModelState.IsValid) return View(marcaViewModel);

        var marcaAtual = await _MarcasService.ObterMarcaPorId(marcaViewModel.Id);
        if (marcaAtual == null) return NotFound();

        if (!Request.HasFormContentType || !Request.Form.ContainsKey(nameof(marcaViewModel.Status)))
            marcaViewModel.Status = marcaAtual.Status;

        var marca = _mapper.Map<Models.Marcas>(marcaViewModel);
        await _MarcasService.AtualizarMarca(marca);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Excluir(int id)
    {
        var marca = await _MarcasService.ObterMarcaPorId(id);

        if (marca == null) return NotFound();

        var marcaViewModel = _mapper.Map<MarcasViewModel>(marca);
        return View(marcaViewModel);
    }

    [HttpPost, ActionName("Excluir")]
    public async Task<IActionResult> ConfirmarExclusao(int id)
    {
        await _MarcasService.ExcluirMarca(id);
        return RedirectToAction(nameof(Index));
    }
}
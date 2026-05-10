using Microsoft.AspNetCore.Mvc;
using FabysUnha.Services;
using FabysUnha.ViewModels.Servicos;
using AutoMapper;

namespace FabysUnha.Controllers;

public class ServicosController : Controller
{
    private readonly IServicosService _servicosService;
    private readonly IMapper _mapper;

    public ServicosController(IServicosService servicosService, IMapper mapper)
    {
        _servicosService = servicosService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var servicos = await _servicosService.ObterTodosServicos();
        var viewModel = _mapper.Map<IEnumerable<ServicoListagemViewModel>>(servicos);

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Detalhes(int id)
    {
        var servico = await _servicosService.ObterServicoPorId(id);
        if (servico == null) return NotFound();

        var viewModel = _mapper.Map<ServicoDetalhesViewModel>(servico);
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Criar()
    {
        return View(new ServicoRegistroViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Criar(ServicoRegistroViewModel viewModel)
    {
        if (!ModelState.IsValid) return View(viewModel);

        try
        {
            var servico = _mapper.Map<Models.Servicos>(viewModel);
            await _servicosService.CriarServico(servico);
            
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro ao criar serviço: {ex.Message}");
            return View(viewModel);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var servico = await _servicosService.ObterServicoPorId(id);
        if (servico == null) return NotFound();

        var viewModel = _mapper.Map<ServicoEditarViewModel>(servico);
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(ServicoEditarViewModel viewModel)
    {
        if (!ModelState.IsValid) return View(viewModel);

        try
        {
            var servico = _mapper.Map<Models.Servicos>(viewModel);
            await _servicosService.AtualizarServico(servico);
            
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro ao atualizar serviço: {ex.Message}");
            return View(viewModel);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Excluir(int id)
    {
        var servico = await _servicosService.ObterServicoPorId(id);
        if (servico == null) return NotFound();

        var viewModel = _mapper.Map<ServicoDetalhesViewModel>(servico);
        return View(viewModel);
    }

    [HttpPost, ActionName("Excluir")]
    public async Task<IActionResult> ConfirmarExclusao(int id)
    {
        try
        {
            await _servicosService.ExcluirServico(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro ao excluir serviço: {ex.Message}");
            return RedirectToAction(nameof(Index)); 
        }
    }
}
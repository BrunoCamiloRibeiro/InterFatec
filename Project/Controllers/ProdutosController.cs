using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FabysUnha.Services;
using FabysUnha.ViewModels; 
using AutoMapper;

namespace FabysUnha.Controllers;

public class ProdutosController : Controller
{
    private readonly IProdutosService _produtosService;
    private readonly IMarcasService _marcasService; 
    private readonly IMapper _mapper;

    public ProdutosController(IProdutosService produtosService, IMarcasService marcasService, IMapper mapper)
    {
        _produtosService = produtosService;
        _marcasService = marcasService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var produtos = await _produtosService.ObterTodosProdutos();
        var viewModel = _mapper.Map<IEnumerable<ProdutoListagemViewModel>>(produtos);

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Detalhes(int id)
    {
        var produto = await _produtosService.ObterProdutoPorId(id);
        if (produto == null) return NotFound();

        var viewModel = _mapper.Map<ProdutoDetalhesViewModel>(produto);
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Criar()
    {
        var viewModel = new ProdutoRegistroViewModel
        {
            MarcasList = new SelectList(await _marcasService.ObterTodasMarcas(), "Id", "Nome")
        };
        
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Criar(ProdutoRegistroViewModel viewModel)
    {
        if (!ModelState.IsValid) 
        {
            viewModel.MarcasList = new SelectList(await _marcasService.ObterTodasMarcas(), "Id", "Nome");
            return View(viewModel);
        }

        try
        {
            var produto = _mapper.Map<Models.Produtos>(viewModel);
            await _produtosService.CriarProduto(produto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro ao criar produto: {ex.Message}");
            viewModel.MarcasList = new SelectList(await _marcasService.ObterTodasMarcas(), "Id", "Nome");
            return View(viewModel);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var produto = await _produtosService.ObterProdutoPorId(id);
        if (produto == null) return NotFound();

        var viewModel = _mapper.Map<ProdutoEditarViewModel>(produto);
        
        viewModel.MarcasList = new SelectList(await _marcasService.ObterTodasMarcas(), "Id", "Nome", viewModel.MarcaId);
        
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(ProdutoEditarViewModel viewModel)
    {
        if (!ModelState.IsValid) 
        {
            viewModel.MarcasList = new SelectList(await _marcasService.ObterTodasMarcas(), "Id", "Nome", viewModel.MarcaId);
            return View(viewModel);
        }

        try
        {
            var produtoAtual = await _produtosService.ObterProdutoPorId(viewModel.Codigo);
            if (produtoAtual == null) return NotFound();

            if (!Request.HasFormContentType || !Request.Form.ContainsKey(nameof(viewModel.Status)))
                viewModel.Status = produtoAtual.Status;

            var produto = _mapper.Map<Models.Produtos>(viewModel);
            await _produtosService.AtualizarProduto(produto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro ao atualizar produto: {ex.Message}");
            viewModel.MarcasList = new SelectList(await _marcasService.ObterTodasMarcas(), "Id", "Nome", viewModel.MarcaId);
            return View(viewModel);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Excluir(int id)
    {
        var produto = await _produtosService.ObterProdutoPorId(id);
        if (produto == null) return NotFound();

        var viewModel = _mapper.Map<ProdutoDetalhesViewModel>(produto);
        return View(viewModel);
    }

    [HttpPost, ActionName("Excluir")]
    public async Task<IActionResult> ConfirmarExclusao(int id)
    {
        try
        {
            await _produtosService.ExcluirProduto(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro ao excluir produto: {ex.Message}");
            return RedirectToAction(nameof(Index)); 
        }
    }
}
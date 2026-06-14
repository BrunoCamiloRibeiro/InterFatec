using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using FabysUnha.Services;
using FabysUnha.ViewModels; 
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace FabysUnha.Controllers;

public class ProdutosController : Controller
{
    private readonly IProdutosService _produtosService;
    private readonly IMarcasService _marcasService; 
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;

    public ProdutosController(IProdutosService produtosService, IMarcasService marcasService, IMapper mapper, IWebHostEnvironment env)
    {
        _produtosService = produtosService;
        _marcasService = marcasService;
        _mapper = mapper;
        _env = env;
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
        var marcasAtivas = (await _marcasService.ObterTodasMarcas())
            .Where(m => m.Status == FabysUnha.Enums.MarcaStatus.Ativo)
            .ToList();

        var viewModel = new ProdutoRegistroViewModel
        {
            MarcasList = new SelectList(marcasAtivas, "Id", "Nome")
        };
        
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Criar(ProdutoRegistroViewModel viewModel)
    {
        if (!ModelState.IsValid) 
        {
            var marcasAtivas = (await _marcasService.ObterTodasMarcas())
                .Where(m => m.Status == FabysUnha.Enums.MarcaStatus.Ativo)
                .ToList();
            viewModel.MarcasList = new SelectList(marcasAtivas, "Id", "Nome");
            return View(viewModel);
        }

        try
        {
            var produto = _mapper.Map<Models.Produtos>(viewModel);
            await _produtosService.CriarProduto(produto, viewModel.ImagemUpload);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro ao criar produto: {ex.Message}");
            var marcasAtivas = (await _marcasService.ObterTodasMarcas())
                .Where(m => m.Status == FabysUnha.Enums.MarcaStatus.Ativo)
                .ToList();
            viewModel.MarcasList = new SelectList(marcasAtivas, "Id", "Nome");
            return View(viewModel);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var produto = await _produtosService.ObterProdutoPorId(id);
        if (produto == null) return NotFound();

        var viewModel = _mapper.Map<ProdutoEditarViewModel>(produto);
        
        var marcasAtivasEAtual = (await _marcasService.ObterTodasMarcas())
            .Where(m => m.Status == FabysUnha.Enums.MarcaStatus.Ativo || m.Id == viewModel.MarcaId)
            .ToList();
        
        viewModel.MarcasList = new SelectList(marcasAtivasEAtual, "Id", "Nome", viewModel.MarcaId);
        
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(ProdutoEditarViewModel viewModel)
    {
        if (!ModelState.IsValid) 
        {
            var marcasAtivasEAtual = (await _marcasService.ObterTodasMarcas())
                .Where(m => m.Status == FabysUnha.Enums.MarcaStatus.Ativo || m.Id == viewModel.MarcaId)
                .ToList();
            viewModel.MarcasList = new SelectList(marcasAtivasEAtual, "Id", "Nome", viewModel.MarcaId);
            return View(viewModel);
        }

        try
        {
            bool hasStatusUpdate = Request.HasFormContentType && Request.Form.ContainsKey(nameof(viewModel.Status));
            var produto = _mapper.Map<Models.Produtos>(viewModel);
            
            await _produtosService.AtualizarProduto(produto, viewModel.ImagemUpload, hasStatusUpdate);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro ao atualizar produto: {ex.Message}");
            var marcasAtivasEAtual = (await _marcasService.ObterTodasMarcas())
                .Where(m => m.Status == FabysUnha.Enums.MarcaStatus.Ativo || m.Id == viewModel.MarcaId)
                .ToList();
            viewModel.MarcasList = new SelectList(marcasAtivasEAtual, "Id", "Nome", viewModel.MarcaId);
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
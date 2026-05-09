using Microsoft.AspNetCore.Mvc;
using FabysUnha.Services;
using FabysUnha.ViewModels;
using AutoMapper; 

namespace FabysUnha.Controllers;

public class ClientesController : Controller
{
    private readonly IClientesService _clientesService;
    private readonly IMapper _mapper;

    public ClientesController(IClientesService clientesService, IMapper mapper)
    {
        _clientesService = clientesService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var clientes = await _clientesService.ObterTodosClientes();
        var clientesViewModel = _mapper.Map<IEnumerable<ClienteListagemViewModel>>(clientes);

        return View(clientesViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Detalhes(int id)
    {
        var cliente = await _clientesService.ObterClientePorId(id);
        if (cliente == null) return NotFound();

        var clienteViewModel = _mapper.Map<ClienteDetalhesViewModel>(cliente);
        return View(clienteViewModel);
    }

    [HttpGet]
    public IActionResult Criar()
    {
        return View(new ClienteRegistroViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Criar(ClienteRegistroViewModel clienteViewModel)
    {
        if (!ModelState.IsValid) return View(clienteViewModel);

        try
        {
            var cliente = _mapper.Map<Models.Clientes>(clienteViewModel);
            await _clientesService.RegistrarCliente(cliente);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro ao registrar cliente: {ex.Message}");
            return View(clienteViewModel);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var cliente = await _clientesService.ObterClientePorId(id);
        if (cliente == null) return NotFound();

        var clienteViewModel = _mapper.Map<ClienteEditarViewModel>(cliente);
        return View(clienteViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, ClienteEditarViewModel clienteViewModel)
    {
        if (id != clienteViewModel.Id) return BadRequest();

        if (!ModelState.IsValid) return View(clienteViewModel);

        try
        {
            var cliente = _mapper.Map<Models.Clientes>(clienteViewModel);
            await _clientesService.AtualizarCliente(cliente);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro ao atualizar cliente: {ex.Message}");
            return View(clienteViewModel);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Excluir(int id)
    {
        var cliente = await _clientesService.ObterClientePorId(id);
        if (cliente == null) return NotFound();

        var clienteViewModel = _mapper.Map<ClienteDetalhesViewModel>(cliente);
        return View(clienteViewModel);
    }

    [HttpPost, ActionName("Excluir")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExcluirConfirmado(int id)
    {
        await _clientesService.ExcluirCliente(id);
        return RedirectToAction(nameof(Index));
    }
}
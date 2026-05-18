using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FabysUnha.Enums;
using FabysUnha.Models;
using FabysUnha.Services;
using FabysUnha.ViewModels;

namespace FabysUnha.Controllers;

public class AgendamentosController : Controller
{
    private readonly IAgendamentosService _agendamentosService;
    private readonly IClientesService _clientesService;
    private readonly IFuncionariosService _funcionariosService;
    private readonly IServicosService _servicosService;
    private readonly IProdutosService _produtosService;
    private readonly IMapper _mapper;

    public AgendamentosController(
        IAgendamentosService agendamentosService,
        IClientesService clientesService,
        IFuncionariosService funcionariosService,
        IServicosService servicosService,
        IProdutosService produtosService,
        IMapper mapper)
    {
        _agendamentosService = agendamentosService;
        _clientesService = clientesService;
        _funcionariosService = funcionariosService;
        _servicosService = servicosService;
        _produtosService = produtosService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var agendamentos = await _agendamentosService.ObterTodosAgendamentos();
        var viewModel = _mapper.Map<IEnumerable<AgendamentoListagemViewModel>>(agendamentos);

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Detalhes(int id)
    {
        var agendamento = await _agendamentosService.ObterAgendamentoPorId(id);
        if (agendamento == null) return NotFound();

        var viewModel = _mapper.Map<AgendamentoDetalhesViewModel>(agendamento);
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Criar()
    {
        var viewModel = new AgendamentoRegistroViewModel
        {
            Status = AgendamentoStatus.Pendente
        };

        await PrepararListasAsync(viewModel);
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Criar(AgendamentoRegistroViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            await PrepararListasAsync(viewModel);
            return View(viewModel);
        }

        try
        {
            await _agendamentosService.CriarAgendamento(viewModel);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro ao criar agendamento: {ex.Message}");
            await PrepararListasAsync(viewModel);
            return View(viewModel);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var agendamento = await _agendamentosService.ObterAgendamentoPorId(id);
        if (agendamento == null) return NotFound();

        var viewModel = _mapper.Map<AgendamentoEditarViewModel>(agendamento);
        await PrepararListasAsync(viewModel);
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, AgendamentoEditarViewModel viewModel)
    {
        if (id != viewModel.Nr) return BadRequest();

        if (!ModelState.IsValid)
        {
            await PrepararListasAsync(viewModel);
            return View(viewModel);
        }

        try
        {
            var agendamentoAtual = await _agendamentosService.ObterAgendamentoPorId(id);
            if (agendamentoAtual == null) return NotFound();

            if (!Request.HasFormContentType || !Request.Form.ContainsKey(nameof(viewModel.Status)))
                viewModel.Status = agendamentoAtual.Status;

            await _agendamentosService.AtualizarAgendamento(viewModel);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro ao atualizar agendamento: {ex.Message}");
            await PrepararListasAsync(viewModel);
            return View(viewModel);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Excluir(int id)
    {
        var agendamento = await _agendamentosService.ObterAgendamentoPorId(id);
        if (agendamento == null) return NotFound();

        var viewModel = _mapper.Map<AgendamentoDetalhesViewModel>(agendamento);
        return View(viewModel);
    }

    [HttpPost, ActionName("Excluir")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmarExclusao(int id)
    {
        await _agendamentosService.ExcluirAgendamento(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancelar(int id)
    {
        await _agendamentosService.CancelarAgendamento(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task PrepararListasAsync(AgendamentoRegistroViewModel viewModel)
    {
        var clientes = await _clientesService.ObterTodosClientes();
        var funcionarios = await _funcionariosService.ObterTodosFuncionarios();
        var servicos = await _servicosService.ObterTodosServicos();
        var produtos = await _produtosService.ObterTodosProdutos();

        viewModel.ClientesList = new SelectList(clientes, nameof(Clientes.Id), nameof(Clientes.Nome), viewModel.ClienteId);
        viewModel.FuncionariosList = new SelectList(funcionarios, nameof(Funcionarios.Id), nameof(Funcionarios.Nome));
        viewModel.ServicosList = new SelectList(servicos, nameof(Servicos.Id), nameof(Servicos.Descricao));
        viewModel.ProdutosList = new SelectList(produtos, nameof(Produtos.Codigo), nameof(Produtos.Nome));
    }
}
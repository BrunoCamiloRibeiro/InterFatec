using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
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

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var actionName = context.ActionDescriptor.RouteValues["action"];
        var isClientAction = actionName == "Agendar" || 
                             actionName == "AgendamentoConfirmado" || 
                             actionName == "ObterHorariosDisponiveis" || 
                             actionName == "MeusAgendamentos" || 
                             actionName == "Logout";
                             
        var tipoUsuario = HttpContext.Session.GetString("UsuarioTipo");

        if (string.IsNullOrEmpty(tipoUsuario))
        {
            context.Result = new RedirectToActionResult("Index", "Login", null);
        }
        else if (tipoUsuario == "Cliente" && !isClientAction)
        {
            context.Result = new RedirectToActionResult("MeusAgendamentos", "Agendamentos", null);
        }
        else if (tipoUsuario == "Funcionario" && (actionName == "Agendar" || actionName == "AgendamentoConfirmado" || actionName == "MeusAgendamentos"))
        {
            context.Result = new RedirectToActionResult("Index", "Agendamentos", null);
        }

        base.OnActionExecuting(context);
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
    public async Task<IActionResult> Editar(int id)
    {
        var agendamento = await _agendamentosService.ObterAgendamentoPorId(id);
        if (agendamento == null) return NotFound();

        var viewModel = _mapper.Map<AgendamentoEditarViewModel>(agendamento);
        await PrepararListasAsync(viewModel);
        return View(viewModel);
    }

    [HttpPost]

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

    public async Task<IActionResult> ConfirmarExclusao(int id)
    {
        await _agendamentosService.ExcluirAgendamento(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]

    public async Task<IActionResult> Cancelar(int id)
    {
        await _agendamentosService.CancelarAgendamento(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]

    public async Task<IActionResult> Finalizar(int id)
    {
        await _agendamentosService.FinalizarAgendamento(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task PrepararListasAsync(AgendamentoRegistroViewModel viewModel)
    {
        var clientes = await _clientesService.ObterTodosClientes();
        var funcionarios = await _funcionariosService.ObterTodosFuncionarios();
        var servicos = await _servicosService.ObterTodosServicos();
        var produtos = await _produtosService.ObterTodosProdutos();

        var funcionariosSelecionados = viewModel.ServicosSelecionados?.Select(s => s.FuncionarioId).ToList() ?? new List<int>();
        var servicosSelecionados = viewModel.ServicosSelecionados?.Select(s => s.ServicoId).ToList() ?? new List<int>();
        var produtosSelecionados = viewModel.ProdutosSelecionados?.Select(p => p.ProdutoCodigo).ToList() ?? new List<int>();

        var clientesAtivos = clientes.Where(c => c.Status == PessoaStatus.Ativo || c.Id == viewModel.ClienteId).ToList();
        var funcionariosAtivos = funcionarios.Where(f => f.Status == PessoaStatus.Ativo || funcionariosSelecionados.Contains(f.Id)).ToList();
        var servicosAtivos = servicos.Where(s => s.Status == ServicoStatus.Ativo || servicosSelecionados.Contains(s.Id)).ToList();
        var produtosAtivos = produtos.Where(p => p.Status == ProdutoStatus.Ativo || produtosSelecionados.Contains(p.Codigo)).ToList();

        viewModel.ClientesList = new SelectList(clientesAtivos, nameof(Clientes.Id), nameof(Clientes.Nome), viewModel.ClienteId);
        viewModel.FuncionariosList = new SelectList(funcionariosAtivos, nameof(Funcionarios.Id), nameof(Funcionarios.Nome));
        viewModel.ServicosList = new SelectList(servicosAtivos, nameof(Servicos.Id), nameof(Servicos.Descricao));
        viewModel.ProdutosList = new SelectList(produtosAtivos, nameof(Produtos.Codigo), nameof(Produtos.Nome));
    }

    // ==========================================
    // Versão do CLIENTE
    // ==========================================

    [HttpGet]
    public async Task<IActionResult> Agendar()
    {
        var viewModel = new AgendamentoClienteViewModel();
        await PrepararListasClienteAsync(viewModel);
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Agendar(AgendamentoClienteViewModel viewModel)
    {
        // Remove validações dos campos que não usaremos mais
        ModelState.Remove("Nome");
        ModelState.Remove("Telefone");

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            System.IO.File.WriteAllText("modelstate_errors.txt", string.Join("\n", errors));
            
            await PrepararListasClienteAsync(viewModel);
            return View(viewModel);
        }

        System.IO.File.WriteAllText("post_debug.txt", System.Text.Json.JsonSerializer.Serialize(viewModel));


        var clienteId = HttpContext.Session.GetInt32("ClienteId");
        if (clienteId == null)
            return RedirectToAction("Index", "Login");

        try
        {
            await _agendamentosService.CriarAgendamentoCliente(viewModel, clienteId.Value);
            TempData["Sucesso"] = "Agendamento realizado com sucesso!";
            return RedirectToAction(nameof(AgendamentoConfirmado));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro ao realizar agendamento: {ex.Message}");
            await PrepararListasClienteAsync(viewModel);
            return View(viewModel);
        }
    }

    [HttpGet]
    public IActionResult AgendamentoConfirmado()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ObterHorariosDisponiveis(int funcionarioId, string data)
    {
        if (!DateTime.TryParse(data, out var dataParsed))
            return Json(new List<string>());

        var horarios = await _agendamentosService.ObterHorariosDisponiveis(funcionarioId, dataParsed);
        var resultado = horarios.Select(h => new { valor = h.ToString(@"hh\:mm\:ss"), texto = h.ToString(@"hh\:mm") });
        return Json(resultado);
    }

    private async Task PrepararListasClienteAsync(AgendamentoClienteViewModel viewModel)
    {
        var funcionarios = await _funcionariosService.ObterTodosFuncionarios();
        var servicos = await _servicosService.ObterTodosServicos();
        var produtos = await _produtosService.ObterTodosProdutos();

        var funcionariosSelecionados = viewModel.Servicos?.Select(s => s.FuncionarioId).ToList() ?? new List<int>();
        var servicosSelecionados = viewModel.Servicos?.Select(s => s.ServicoId).ToList() ?? new List<int>();
        
        var funcionariosAtivos = funcionarios.Where(f => f.Status == PessoaStatus.Ativo || funcionariosSelecionados.Contains(f.Id)).ToList();
        var servicosAtivos = servicos.Where(s => s.Status == ServicoStatus.Ativo || servicosSelecionados.Contains(s.Id)).ToList();
        var produtosAtivos = produtos.Where(p => p.Status == ProdutoStatus.Ativo).ToList();

        viewModel.FuncionariosList = new SelectList(funcionariosAtivos, nameof(Funcionarios.Id), nameof(Funcionarios.Nome));
        
        var servicosItens = servicosAtivos.Select(s => new {
            Id = s.Id,
            Descricao = s.Descricao,
            Preco = s.Preco,
            TextoFormatado = $"{s.Descricao} - R$ {s.Preco:F2}"
        }).ToList();
        viewModel.ServicosList = new SelectList(servicosItens, "Id", "TextoFormatado");

        var produtosItens = produtosAtivos.Select(p => new {
            Codigo = p.Codigo,
            Nome = p.Nome,
            Preco = p.Preco,
            PathImagem = p.PathImagem,
            TextoFormatado = $"{p.Nome} - R$ {p.Preco:F2}"
        }).ToList();
        viewModel.ProdutosList = new SelectList(produtosItens, "Codigo", "TextoFormatado");

        ViewBag.ProdutosJson = System.Text.Json.JsonSerializer.Serialize(produtosItens);
        ViewBag.ServicosJson = System.Text.Json.JsonSerializer.Serialize(servicosItens);
    }

    /// <summary>
    /// Exibe os agendamentos do cliente logado via Session
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> MeusAgendamentos()
    {
        var clienteId = HttpContext.Session.GetInt32("ClienteId");
        var clienteTelefone = HttpContext.Session.GetString("ClienteTelefone");

        var tipoUsuario = HttpContext.Session.GetString("UsuarioTipo");

        if (tipoUsuario == "Funcionario")
            return RedirectToAction("Index", "Agendamentos");

        if (!clienteId.HasValue || string.IsNullOrEmpty(clienteTelefone))
            return RedirectToAction("Index", "Login");

        var agendamentos = await _agendamentosService.ObterAgendamentosPorCliente(clienteId.Value);
        var viewModel = _mapper.Map<IEnumerable<AgendamentoListagemViewModel>>(agendamentos);

        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
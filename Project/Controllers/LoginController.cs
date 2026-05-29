using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FabysUnha.Enums;
using FabysUnha.Models;
using FabysUnha.Services;
using FabysUnha.Services.Interfaces;
using FabysUnha.ViewModels;

namespace FabysUnha.Controllers;

public class LoginController : Controller
{
    private readonly IClienteAuthService _clienteAuthService;
    private readonly IFuncionariosService _funcionariosService;
    private readonly IClientesService _clientesService;
    private readonly IEspecialidadeService _especialidadeService;

    public LoginController(
        IClienteAuthService clienteAuthService,
        IFuncionariosService funcionariosService,
        IClientesService clientesService,
        IEspecialidadeService especialidadeService)
    {
        _clienteAuthService = clienteAuthService;
        _funcionariosService = funcionariosService;
        _clientesService = clientesService;
        _especialidadeService = especialidadeService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var viewModel = new LoginViewModel();
        return View(viewModel);
    }

    private async Task<IEnumerable<SelectListItem>> ObterEspecialidadesAsync()
    {
        var especialidades = await _especialidadeService.ObterTodasEspecialidades();
        return especialidades
            .Select(e => new SelectListItem(e.Descricao, e.Id.ToString()))
            .ToList();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegistrarUsuario(LoginViewModel model)
    {

        if (string.IsNullOrWhiteSpace(model.CadastroNome) || string.IsNullOrWhiteSpace(model.CadastroTelefone) || string.IsNullOrWhiteSpace(model.CadastroSenha))
        {
            ModelState.AddModelError(string.Empty, "Nome, telefone e senha são obrigatórios para cadastro.");
            ViewBag.ActiveTab = "cadastrar";
            return View("Index", model);
        }

        if (model.CadastroSenha != model.CadastroConfirmacaoSenha)
        {
            ModelState.AddModelError(string.Empty, "A confirmação da senha não confere.");
            ViewBag.ActiveTab = "cadastrar";
            return View("Index", model);
        }

        if (model.CadastroTipo == "funcionario")
        {
            var funcionario = new Funcionarios
            {
                Nome = model.CadastroNome.Trim(),
                Telefone = model.CadastroTelefone.Trim(),
                Senha = model.CadastroSenha,
                Status = PessoaStatus.Ativo
            };

            await _funcionariosService.RegistrarFuncionario(funcionario);
            TempData["RegistroSucesso"] = "Cadastro de funcionário realizado com sucesso!";
            return RedirectToAction("Index");
        }

        var clienteExistente = await _clientesService.ObterClientePorTelefone(model.CadastroTelefone.Trim());
        if (clienteExistente != null)
        {
            ModelState.AddModelError(string.Empty, "Este telefone já está cadastrado.");
            ViewBag.ActiveTab = "cadastrar";
            return View("Index", model);
        }

        var cliente = new Clientes
        {
            Nome = model.CadastroNome.Trim(),
            Telefone = model.CadastroTelefone.Trim(),
            Senha = model.CadastroSenha,
            Status = PessoaStatus.Ativo
        };

        await _clientesService.RegistrarCliente(cliente);
        TempData["RegistroSucesso"] = "Cadastro de cliente realizado com sucesso! Faça login para continuar.";
        return RedirectToAction("Index");
    }

    /// <summary>
    /// Login de Cliente: Telefone + Senha
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ClienteLogin(string telefone, string senha)
    {
        if (string.IsNullOrWhiteSpace(telefone) || string.IsNullOrWhiteSpace(senha))
        {
            ModelState.AddModelError(string.Empty, "Telefone e senha são obrigatórios.");
            return View("Index", new LoginViewModel());
        }

        var (valido, cliente, agendamentos) = await _clienteAuthService
            .AutenticarClientePorTelefoneESenha(telefone, senha);

        if (!valido)
        {
            ModelState.AddModelError(string.Empty, "Telefone ou senha inválidos.");
            return View("Index", new LoginViewModel());
        }

        // Salvar na Session
        HttpContext.Session.SetInt32("ClienteId", cliente!.Id);
        HttpContext.Session.SetString("ClienteTelefone", cliente.Telefone);
        HttpContext.Session.SetString("ClienteNome", cliente.Nome);
        HttpContext.Session.SetString("UsuarioTipo", "Cliente");

        // Redirecionar para página de agendamentos do cliente
        return RedirectToAction("MeusAgendamentos", "Agendamentos");
    }

    /// <summary>
    /// Login de Funcionário: Email/Usuário + Senha
    /// (Implementação simplificada - você pode integrar com Identity depois)
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FuncionarioLogin(string telefone, string senha)
    {
        if (string.IsNullOrWhiteSpace(telefone) || string.IsNullOrWhiteSpace(senha))
        {
            ModelState.AddModelError(string.Empty, "Telefone e senha são obrigatórios.");
            ViewBag.ActiveTab = "funcionario";
            return View("Index", new LoginViewModel());
        }

        var funcionarios = await _funcionariosService.ObterTodosFuncionarios();
        var funcionario = funcionarios
            .FirstOrDefault(f => f.Telefone == telefone && f.Senha == senha);

        if (funcionario == null)
        {
            ModelState.AddModelError(string.Empty, "Telefone ou senha inválidos.");
            ViewBag.ActiveTab = "funcionario";
            return View("Index", new LoginViewModel());
        }

        HttpContext.Session.SetInt32("FuncionarioId", funcionario.Id);
        HttpContext.Session.SetString("FuncionarioNome", funcionario.Nome);
        HttpContext.Session.SetString("FuncionarioTelefone", funcionario.Telefone);
        HttpContext.Session.SetString("UsuarioTipo", "Funcionario");

        return RedirectToAction("Index", "Funcionarios");
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
}

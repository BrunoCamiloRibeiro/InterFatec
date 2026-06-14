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
    private readonly ILoginAuthService _loginAuthService;
    private readonly IFuncionariosService _funcionariosService;
    private readonly IClientesService _clientesService;

    public LoginController(
        ILoginAuthService loginAuthService,
        IFuncionariosService funcionariosService,
        IClientesService clientesService)
    {
        _loginAuthService = loginAuthService;
        _funcionariosService = funcionariosService;
        _clientesService = clientesService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var viewModel = new LoginViewModel();
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Cadastro()
    {
        var viewModel = new LoginViewModel();
        return View(viewModel);
    }



    [HttpPost]

    public async Task<IActionResult> RegistrarUsuario(LoginViewModel model)
    {

        if (string.IsNullOrWhiteSpace(model.CadastroNome) || string.IsNullOrWhiteSpace(model.CadastroTelefone) || string.IsNullOrWhiteSpace(model.CadastroSenha))
        {
            ModelState.AddModelError(string.Empty, "Nome, telefone e senha são obrigatórios para cadastro.");
            return View("Cadastro", model);
        }

        if (model.CadastroSenha.Length < 6)
        {
            ModelState.AddModelError(string.Empty, "A senha deve ter no mínimo 6 dígitos.");
            return View("Cadastro", model);
        }

        if (model.CadastroSenha != model.CadastroConfirmacaoSenha)
        {
            ModelState.AddModelError(string.Empty, "A confirmação da senha não confere.");
            return View("Cadastro", model);
        }

        if (model.CadastroTipo == TipoUsuario.Funcionario)
        {
            var telefoneLimpo = new string(model.CadastroTelefone.Where(char.IsDigit).ToArray());

            var funcionario = new Funcionarios
            {
                Nome = model.CadastroNome.Trim(),
                Telefone = telefoneLimpo,
                Senha = model.CadastroSenha,
                Status = PessoaStatus.Ativo,
                Salario = 1412.00M,
                EspecialidadeId = null
            };

            await _funcionariosService.RegistrarFuncionario(funcionario);
            TempData["RegistroSucesso"] = "Cadastro de funcionário realizado com sucesso!";
            return RedirectToAction("Index");
        }

        var telefoneLimpoCliente = new string(model.CadastroTelefone.Where(char.IsDigit).ToArray());

        var clienteExistente = await _clientesService.ObterClientePorTelefone(telefoneLimpoCliente);
        if (clienteExistente != null)
        {
            ModelState.AddModelError(string.Empty, "Este telefone já está cadastrado.");
            return View("Cadastro", model);
        }

        var cliente = new Clientes
        {
            Nome = model.CadastroNome.Trim(),
            Telefone = telefoneLimpoCliente,
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

    public async Task<IActionResult> ClienteLogin(string telefone, string senha)
    {
        if (string.IsNullOrWhiteSpace(telefone) || string.IsNullOrWhiteSpace(senha))
        {
            ModelState.AddModelError(string.Empty, "Telefone e senha são obrigatórios.");
            return View("Index", new LoginViewModel());
        }

        var telefoneLimpo = new string(telefone.Where(char.IsDigit).ToArray());

        var (valido, cliente, agendamentos) = await _loginAuthService
            .AutenticarClientePorTelefoneESenha(telefoneLimpo, senha);

        if (!valido)
        {
            ModelState.AddModelError(string.Empty, "Telefone ou senha inválidos.");
            return View("Index", new LoginViewModel());
        }

        // Salvar na Session
        HttpContext.Session.SetInt32("ClienteId", cliente!.Id);
        HttpContext.Session.SetString("ClienteTelefone", cliente.Telefone);
        HttpContext.Session.SetString("ClienteNome", cliente.Nome);
        HttpContext.Session.SetString("UsuarioTipo", nameof(TipoUsuario.Cliente));

        // Redirecionar para página de agendamentos do cliente
        return RedirectToAction("MeusAgendamentos", "Agendamentos");
    }

    /// <summary>
    /// Login de Funcionário: Email/Usuário + Senha
    /// (Implementação simplificada - você pode integrar com Identity depois)
    /// </summary>
    [HttpPost]

    public async Task<IActionResult> FuncionarioLogin(string telefone, string senha)
    {
        if (string.IsNullOrWhiteSpace(telefone) || string.IsNullOrWhiteSpace(senha))
        {
            ModelState.AddModelError(string.Empty, "Telefone e senha são obrigatórios.");
            ViewBag.ActiveTab = "funcionario";
            return View("Index", new LoginViewModel());
        }

        var telefoneLimpo = new string(telefone.Where(char.IsDigit).ToArray());

        var funcionario = await _loginAuthService.AutenticarFuncionario(telefoneLimpo, senha);

        if (funcionario == null)
        {
            ModelState.AddModelError(string.Empty, "Telefone ou senha inválidos.");
            ViewBag.ActiveTab = "funcionario";
            return View("Index", new LoginViewModel());
        }

        HttpContext.Session.SetInt32("FuncionarioId", funcionario.Id);
        HttpContext.Session.SetString("FuncionarioNome", funcionario.Nome);
        HttpContext.Session.SetString("FuncionarioTelefone", funcionario.Telefone);
        HttpContext.Session.SetString("UsuarioTipo", nameof(TipoUsuario.Funcionario));

        return RedirectToAction("Index", "Funcionarios");
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
}

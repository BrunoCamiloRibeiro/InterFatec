using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FabysUnha.Enums;
using FabysUnha.Models;
using FabysUnha.Services;
using FabysUnha.Services.Interfaces;
using FabysUnha.ViewModels;

namespace FabysUnha.Controllers;

/// <summary>
/// Controlador responsável pela autenticação e registro de usuários (Clientes e Funcionários).
/// Ele gerencia o processo de login, cadastro de novos usuários e encerramento de sessão (logout).
/// </summary>
public class LoginController : Controller
{
    // Injeção de dependência dos serviços necessários.
    // Usamos readonly para garantir que os serviços não sejam alterados após a construção do controlador.
    private readonly ILoginAuthService _loginAuthService;
    private readonly IFuncionariosService _funcionariosService;
    private readonly IClientesService _clientesService;

    /// <summary>
    /// Construtor do LoginController.
    /// Recebe as dependências injetadas pelo contêiner de Injeção de Dependência (DI) do ASP.NET Core.
    /// </summary>
    /// <param name="loginAuthService">Serviço de autenticação.</param>
    /// <param name="funcionariosService">Serviço de gerenciamento de funcionários.</param>
    /// <param name="clientesService">Serviço de gerenciamento de clientes.</param>
    public LoginController(
        ILoginAuthService loginAuthService,
        IFuncionariosService funcionariosService,
        IClientesService clientesService)
    {
        // Atribui as instâncias injetadas às variáveis privadas para uso nas ações (métodos).
        _loginAuthService = loginAuthService;
        _funcionariosService = funcionariosService;
        _clientesService = clientesService;
    }

    /// <summary>
    /// Exibe a página principal de Login.
    /// Responde a requisições HTTP GET.
    /// </summary>
    /// <returns>Retorna a View contendo o formulário de login.</returns>
    [HttpGet]
    public IActionResult Index()
    {
        // Instancia um ViewModel vazio para ser preenchido pela View.
        var viewModel = new LoginViewModel();
        return View(viewModel);
    }

    /// <summary>
    /// Exibe a página de Cadastro de novos usuários.
    /// Responde a requisições HTTP GET.
    /// </summary>
    /// <returns>Retorna a View contendo o formulário de cadastro.</returns>
    [HttpGet]
    public IActionResult Cadastro()
    {
        // Prepara um ViewModel para a tela de cadastro.
        var viewModel = new LoginViewModel();
        return View(viewModel);
    }

    /// <summary>
    /// Processa a submissão do formulário de cadastro de um novo usuário.
    /// Diferencia o cadastro entre 'Funcionario' e 'Cliente' de acordo com o tipo escolhido.
    /// </summary>
    /// <param name="model">Os dados de cadastro submetidos pelo usuário na View (encapsulados em LoginViewModel).</param>
    /// <returns>Redireciona para o login em caso de sucesso, ou retorna à View de cadastro em caso de erro de validação.</returns>
    [HttpPost]
    public async Task<IActionResult> RegistrarUsuario(LoginViewModel model)
    {
        // Verifica se os campos essenciais foram preenchidos. Caso contrário, adiciona um erro ao ModelState.
        if (string.IsNullOrWhiteSpace(model.CadastroNome) || string.IsNullOrWhiteSpace(model.CadastroTelefone) || string.IsNullOrWhiteSpace(model.CadastroSenha))
        {
            ModelState.AddModelError(string.Empty, "Nome, telefone e senha são obrigatórios para cadastro.");
            // Retorna a view de cadastro exibindo a mensagem de erro e mantendo os dados preenchidos pelo usuário.
            return View("Cadastro", model);
        }

        // Validação de segurança básica: a senha deve possuir pelo menos 6 caracteres.
        if (model.CadastroSenha.Length < 6)
        {
            ModelState.AddModelError(string.Empty, "A senha deve ter no mínimo 6 dígitos.");
            return View("Cadastro", model);
        }

        // Verifica se a senha e a confirmação de senha coincidem.
        if (model.CadastroSenha != model.CadastroConfirmacaoSenha)
        {
            ModelState.AddModelError(string.Empty, "A confirmação da senha não confere.");
            return View("Cadastro", model);
        }

        // Fluxo de cadastro caso o usuário selecionado seja do tipo 'Funcionario'
        if (model.CadastroTipo == TipoUsuario.Funcionario)
        {
            // Remove qualquer caractere não numérico do telefone (ex: parênteses, traços).
            var telefoneLimpo = new string(model.CadastroTelefone.Where(char.IsDigit).ToArray());

            // Cria um novo objeto Funcionario com os dados informados.
            var funcionario = new Funcionarios
            {
                Nome = model.CadastroNome.Trim(), // Remove espaços em branco nas extremidades do nome
                Telefone = telefoneLimpo,
                Senha = model.CadastroSenha,
                Status = PessoaStatus.Ativo, // Define o funcionário como ativo por padrão
                Salario = 1412.00M, // Define um salário base padrão
                EspecialidadeId = null // Sem especialidade atribuída inicialmente
            };

            // Salva o funcionário no banco de dados através do serviço de funcionários.
            await _funcionariosService.RegistrarFuncionario(funcionario);
            
            // Usa o TempData para passar uma mensagem de sucesso para a próxima requisição (ex: exibir um alerta na tela de login).
            TempData["RegistroSucesso"] = "Cadastro de funcionário realizado com sucesso!";
            return RedirectToAction("Index"); // Redireciona para a tela inicial de Login
        }

        // Fluxo de cadastro caso o usuário selecionado seja do tipo 'Cliente'
        
        // Remove formatação não numérica do telefone do cliente.
        var telefoneLimpoCliente = new string(model.CadastroTelefone.Where(char.IsDigit).ToArray());

        // Verifica se já existe um cliente com este mesmo telefone no banco de dados.
        // A regra de negócio não permite dois clientes com o mesmo telefone de contato/login.
        var clienteExistente = await _clientesService.ObterClientePorTelefone(telefoneLimpoCliente);
        if (clienteExistente != null)
        {
            // Se existir, informa o erro e não permite o cadastro duplicado.
            ModelState.AddModelError(string.Empty, "Este telefone já está cadastrado.");
            return View("Cadastro", model);
        }

        // Cria a entidade Cliente com os dados validados.
        var cliente = new Clientes
        {
            Nome = model.CadastroNome.Trim(),
            Telefone = telefoneLimpoCliente,
            Senha = model.CadastroSenha,
            Status = PessoaStatus.Ativo // O cliente nasce ativo no sistema.
        };

        // Salva o cliente no banco de dados usando o serviço correspondente.
        await _clientesService.RegistrarCliente(cliente);
        
        // Notifica o usuário do sucesso do cadastro usando TempData.
        TempData["RegistroSucesso"] = "Cadastro de cliente realizado com sucesso! Faça login para continuar.";
        
        // Redireciona para a tela de Login.
        return RedirectToAction("Index");
    }

    /// <summary>
    /// Ação que processa o Login exclusivo de um Cliente utilizando Telefone e Senha.
    /// </summary>
    /// <param name="telefone">Telefone informado no formulário de login.</param>
    /// <param name="senha">Senha informada no formulário de login.</param>
    /// <returns>Redireciona para a área logada em caso de sucesso, ou volta ao login em caso de falha.</returns>
    [HttpPost]
    public async Task<IActionResult> ClienteLogin(string telefone, string senha)
    {
        // Validação inicial para garantir que os campos não estão vazios.
        if (string.IsNullOrWhiteSpace(telefone) || string.IsNullOrWhiteSpace(senha))
        {
            ModelState.AddModelError(string.Empty, "Telefone e senha são obrigatórios.");
            return View("Index", new LoginViewModel());
        }

        // Limpa o telefone retirando a formatação visual, deixando apenas os números para buscar no BD.
        var telefoneLimpo = new string(telefone.Where(char.IsDigit).ToArray());

        // Chama o serviço de autenticação, que retorna uma tupla (Tuple) indicando:
        // 1. Se é válido (bool)
        // 2. A entidade do cliente (se encontrado e autenticado)
        // 3. A lista de agendamentos associados (informação extra, se necessário)
        var (valido, cliente, agendamentos) = await _loginAuthService
            .AutenticarClientePorTelefoneESenha(telefoneLimpo, senha);

        // Se as credenciais estiverem incorretas, exibe o erro na mesma View de login.
        if (!valido)
        {
            ModelState.AddModelError(string.Empty, "Telefone ou senha inválidos.");
            return View("Index", new LoginViewModel());
        }

        // Em caso de sucesso, grava os dados principais do cliente na Sessão (Session) do ASP.NET.
        // Isso manterá o cliente "logado" nas requisições seguintes, como uma credencial temporária.
        HttpContext.Session.SetInt32("ClienteId", cliente!.Id);
        HttpContext.Session.SetString("ClienteTelefone", cliente.Telefone);
        HttpContext.Session.SetString("ClienteNome", cliente.Nome);
        
        // Armazena qual o tipo de usuário está logado. Esta chave é usada, por exemplo,
        // pelo HomeController e filtros de autorização para redirecionamentos baseados em perfil.
        HttpContext.Session.SetString("UsuarioTipo", nameof(TipoUsuario.Cliente));

        // Por fim, redireciona o cliente recém-autenticado para a página de visualização de seus agendamentos.
        return RedirectToAction("MeusAgendamentos", "Agendamentos");
    }

    /// <summary>
    /// Ação que processa o Login exclusivo de um Funcionário utilizando Telefone e Senha.
    /// (Esta implementação é simples, focada em aprendizado. Sistemas reais costumam usar ASP.NET Core Identity ou JWT).
    /// </summary>
    /// <param name="telefone">Telefone informado no formulário de login.</param>
    /// <param name="senha">Senha informada.</param>
    /// <returns>Redireciona para o painel de controle (Index de Funcionarios) em caso de sucesso.</returns>
    [HttpPost]
    public async Task<IActionResult> FuncionarioLogin(string telefone, string senha)
    {
        // Verifica preenchimento dos campos obrigatórios.
        if (string.IsNullOrWhiteSpace(telefone) || string.IsNullOrWhiteSpace(senha))
        {
            ModelState.AddModelError(string.Empty, "Telefone e senha são obrigatórios.");
            // ViewBag.ActiveTab ajuda a interface (View) a manter a aba 'funcionario' ativa
            // caso a página tenha divs divididas por tipo de login (tabs).
            ViewBag.ActiveTab = "funcionario";
            return View("Index", new LoginViewModel());
        }

        // Remove caracteres não numéricos.
        var telefoneLimpo = new string(telefone.Where(char.IsDigit).ToArray());

        // Realiza a tentativa de autenticação via serviço de autenticação específico para funcionários.
        var funcionario = await _loginAuthService.AutenticarFuncionario(telefoneLimpo, senha);

        // Se o objeto funcionario retornado for nulo, a autenticação falhou (usuário não existe ou senha incorreta).
        if (funcionario == null)
        {
            ModelState.AddModelError(string.Empty, "Telefone ou senha inválidos.");
            ViewBag.ActiveTab = "funcionario"; // Mantém a aba correta focada na interface para nova tentativa.
            return View("Index", new LoginViewModel());
        }

        // Salva as credenciais básicas e a identificação do funcionário logado na Sessão.
        HttpContext.Session.SetInt32("FuncionarioId", funcionario.Id);
        HttpContext.Session.SetString("FuncionarioNome", funcionario.Nome);
        HttpContext.Session.SetString("FuncionarioTelefone", funcionario.Telefone);
        // Define o tipo de usuário como 'Funcionario' para o controle de acesso e redirecionamento.
        HttpContext.Session.SetString("UsuarioTipo", nameof(TipoUsuario.Funcionario));

        // Redireciona o funcionário para a sua tela principal do sistema.
        return RedirectToAction("Index", "Funcionarios");
    }

    /// <summary>
    /// Realiza o "Logout" do usuário no sistema.
    /// Limpa a sessão, apagando as informações e esquecendo assim quem estava logado.
    /// </summary>
    /// <returns>Redireciona para a tela inicial de Login.</returns>
    [HttpGet]
    public IActionResult Logout()
    {
        // Remove todos os valores armazenados na Session atual, encerrando efetivamente a autenticação.
        HttpContext.Session.Clear();
        
        // Retorna para a tela de login.
        return RedirectToAction("Index");
    }
}

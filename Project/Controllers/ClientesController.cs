using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using FabysUnha.Services;
using FabysUnha.ViewModels;
using AutoMapper; 

namespace FabysUnha.Controllers;

/// <summary>
/// Controlador responsável por gerenciar o cadastro e manutenção de Clientes.
/// </summary>
public class ClientesController : Controller
{
    private readonly IClientesService _clientesService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Construtor que recebe as dependências necessárias.
    /// </summary>
    /// <param name="clientesService">Serviço contendo regras de negócio para Clientes</param>
    /// <param name="mapper">Ferramenta para conversão entre Models e ViewModels</param>
    public ClientesController(IClientesService clientesService, IMapper mapper)
    {
        _clientesService = clientesService;
        _mapper = mapper;
    }

    /// <summary>
    /// Intercepta a requisição antes de executar qualquer action.
    /// Usado aqui para verificar permissões de acesso.
    /// </summary>
    /// <param name="context">Contexto da requisição</param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Verifica se quem está acessando tem perfil de "Funcionario"
        var tipoUsuario = HttpContext.Session.GetString("UsuarioTipo");
        if (tipoUsuario != "Funcionario")
        {
            // Bloqueia o acesso e redireciona para o login
            context.Result = new RedirectToActionResult("Index", "Login", null);
        }
        base.OnActionExecuting(context);
    }

    /// <summary>
    /// Lista todos os clientes cadastrados no sistema.
    /// </summary>
    /// <returns>View de listagem com uma coleção de ClienteListagemViewModel</returns>
    public async Task<IActionResult> Index()
    {
        // Consulta todos os clientes via serviço
        var clientes = await _clientesService.ObterTodosClientes();
        
        // Mapeia o resultado para o formato esperado pela view
        var clientesViewModel = _mapper.Map<IEnumerable<ClienteListagemViewModel>>(clientes);

        return View(clientesViewModel);
    }

    /// <summary>
    /// Exibe os detalhes de um cliente específico.
    /// </summary>
    /// <param name="id">ID do cliente</param>
    /// <returns>View de Detalhes</returns>
    [HttpGet]
    public async Task<IActionResult> Detalhes(int id)
    {
        var cliente = await _clientesService.ObterClientePorId(id);
        if (cliente == null) return NotFound();

        var clienteViewModel = _mapper.Map<ClienteDetalhesViewModel>(cliente);
        return View(clienteViewModel);
    }

    /// <summary>
    /// Exibe o formulário vazio para criação de um novo cliente.
    /// </summary>
    /// <returns>View de Criação (Registro)</returns>
    [HttpGet]
    public IActionResult Criar()
    {
        return View(new ClienteRegistroViewModel());
    }

    /// <summary>
    /// Recebe os dados do formulário para salvar um novo cliente.
    /// </summary>
    /// <param name="clienteViewModel">Os dados preenchidos no form</param>
    /// <returns>Redireciona para listagem se sucesso, ou recarrega form se houver erros</returns>
    [HttpPost]
    public async Task<IActionResult> Criar(ClienteRegistroViewModel clienteViewModel)
    {
        // Verifica se as anotações DataAnnotations (Required, MaxLength) foram respeitadas
        if (!ModelState.IsValid) return View(clienteViewModel);

        try
        {
            // Mapeia ViewModel de volta para o Model (Entidade)
            var cliente = _mapper.Map<Models.Clientes>(clienteViewModel);
            
            // Salva no banco de dados usando o serviço
            await _clientesService.RegistrarCliente(cliente);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Captura possíveis exceções (como e-mail ou telefone duplicado)
            ModelState.AddModelError(string.Empty, $"Erro ao registrar cliente: {ex.Message}");
            return View(clienteViewModel);
        }
    }

    /// <summary>
    /// Carrega os dados de um cliente no formulário de edição.
    /// </summary>
    /// <param name="id">ID do cliente a ser editado</param>
    /// <returns>View de Edição</returns>
    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var cliente = await _clientesService.ObterClientePorId(id);
        if (cliente == null) return NotFound();

        var clienteViewModel = _mapper.Map<ClienteEditarViewModel>(cliente);
        return View(clienteViewModel);
    }

    /// <summary>
    /// Recebe as modificações e atualiza o cadastro do cliente.
    /// </summary>
    /// <param name="id">ID do cliente vindo na URL</param>
    /// <param name="clienteViewModel">Dados enviados do formulário</param>
    /// <returns>Redireciona para listagem em caso de sucesso</returns>
    [HttpPost]
    public async Task<IActionResult> Editar(int id, ClienteEditarViewModel clienteViewModel)
    {
        // Proteção para garantir que o formulário pertence ao registro correto
        if (id != clienteViewModel.Id) return BadRequest();

        if (!ModelState.IsValid) return View(clienteViewModel);

        try
        {
            // Recupera o estado atual no banco
            var clienteAtual = await _clientesService.ObterClientePorId(id);
            if (clienteAtual == null) return NotFound();

            // Previne falhas se o Status não for enviado no form
            if (!Request.HasFormContentType || !Request.Form.ContainsKey(nameof(clienteViewModel.Status)))
                clienteViewModel.Status = clienteAtual.Status;

            // Mapeia dados e envia para atualização
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

    /// <summary>
    /// Exibe uma página para confirmar a intenção de exclusão do cliente.
    /// </summary>
    /// <param name="id">ID do cliente</param>
    /// <returns>View com detalhes para confirmar a ação</returns>
    [HttpGet]
    public async Task<IActionResult> Excluir(int id)
    {
        var cliente = await _clientesService.ObterClientePorId(id);
        if (cliente == null) return NotFound();

        var clienteViewModel = _mapper.Map<ClienteDetalhesViewModel>(cliente);
        return View(clienteViewModel);
    }

    /// <summary>
    /// Ação final que deleta o cliente após confirmação do usuário.
    /// </summary>
    /// <param name="id">ID do cliente</param>
    /// <returns>Redireciona para a listagem principal</returns>
    [HttpPost, ActionName("Excluir")]
    public async Task<IActionResult> ExcluirConfirmado(int id)
    {
        await _clientesService.ExcluirCliente(id);
        return RedirectToAction(nameof(Index));
    }
}
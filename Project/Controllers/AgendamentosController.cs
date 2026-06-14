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

/// <summary>
/// Controlador responsável por gerenciar as requisições relacionadas aos agendamentos.
/// Controla tanto a visão do cliente (realizar agendamentos, ver seus próprios agendamentos)
/// quanto a visão do funcionário (listar todos, editar, cancelar e finalizar agendamentos).
/// </summary>
public class AgendamentosController : Controller
{
    // Dependências injetadas para acesso a dados e regras de negócio
    private readonly IAgendamentosService _agendamentosService;
    private readonly IClientesService _clientesService;
    private readonly IFuncionariosService _funcionariosService;
    private readonly IServicosService _servicosService;
    private readonly IProdutosService _produtosService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Construtor do AgendamentosController.
    /// Recebe as instâncias dos serviços e do AutoMapper por injeção de dependência.
    /// </summary>
    /// <param name="agendamentosService">Serviço de agendamentos</param>
    /// <param name="clientesService">Serviço de clientes</param>
    /// <param name="funcionariosService">Serviço de funcionários</param>
    /// <param name="servicosService">Serviço de serviços (manicure, pedicure, etc.)</param>
    /// <param name="produtosService">Serviço de produtos</param>
    /// <param name="mapper">Mapeador de objetos (AutoMapper)</param>
    public AgendamentosController(
        IAgendamentosService agendamentosService,
        IClientesService clientesService,
        IFuncionariosService funcionariosService,
        IServicosService servicosService,
        IProdutosService produtosService,
        IMapper mapper)
    {
        // Atribui as dependências às variáveis de classe (campos) correspondentes
        _agendamentosService = agendamentosService;
        _clientesService = clientesService;
        _funcionariosService = funcionariosService;
        _servicosService = servicosService;
        _produtosService = produtosService;
        _mapper = mapper;
    }

    /// <summary>
    /// Método executado antes de qualquer action deste controller.
    /// É utilizado aqui para verificar a autenticação e autorização dos usuários.
    /// </summary>
    /// <param name="context">O contexto da execução da action</param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Obtém o nome da action que está sendo chamada a partir das rotas
        var actionName = context.ActionDescriptor.RouteValues["action"];
        
        // Define quais actions são permitidas para clientes
        var isClientAction = actionName == "Agendar" || 
                             actionName == "AgendamentoConfirmado" || 
                             actionName == "ObterHorariosDisponiveis" || 
                             actionName == "MeusAgendamentos" || 
                             actionName == "Logout";
                             
        // Recupera o tipo de usuário que está logado, salvo na sessão ("Cliente" ou "Funcionario")
        var tipoUsuario = HttpContext.Session.GetString("UsuarioTipo");

        // Se não houver tipo de usuário na sessão, significa que o usuário não está logado
        if (string.IsNullOrEmpty(tipoUsuario))
        {
            // Redireciona o usuário para a página de Login
            context.Result = new RedirectToActionResult("Index", "Login", null);
        }
        // Se for um Cliente tentando acessar uma action que não é permitida para clientes (ex: Editar agendamentos)
        else if (tipoUsuario == "Cliente" && !isClientAction)
        {
            // Redireciona o cliente para a sua página de "Meus Agendamentos"
            context.Result = new RedirectToActionResult("MeusAgendamentos", "Agendamentos", null);
        }
        // Se for um Funcionário tentando acessar as telas que são exclusivas do fluxo de cliente
        else if (tipoUsuario == "Funcionario" && (actionName == "Agendar" || actionName == "AgendamentoConfirmado" || actionName == "MeusAgendamentos"))
        {
            // Redireciona o funcionário para a listagem geral de agendamentos
            context.Result = new RedirectToActionResult("Index", "Agendamentos", null);
        }

        // Chama a implementação base para continuar o ciclo de vida da requisição
        base.OnActionExecuting(context);
    }

    /// <summary>
    /// Action que lista todos os agendamentos. Acesso restrito a Funcionários.
    /// </summary>
    /// <returns>A View Index com a lista de agendamentos mapeados para ViewModel</returns>
    public async Task<IActionResult> Index()
    {
        // Busca todos os agendamentos cadastrados através do serviço
        var agendamentos = await _agendamentosService.ObterTodosAgendamentos();
        
        // Mapeia a lista de agendamentos do tipo Entidade (Models) para ViewModels (usadas na tela)
        var viewModel = _mapper.Map<IEnumerable<AgendamentoListagemViewModel>>(agendamentos);

        // Retorna a View passando o viewModel
        return View(viewModel);
    }

    /// <summary>
    /// Action para exibir os detalhes de um agendamento específico.
    /// </summary>
    /// <param name="id">O ID do agendamento</param>
    /// <returns>A View Detalhes com as informações do agendamento ou NotFound se não existir</returns>
    [HttpGet]
    public async Task<IActionResult> Detalhes(int id)
    {
        // Tenta recuperar o agendamento pelo seu identificador único
        var agendamento = await _agendamentosService.ObterAgendamentoPorId(id);
        
        // Se o agendamento não for encontrado, retorna HTTP 404 (Não Encontrado)
        if (agendamento == null) return NotFound();

        // Mapeia o agendamento para o formato DetalhesViewModel
        var viewModel = _mapper.Map<AgendamentoDetalhesViewModel>(agendamento);
        return View(viewModel);
    }

    /// <summary>
    /// Action GET para exibir o formulário de edição de um agendamento.
    /// </summary>
    /// <param name="id">O ID do agendamento a ser editado</param>
    /// <returns>A View Editar preenchida com os dados atuais do agendamento</returns>
    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        // Busca o agendamento que se deseja editar
        var agendamento = await _agendamentosService.ObterAgendamentoPorId(id);
        if (agendamento == null) return NotFound();

        // Mapeia para o ViewModel de edição
        var viewModel = _mapper.Map<AgendamentoEditarViewModel>(agendamento);
        
        // Prepara as listas de seleção (dropdowns) que a View necessitará (clientes, funcionários, serviços, etc.)
        await PrepararListasAsync(viewModel);
        return View(viewModel);
    }

    /// <summary>
    /// Action POST que recebe os dados enviados pelo formulário de edição para salvar no banco.
    /// </summary>
    /// <param name="id">O ID do agendamento na URL</param>
    /// <param name="viewModel">Os dados modificados pelo usuário no formulário</param>
    /// <returns>Redireciona para Index em caso de sucesso, ou retorna a View com erros de validação</returns>
    [HttpPost]
    public async Task<IActionResult> Editar(int id, AgendamentoEditarViewModel viewModel)
    {
        // Valida se o ID da URL corresponde ao número do agendamento no modelo enviado
        // Isso previne que alguém altere maliciosamente o ID na requisição
        if (id != viewModel.Nr) return BadRequest();

        // Se o estado do modelo (validações como campos obrigatórios, tamanho máximo) não for válido
        if (!ModelState.IsValid)
        {
            // É necessário carregar novamente as listas antes de devolver a página com erros
            await PrepararListasAsync(viewModel);
            return View(viewModel);
        }

        try
        {
            // Obtém o agendamento original no banco de dados
            var agendamentoAtual = await _agendamentosService.ObterAgendamentoPorId(id);
            if (agendamentoAtual == null) return NotFound();

            // Proteção: caso o form não tenha enviado o Status (ex: campo desabilitado),
            // mantém-se o status que já estava no banco.
            if (!Request.HasFormContentType || !Request.Form.ContainsKey(nameof(viewModel.Status)))
                viewModel.Status = agendamentoAtual.Status;

            // Envia o agendamento atualizado para o serviço persistir no banco de dados
            await _agendamentosService.AtualizarAgendamento(viewModel);
            
            // Retorna para a página de listagem
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Se houver algum erro de sistema ou regra de negócio, adicionamos a mensagem à ModelState
            ModelState.AddModelError(string.Empty, $"Erro ao atualizar agendamento: {ex.Message}");
            await PrepararListasAsync(viewModel);
            return View(viewModel);
        }
    }

    /// <summary>
    /// Action GET para exibir uma página de confirmação de exclusão.
    /// </summary>
    /// <param name="id">ID do agendamento a ser excluído</param>
    /// <returns>View de Excluir contendo os detalhes</returns>
    [HttpGet]
    public async Task<IActionResult> Excluir(int id)
    {
        // Busca o agendamento no banco
        var agendamento = await _agendamentosService.ObterAgendamentoPorId(id);
        if (agendamento == null) return NotFound();

        // Mapeia para ViewModel e apresenta os dados para confirmação visual
        var viewModel = _mapper.Map<AgendamentoDetalhesViewModel>(agendamento);
        return View(viewModel);
    }

    /// <summary>
    /// Action POST que efetivamente realiza a exclusão após confirmação.
    /// Mapeado para o ActionName "Excluir", mas com nome de método diferente (ConfirmarExclusao) 
    /// para diferenciar do método GET (que recebe os mesmos parâmetros).
    /// </summary>
    /// <param name="id">O ID do agendamento</param>
    /// <returns>Redireciona para Index em caso de sucesso</returns>
    [HttpPost, ActionName("Excluir")]
    public async Task<IActionResult> ConfirmarExclusao(int id)
    {
        // Chama o serviço para remover o registro fisicamente (ou logicamente)
        await _agendamentosService.ExcluirAgendamento(id);
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Action POST rápida para cancelar um agendamento diretamente (sem tela de edição).
    /// </summary>
    /// <param name="id">ID do agendamento</param>
    /// <returns>Redireciona para a Index</returns>
    [HttpPost]
    public async Task<IActionResult> Cancelar(int id)
    {
        // Altera o status do agendamento para cancelado utilizando a regra de negócio do serviço
        await _agendamentosService.CancelarAgendamento(id);
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Action POST rápida para finalizar um agendamento (marcar como concluído).
    /// </summary>
    /// <param name="id">ID do agendamento</param>
    /// <returns>Redireciona para a Index</returns>
    [HttpPost]
    public async Task<IActionResult> Finalizar(int id)
    {
        // Altera o status do agendamento para finalizado
        await _agendamentosService.FinalizarAgendamento(id);
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Método privado auxiliar para carregar as listas (ViewBags / SelectLists) usadas
    /// em telas de criação e edição por funcionários (Clientes, Funcionários, Serviços, etc).
    /// </summary>
    /// <param name="viewModel">O ViewModel base de registro/edição de agendamento</param>
    private async Task PrepararListasAsync(AgendamentoRegistroViewModel viewModel)
    {
        // Carrega todas as informações dos respectivos serviços
        var clientes = await _clientesService.ObterTodosClientes();
        var funcionarios = await _funcionariosService.ObterTodosFuncionarios();
        var servicos = await _servicosService.ObterTodosServicos();
        var produtos = await _produtosService.ObterTodosProdutos();

        // Extrai os IDs dos itens que já estão selecionados no agendamento atual (caso seja edição)
        // Isso evita que um item selecionado antes, mas inativado depois, deixe de aparecer e cause erro
        var funcionariosSelecionados = viewModel.ServicosSelecionados?.Select(s => s.FuncionarioId).ToList() ?? new List<int>();
        var servicosSelecionados = viewModel.ServicosSelecionados?.Select(s => s.ServicoId).ToList() ?? new List<int>();
        var produtosSelecionados = viewModel.ProdutosSelecionados?.Select(p => p.ProdutoCodigo).ToList() ?? new List<int>();

        // Filtra as listas mantendo apenas os ativos ou os que já estavam selecionados (mesmo se inativos)
        var clientesAtivos = clientes.Where(c => c.Status == PessoaStatus.Ativo || c.Id == viewModel.ClienteId).ToList();
        var funcionariosAtivos = funcionarios.Where(f => f.Status == PessoaStatus.Ativo || funcionariosSelecionados.Contains(f.Id)).ToList();
        var servicosAtivos = servicos.Where(s => s.Status == ServicoStatus.Ativo || servicosSelecionados.Contains(s.Id)).ToList();
        var produtosAtivos = produtos.Where(p => p.Status == ProdutoStatus.Ativo || produtosSelecionados.Contains(p.Codigo)).ToList();

        // Popula as SelectLists no ViewModel, as quais serão usadas nos elementos <select> (dropdowns) da View
        viewModel.ClientesList = new SelectList(clientesAtivos, nameof(Clientes.Id), nameof(Clientes.Nome), viewModel.ClienteId);
        viewModel.FuncionariosList = new SelectList(funcionariosAtivos, nameof(Funcionarios.Id), nameof(Funcionarios.Nome));
        viewModel.ServicosList = new SelectList(servicosAtivos, nameof(Servicos.Id), nameof(Servicos.Descricao));
        viewModel.ProdutosList = new SelectList(produtosAtivos, nameof(Produtos.Codigo), nameof(Produtos.Nome));
    }

    // ==========================================
    // Versão do CLIENTE (Ações do portal do cliente)
    // ==========================================

    /// <summary>
    /// Action GET para exibir a tela de "Agendar" voltada para o Cliente.
    /// </summary>
    /// <returns>A View Agendar preenchida com as listas de escolha</returns>
    [HttpGet]
    public async Task<IActionResult> Agendar()
    {
        var viewModel = new AgendamentoClienteViewModel();
        // Chama a preparação de listas específica para o cliente (ex: apenas serviços ativos)
        await PrepararListasClienteAsync(viewModel);
        return View(viewModel);
    }

    /// <summary>
    /// Action POST que recebe a tentativa do cliente de criar um agendamento no sistema.
    /// </summary>
    /// <param name="viewModel">O ViewModel de cliente com os dados do serviço que deseja agendar</param>
    /// <returns>Redireciona para sucesso ou recarrega a página em caso de erro</returns>
    [HttpPost]
    public async Task<IActionResult> Agendar(AgendamentoClienteViewModel viewModel)
    {
        // Remove validações dos campos que não usaremos mais no formulário final
        // Pois o Nome e Telefone do cliente já foram pegos no login e estão na sessão
        ModelState.Remove("Nome");
        ModelState.Remove("Telefone");

        // Verifica se os outros dados do formulário estão corretos (ex: escolheu data, serviço)
        if (!ModelState.IsValid)
        {
            // Coleta os erros para gravar em arquivo de debug (útil durante o desenvolvimento/testes)
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            System.IO.File.WriteAllText("modelstate_errors.txt", string.Join("\n", errors));
            
            // Recarrega as listas antes de devolver a view com erro
            await PrepararListasClienteAsync(viewModel);
            return View(viewModel);
        }

        // Outro log de debug gravando as informações no sistema de arquivos
        System.IO.File.WriteAllText("post_debug.txt", System.Text.Json.JsonSerializer.Serialize(viewModel));

        // Pega a identificação do cliente logado pela sessão
        var clienteId = HttpContext.Session.GetInt32("ClienteId");
        
        // Se a sessão expirou ou não existir, o cliente não pode agendar, vai pro login
        if (clienteId == null)
            return RedirectToAction("Index", "Login");

        try
        {
            // O serviço de agendamentos fará toda a inserção e relacionamento no banco de dados
            await _agendamentosService.CriarAgendamentoCliente(viewModel, clienteId.Value);
            
            // Usa o TempData para passar uma mensagem rápida (flash message) para a próxima tela
            TempData["Sucesso"] = "Agendamento realizado com sucesso!";
            return RedirectToAction(nameof(AgendamentoConfirmado));
        }
        catch (Exception ex)
        {
            // Caso falhe ao salvar (ex: horário acabou de ser ocupado), mostra a mensagem
            ModelState.AddModelError(string.Empty, $"Erro ao realizar agendamento: {ex.Message}");
            await PrepararListasClienteAsync(viewModel);
            return View(viewModel);
        }
    }

    /// <summary>
    /// Action simples para mostrar uma tela de sucesso após o agendamento ter sido concluído.
    /// </summary>
    /// <returns>A View de Confirmação</returns>
    [HttpGet]
    public IActionResult AgendamentoConfirmado()
    {
        return View();
    }

    /// <summary>
    /// Endpoint AJAX chamado pela tela de agendamento do cliente via JavaScript.
    /// Quando o cliente escolhe um funcionário e uma data, a página consulta os horários livres.
    /// </summary>
    /// <param name="funcionarioId">O ID do profissional</param>
    /// <param name="data">A data em string (formato YYYY-MM-DD geralmente)</param>
    /// <returns>JSON contendo a lista de horários disponíveis</returns>
    [HttpGet]
    public async Task<IActionResult> ObterHorariosDisponiveis(int funcionarioId, string data)
    {
        // Tenta converter a string de data para o tipo DateTime
        if (!DateTime.TryParse(data, out var dataParsed))
            return Json(new List<string>());

        // Chama a regra de negócio para checar a agenda do profissional naquele dia
        var horarios = await _agendamentosService.ObterHorariosDisponiveis(funcionarioId, dataParsed);
        
        // Formata os horários encontrados para exibir na tela (ex: texto="09:00", valor="09:00:00")
        var resultado = horarios.Select(h => new { valor = h.ToString(@"hh\:mm\:ss"), texto = h.ToString(@"hh\:mm") });
        
        // Retorna formato JSON para o client-side (navegador) consumir
        return Json(resultado);
    }

    /// <summary>
    /// Prepara as listas de funcionários, serviços e produtos especificamente para a tela do Cliente.
    /// </summary>
    /// <param name="viewModel">ViewModel do agendamento cliente</param>
    private async Task PrepararListasClienteAsync(AgendamentoClienteViewModel viewModel)
    {
        // Busca os dados cadastrados
        var funcionarios = await _funcionariosService.ObterTodosFuncionarios();
        var servicos = await _servicosService.ObterTodosServicos();
        var produtos = await _produtosService.ObterTodosProdutos();

        // Garante que mesmo os selecionados (caso aconteça algum recarregamento) sejam mantidos na lista
        var funcionariosSelecionados = viewModel.Servicos?.Select(s => s.FuncionarioId).ToList() ?? new List<int>();
        var servicosSelecionados = viewModel.Servicos?.Select(s => s.ServicoId).ToList() ?? new List<int>();
        
        // Filtra para mostrar somente os ativos e os já selecionados
        var funcionariosAtivos = funcionarios.Where(f => f.Status == PessoaStatus.Ativo || funcionariosSelecionados.Contains(f.Id)).ToList();
        var servicosAtivos = servicos.Where(s => s.Status == ServicoStatus.Ativo || servicosSelecionados.Contains(s.Id)).ToList();
        var produtosAtivos = produtos.Where(p => p.Status == ProdutoStatus.Ativo).ToList();

        // Constrói SelectList para funcionários
        viewModel.FuncionariosList = new SelectList(funcionariosAtivos, nameof(Funcionarios.Id), nameof(Funcionarios.Nome));
        
        // Para serviços e produtos, criamos um objeto anônimo que tem a propriedade "TextoFormatado" 
        // para exibir o nome e o preço juntos no dropdown
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

        // Passa esses objetos em JSON via ViewBag para que o JavaScript na tela possa acessá-los 
        // e usá-los (por exemplo, atualizar o total a pagar ao selecionar um serviço)
        ViewBag.ProdutosJson = System.Text.Json.JsonSerializer.Serialize(produtosItens);
        ViewBag.ServicosJson = System.Text.Json.JsonSerializer.Serialize(servicosItens);
    }

    /// <summary>
    /// Exibe os agendamentos do cliente logado via Session
    /// </summary>
    /// <returns>A View de listar agendamentos do cliente</returns>
    [HttpGet]
    public async Task<IActionResult> MeusAgendamentos()
    {
        // Resgata os dados da sessão
        var clienteId = HttpContext.Session.GetInt32("ClienteId");
        var clienteTelefone = HttpContext.Session.GetString("ClienteTelefone");
        var tipoUsuario = HttpContext.Session.GetString("UsuarioTipo");

        // Se for um funcionário acessando esta rota, redireciona para a listagem geral do sistema
        if (tipoUsuario == "Funcionario")
            return RedirectToAction("Index", "Agendamentos");

        // Se o cliente não estiver logado (sessão vazia), vai pro login
        if (!clienteId.HasValue || string.IsNullOrEmpty(clienteTelefone))
            return RedirectToAction("Index", "Login");

        // Busca apenas os agendamentos pertencentes ao cliente autenticado
        var agendamentos = await _agendamentosService.ObterAgendamentosPorCliente(clienteId.Value);
        
        // Mapeia os dados da Entidade para exibição na View de Listagem
        var viewModel = _mapper.Map<IEnumerable<AgendamentoListagemViewModel>>(agendamentos);

        // Retorna a view específica para o cliente com os seus agendamentos
        return View(viewModel);
    }

    /// <summary>
    /// Realiza o logout do usuário (Cliente ou Funcionário) limpando a Sessão e retornando à Home.
    /// </summary>
    /// <returns>Redireciona para a Home do site</returns>
    [HttpGet]
    public IActionResult Logout()
    {
        // Limpa todas as chaves da sessão atual, deslogando o usuário
        HttpContext.Session.Clear();
        // Redireciona o usuário para a página principal do site
        return RedirectToAction("Index", "Home");
    }
}
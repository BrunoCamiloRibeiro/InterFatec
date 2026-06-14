using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using FabysUnha.Services;
using FabysUnha.ViewModels.Servicos;
using AutoMapper;

namespace FabysUnha.Controllers;

/// <summary>
/// Controlador responsável por orquestrar as requisições referentes aos Serviços prestados.
/// Envolve a visualização, o cadastro, a atualização e a remoção dos serviços disponíveis.
/// </summary>
public class ServicosController : Controller
{
    /// <summary>
    /// Dependência do serviço que contém a lógica de negócios para a entidade "Servicos".
    /// </summary>
    private readonly IServicosService _servicosService;

    /// <summary>
    /// Ferramenta para tradução de Models de Domínio em ViewModels de forma ágil e segura.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Construtor que recebe injeções de dependência configuradas no Program.cs.
    /// </summary>
    /// <param name="servicosService">Implementação do serviço de serviços injetada pelo framework.</param>
    /// <param name="mapper">Instância injetada do AutoMapper.</param>
    public ServicosController(IServicosService servicosService, IMapper mapper)
    {
        // Atribui os serviços às variáveis apenas de leitura para uso por todo o ciclo de vida do controller.
        _servicosService = servicosService;
        _mapper = mapper;
    }

    /// <summary>
    /// Método disparado pelo pipeline de execução antes de iniciar a execução da View respectiva ou processar qualquer dado.
    /// </summary>
    /// <param name="context">O contexto atual da requisição, provendo informações como HttpContext e Rotas.</param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Extrai a string de tipo de usuário armazenada temporariamente na sessão.
        var tipoUsuario = HttpContext.Session.GetString("UsuarioTipo");
        
        // Bloqueia qualquer acesso de usuários que não tenham a "role" de Funcionario.
        if (tipoUsuario != "Funcionario")
        {
            // Interrompe o fluxo e força um redirecionamento imediato para a tela de autenticação.
            context.Result = new RedirectToActionResult("Index", "Login", null);
        }
        
        // Retoma o fluxo para a classe pai concluir a execução do filtro.
        base.OnActionExecuting(context);
    }

    /// <summary>
    /// Ação HTTP GET para listar no navegador todos os serviços disponíveis.
    /// </summary>
    /// <returns>A tela (View) povoada pela lista de serviços encapsulada em ServicoListagemViewModel.</returns>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // Consulta o banco, em uma chamada assíncrona, e recebe todos os serviços.
        var servicos = await _servicosService.ObterTodosServicos();
        
        // Usa o AutoMapper para converter a coleção do tipo Entidade em coleção do tipo ViewModel.
        var viewModel = _mapper.Map<IEnumerable<ServicoListagemViewModel>>(servicos);

        // Prepara e devolve os dados mapeados para renderização na camada da View.
        return View(viewModel);
    }

    /// <summary>
    /// Ação HTTP GET acionada ao solicitar uma visualização profunda (detalhes) de apenas um serviço.
    /// </summary>
    /// <param name="id">Chave primária do serviço no banco de dados.</param>
    /// <returns>Retorna os dados completos ou gera um erro caso seja um Id fantasma.</returns>
    [HttpGet]
    public async Task<IActionResult> Detalhes(int id)
    {
        // Chama a base de dados em busca daquele ID específico do serviço.
        var servico = await _servicosService.ObterServicoPorId(id);
        
        // Tratamento simples contra erros de referência inexistente na base.
        if (servico == null) return NotFound();

        // O serviço resgatado é então transposto para seu formato focado em detalhes.
        var viewModel = _mapper.Map<ServicoDetalhesViewModel>(servico);
        
        return View(viewModel);
    }

    /// <summary>
    /// Ação HTTP GET chamada ao clicar no botão "Novo" ou navegar pela url /Servicos/Criar.
    /// </summary>
    /// <returns>Entrega uma página com um formulário limpo preparado para inserção de um Serviço.</returns>
    [HttpGet]
    public IActionResult Criar()
    {
        // A view recebe um ServicoRegistroViewModel zerado, essencial para vincular campos do HTML com a model.
        return View(new ServicoRegistroViewModel());
    }

    /// <summary>
    /// Ação HTTP POST que captura os dados enviados (submetidos) da página de registro.
    /// </summary>
    /// <param name="viewModel">Os valores recém-digitados que compõem o novo serviço.</param>
    /// <returns>Redireciona caso o serviço seja criado com sucesso ou refaz a página mantendo o que foi digitado em caso de erro.</returns>
    [HttpPost]
    public async Task<IActionResult> Criar(ServicoRegistroViewModel viewModel)
    {
        // Verifica as anotações do ViewModel e impede o prosseguimento caso algo fuja das regras estabelecidas.
        if (!ModelState.IsValid) return View(viewModel);

        try
        {
            // Usa o mapper para jogar todas as informações aprovadas para a classe espelho do banco de dados (Entidade).
            var servico = _mapper.Map<Models.Servicos>(viewModel);
            
            // Orquestra a requisição para inserção por meio do serviço injetado.
            await _servicosService.CriarServico(servico);
            
            // Depois do trabalho concluído perfeitamente, manda de volta para a tabela de todos os serviços.
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Adiciona no formulário a resposta com o problema detectado durante o cadastro no banco.
            ModelState.AddModelError(string.Empty, $"Erro ao criar serviço: {ex.Message}");
            return View(viewModel);
        }
    }

    /// <summary>
    /// Ação HTTP GET encarregada de exibir a interface para alteração de propriedades de um serviço anterior.
    /// </summary>
    /// <param name="id">O identificador numérico atrelado ao serviço desejado para mudança.</param>
    /// <returns>A página que contêm campos a serem substituídos, preenchidos preventivamente.</returns>
    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        // Encontra o serviço com base na busca individual por chave de acesso.
        var servico = await _servicosService.ObterServicoPorId(id);
        
        // Verifica de modo direto se essa busca não retornou null, se sim para por aqui mesmo.
        if (servico == null) return NotFound();

        // Envia as propriedades recém-achadas do banco para a tela via ServicoEditarViewModel.
        var viewModel = _mapper.Map<ServicoEditarViewModel>(servico);
        
        return View(viewModel);
    }

    /// <summary>
    /// Ação HTTP POST chamada por meio de um formulário preenchido da seção de edição.
    /// </summary>
    /// <param name="viewModel">Um objeto contendo as propriedades modificadas aguardando a persistência.</param>
    /// <returns>Retorna uma tela Index quando o update obtiver êxito, ou exibe os problemas levantados.</returns>
    [HttpPost]
    public async Task<IActionResult> Editar(ServicoEditarViewModel viewModel)
    {
        // Processa as requisições básicas de tipagem e preenchimento (exigido pelos Data Annotations).
        if (!ModelState.IsValid) return View(viewModel);

        try
        {
            // Antes de alterar, busca a versão do banco para checar se ele ainda existe e para reter possíveis valores.
            var servicoAtual = await _servicosService.ObterServicoPorId(viewModel.Id);
            
            // Encerra imediatamente se não localizou, impedindo o update em base de dados de um registro fantasma.
            if (servicoAtual == null) return NotFound();

            // Averígua se o pacote recebido via formulário (form content) trazia uma tag Status junto consigo.
            // Isso previne que a propriedade Status seja zerada se o HTML falhar em enviá-la.
            if (!Request.HasFormContentType || !Request.Form.ContainsKey(nameof(viewModel.Status)))
                viewModel.Status = servicoAtual.Status; // Reafirma o valor antigo do status.

            // Finalmente, refaz o modelo contendo a mescla dos atributos com a forma compatível para o DataLayer.
            var servico = _mapper.Map<Models.Servicos>(viewModel);
            
            // Consagra as modificações executando a atualização na base através do serviço acoplado.
            await _servicosService.AtualizarServico(servico);
            
            // Conduz o fluxo final da tela a visualizar a listagem novamente.
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Repassa o aviso de falha (Exception capturada) apontando os detalhes da avaria em tela.
            ModelState.AddModelError(string.Empty, $"Erro ao atualizar serviço: {ex.Message}");
            return View(viewModel);
        }
    }

    /// <summary>
    /// Ação HTTP GET designada a solicitar a confirmação para a ação destrutiva de exclusão.
    /// </summary>
    /// <param name="id">Chave de acesso requerida para localizar qual serviço se pretende destruir.</param>
    /// <returns>Uma visão restritiva que somente exibe dados e questiona se deseja excluí-los.</returns>
    [HttpGet]
    public async Task<IActionResult> Excluir(int id)
    {
        // Encontra o serviço pelo identificador no domínio principal.
        var servico = await _servicosService.ObterServicoPorId(id);
        
        // Bloqueia com a resposta de "Arquivo Ausente" se não for reconhecido.
        if (servico == null) return NotFound();

        // O tipo DetalhesViewModel é reutilizado aqui, sendo suficiente para a visualização simplificada da exclusão.
        var viewModel = _mapper.Map<ServicoDetalhesViewModel>(servico);
        
        return View(viewModel);
    }

    /// <summary>
    /// Ação HTTP POST que concretiza o desejo de extinção do elemento selecionado na tela de exclusão.
    /// ActionName usado aqui permite manter o método C# com o nome descritivo "ConfirmarExclusao",
    /// enquanto a view e as rotas continuam enxergando "Excluir".
    /// </summary>
    /// <param name="id">O identificador chave confirmando a remoção do banco.</param>
    /// <returns>O redirecionamento padrão ao término do evento devolvendo-o para a lista do sistema.</returns>
    [HttpPost, ActionName("Excluir")]
    public async Task<IActionResult> ConfirmarExclusao(int id)
    {
        try
        {
            // Envia o pedido de supressão ao serviço, que lidará com a camada inferior de dados.
            await _servicosService.ExcluirServico(id);
            
            // Transfere o andamento para a seção inicial.
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Registra um erro associado se não houver exito ao processar o fim da entidade e volta a raiz.
            ModelState.AddModelError(string.Empty, $"Erro ao excluir serviço: {ex.Message}");
            return RedirectToAction(nameof(Index)); 
        }
    }
}
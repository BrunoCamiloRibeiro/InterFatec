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

/// <summary>
/// Controlador responsável por gerenciar o ciclo de vida dos Produtos no sistema.
/// Abrange listagem, visualização de detalhes, criação, edição e exclusão de produtos.
/// </summary>
public class ProdutosController : Controller
{
    /// <summary>
    /// Serviço responsável pela lógica de negócios dos produtos.
    /// </summary>
    private readonly IProdutosService _produtosService;

    /// <summary>
    /// Serviço de marcas, necessário para carregar listas de marcas (ex: dropdowns de seleção).
    /// </summary>
    private readonly IMarcasService _marcasService; 

    /// <summary>
    /// Utilitário responsável pelo mapeamento entre Entidades e ViewModels.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Interface que fornece informações sobre o ambiente de hospedagem da web (como caminhos de arquivos).
    /// </summary>
    private readonly IWebHostEnvironment _env;

    /// <summary>
    /// Construtor para injeção das dependências necessárias ao controlador.
    /// </summary>
    /// <param name="produtosService">Serviço de gerenciamento de produtos.</param>
    /// <param name="marcasService">Serviço de gerenciamento de marcas.</param>
    /// <param name="mapper">O objeto mapeador (AutoMapper).</param>
    /// <param name="env">Objeto de contexto do ambiente de hospedagem web.</param>
    public ProdutosController(IProdutosService produtosService, IMarcasService marcasService, IMapper mapper, IWebHostEnvironment env)
    {
        // Inicialização de todos os serviços de leitura.
        _produtosService = produtosService;
        _marcasService = marcasService;
        _mapper = mapper;
        _env = env;
    }

    /// <summary>
    /// Executado automaticamente antes de qualquer método de ação neste controlador ser chamado.
    /// Utilizado como um "filtro de autorização" manual.
    /// </summary>
    /// <param name="context">Contexto da ação que está prestes a ser executada.</param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Resgata o tipo de usuário contido na sessão atual.
        var tipoUsuario = HttpContext.Session.GetString("UsuarioTipo");
        
        // Bloqueia o acesso a usuários que não possuam o perfil 'Funcionario'.
        if (tipoUsuario != "Funcionario")
        {
            // Substitui o resultado atual, forçando o redirecionamento para o login.
            context.Result = new RedirectToActionResult("Index", "Login", null);
        }
        
        // O fluxo da classe base é chamado para concluir a execução normal do pipeline.
        base.OnActionExecuting(context);
    }

    /// <summary>
    /// Ação responsável por listar todos os produtos disponíveis.
    /// </summary>
    /// <returns>View contendo a lista dos produtos formatada em um ViewModel.</returns>
    public async Task<IActionResult> Index()
    {
        // Chama a camada de serviço para obter todos os produtos do banco de forma assíncrona.
        var produtos = await _produtosService.ObterTodosProdutos();
        
        // Utiliza o mapeador para transformar as entidades em objetos de visualização otimizados.
        var viewModel = _mapper.Map<IEnumerable<ProdutoListagemViewModel>>(produtos);

        // Renderiza e devolve a view padrão Index, populada com a coleção.
        return View(viewModel);
    }

    /// <summary>
    /// Ação HTTP GET para exibir de forma detalhada as informações de um produto específico.
    /// </summary>
    /// <param name="id">O identificador do produto que se deseja visualizar.</param>
    /// <returns>A View contendo todos os detalhes do produto, ou NotFound caso o ID seja inválido.</returns>
    [HttpGet]
    public async Task<IActionResult> Detalhes(int id)
    {
        // Solicita ao serviço o resgate do produto pelo seu ID.
        var produto = await _produtosService.ObterProdutoPorId(id);
        
        // Se a busca retornar nulo, a página 404 é exibida ao usuário.
        if (produto == null) return NotFound();

        // Converte o objeto recuperado em um ProdutoDetalhesViewModel, próprio para esta tela.
        var viewModel = _mapper.Map<ProdutoDetalhesViewModel>(produto);
        
        // Envia o objeto ViewModel para o processamento da View.
        return View(viewModel);
    }

    /// <summary>
    /// Ação HTTP GET que apresenta a interface de registro de um novo produto.
    /// </summary>
    /// <returns>A View de criação já com a lista de marcas carregada e pronta para seleção.</returns>
    [HttpGet]
    public async Task<IActionResult> Criar()
    {
        // Busca todas as marcas no banco e, por meio do LINQ, filtra somente as ativas.
        var marcasAtivas = (await _marcasService.ObterTodasMarcas())
            .Where(m => m.Status == FabysUnha.Enums.MarcaStatus.Ativo)
            .ToList();

        // Prepara um novo ViewModel de registro, populando a propriedade MarcasList com as opções para o DropDown.
        var viewModel = new ProdutoRegistroViewModel
        {
            // Cria um SelectList que fará o binding do Id e mostrará o Nome na combobox.
            MarcasList = new SelectList(marcasAtivas, "Id", "Nome")
        };
        
        return View(viewModel);
    }

    /// <summary>
    /// Ação HTTP POST responsável por receber e processar os dados para a inserção de um novo produto.
    /// </summary>
    /// <param name="viewModel">Dados preenchidos no formulário (incluindo arquivo de imagem).</param>
    /// <returns>Retorna à listagem (Index) ou reexibe a tela de criação em caso de erro.</returns>
    [HttpPost]
    public async Task<IActionResult> Criar(ProdutoRegistroViewModel viewModel)
    {
        // Verifica se não há violações nas validações do DataAnnotation definidas no ViewModel.
        if (!ModelState.IsValid) 
        {
            // Se houver erro de validação, é necessário recarregar a lista de marcas ativas
            // pois o HTML da dropdown precisa ser renderizado novamente.
            var marcasAtivas = (await _marcasService.ObterTodasMarcas())
                .Where(m => m.Status == FabysUnha.Enums.MarcaStatus.Ativo)
                .ToList();
            viewModel.MarcasList = new SelectList(marcasAtivas, "Id", "Nome");
            
            // Retorna o formulário para o usuário arrumar os erros, sem perder o que ele preencheu.
            return View(viewModel);
        }

        try
        {
            // Realiza o mapeamento dos dados da interface para a Entidade "Produtos"
            var produto = _mapper.Map<Models.Produtos>(viewModel);
            
            // Envia o produto e o arquivo da imagem para a rotina de criação e upload no serviço.
            await _produtosService.CriarProduto(produto, viewModel.ImagemUpload);
            
            // Sucesso! Redireciona de volta à tela de listagem de produtos.
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Caso ocorra uma exceção (ex: falha ao salvar a imagem), inclui uma mensagem de erro na ModelState.
            ModelState.AddModelError(string.Empty, $"Erro ao criar produto: {ex.Message}");
            
            // Novamente, é preciso recarregar o SelectList das marcas.
            var marcasAtivas = (await _marcasService.ObterTodasMarcas())
                .Where(m => m.Status == FabysUnha.Enums.MarcaStatus.Ativo)
                .ToList();
            viewModel.MarcasList = new SelectList(marcasAtivas, "Id", "Nome");
            
            // Retorna a view para tentar novamente.
            return View(viewModel);
        }
    }

    /// <summary>
    /// Ação HTTP GET para carregar a tela de edição, preenchida com as informações de um produto específico.
    /// </summary>
    /// <param name="id">O ID do produto a ser modificado.</param>
    /// <returns>A View preenchida com os dados a alterar, ou NotFound.</returns>
    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        // Traz as informações originais do banco de dados pelo ID fornecido.
        var produto = await _produtosService.ObterProdutoPorId(id);
        
        // Verifica integridade do dado.
        if (produto == null) return NotFound();

        // Converte as propriedades para um ProdutoEditarViewModel
        var viewModel = _mapper.Map<ProdutoEditarViewModel>(produto);
        
        // Busca as marcas ativas E TAMBÉM a marca atual selecionada, para garantir
        // que ela apareça na dropdown mesmo se tiver sido inativada posteriormente.
        var marcasAtivasEAtual = (await _marcasService.ObterTodasMarcas())
            .Where(m => m.Status == FabysUnha.Enums.MarcaStatus.Ativo || m.Id == viewModel.MarcaId)
            .ToList();
        
        // Constrói o dropdown já pré-selecionando o viewModel.MarcaId
        viewModel.MarcasList = new SelectList(marcasAtivasEAtual, "Id", "Nome", viewModel.MarcaId);
        
        return View(viewModel);
    }

    /// <summary>
    /// Ação HTTP POST que recepciona a requisição para salvar as modificações do produto.
    /// </summary>
    /// <param name="viewModel">Os dados editados no formulário web.</param>
    /// <returns>Retorna à Index após o sucesso, ou apresenta a View com feedback de erros.</returns>
    [HttpPost]
    public async Task<IActionResult> Editar(ProdutoEditarViewModel viewModel)
    {
        // Se as regras contidas no ViewModel não forem atendidas (ex: campo em branco).
        if (!ModelState.IsValid) 
        {
            // Recarrega o dropdown de marcas antes de devolver a página de volta.
            var marcasAtivasEAtual = (await _marcasService.ObterTodasMarcas())
                .Where(m => m.Status == FabysUnha.Enums.MarcaStatus.Ativo || m.Id == viewModel.MarcaId)
                .ToList();
            viewModel.MarcasList = new SelectList(marcasAtivasEAtual, "Id", "Nome", viewModel.MarcaId);
            return View(viewModel);
        }

        try
        {
            // Checa se foi recebido algum campo "Status" na requisição.
            // Ajuda a evitar perda do status original se o checkbox não estiver presente no form HTML.
            bool hasStatusUpdate = Request.HasFormContentType && Request.Form.ContainsKey(nameof(viewModel.Status));
            
            // Prepara a entidade a partir do ViewModel recebido.
            var produto = _mapper.Map<Models.Produtos>(viewModel);
            
            // Chama a camada de negócio para validar e aplicar a atualização (bem como cuidar de eventual upload de imagem).
            await _produtosService.AtualizarProduto(produto, viewModel.ImagemUpload, hasStatusUpdate);
            
            // Salvo com sucesso! Volta para a página de produtos.
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Se algo falhar (regras de banco, permissões, etc), a exceção é capturada e a tela exibirá a mensagem de erro.
            ModelState.AddModelError(string.Empty, $"Erro ao atualizar produto: {ex.Message}");
            
            // Preenche o campo select com as marcas ativas e a original do modelo para o usuário tentar arrumar.
            var marcasAtivasEAtual = (await _marcasService.ObterTodasMarcas())
                .Where(m => m.Status == FabysUnha.Enums.MarcaStatus.Ativo || m.Id == viewModel.MarcaId)
                .ToList();
            viewModel.MarcasList = new SelectList(marcasAtivasEAtual, "Id", "Nome", viewModel.MarcaId);
            return View(viewModel);
        }
    }

    /// <summary>
    /// Ação HTTP GET que abre uma tela de confirmação de exclusão do produto.
    /// </summary>
    /// <param name="id">O ID do produto candidato a exclusão.</param>
    /// <returns>A tela com as informações do produto, pedindo a confirmação do usuário.</returns>
    [HttpGet]
    public async Task<IActionResult> Excluir(int id)
    {
        // Verifica a existência do produto no momento que o usuário clica no botão "Excluir".
        var produto = await _produtosService.ObterProdutoPorId(id);
        
        // Caso já tenha sido removido, informa tela 404 (NotFound).
        if (produto == null) return NotFound();

        // Mapeia para uma versão somente leitura de detalhes.
        var viewModel = _mapper.Map<ProdutoDetalhesViewModel>(produto);
        
        return View(viewModel);
    }

    /// <summary>
    /// Ação HTTP POST acionada quando o usuário confirma que de fato quer remover o item.
    /// </summary>
    /// <param name="id">O ID do produto que deve ser apagado.</param>
    /// <returns>Retorna à tela principal independentemente de falhar (mostrará o erro) ou ter sucesso.</returns>
    [HttpPost, ActionName("Excluir")]
    public async Task<IActionResult> ConfirmarExclusao(int id)
    {
        try
        {
            // Dispara a deleção lógica/física pelo lado do serviço do banco de dados.
            await _produtosService.ExcluirProduto(id);
            
            // Devolve o usuário à index de Produtos.
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Adiciona a falha ocorrida de volta no ModelState, embora em caso de redirecionamento isto se perca, 
            // frequentemente usa-se TempData para exibir. Deixaremos conforme a lógica original.
            ModelState.AddModelError(string.Empty, $"Erro ao excluir produto: {ex.Message}");
            return RedirectToAction(nameof(Index)); 
        }
    }
}
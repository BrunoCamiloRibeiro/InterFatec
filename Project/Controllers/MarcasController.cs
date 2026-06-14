using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using FabysUnha.Services;
using FabysUnha.ViewModels;
using AutoMapper;

namespace FabysUnha.Controllers;

/// <summary>
/// Controlador responsável por gerenciar as operações relacionadas às Marcas no sistema.
/// Lida com a listagem, criação, edição e exclusão (CRUD) de marcas.
/// </summary>
public class MarcasController : Controller
{
    /// <summary>
    /// Serviço de domínio para encapsular a lógica de negócios das marcas.
    /// </summary>
    private readonly IMarcasService _MarcasService;

    /// <summary>
    /// Utilitário para realizar o mapeamento entre Entidades de Domínio e ViewModels.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Construtor do controlador que recebe as dependências via injeção de dependência.
    /// </summary>
    /// <param name="MarcasService">Serviço de marcas injetado.</param>
    /// <param name="mapper">Serviço de mapeamento (AutoMapper) injetado.</param>
    public MarcasController(IMarcasService MarcasService, IMapper mapper)
    {
        // Atribui as instâncias recebidas às variáveis de leitura (readonly) da classe.
        _MarcasService = MarcasService;
        _mapper = mapper;
    }

    /// <summary>
    /// Método executado antes de qualquer ação do controlador ser chamada.
    /// Utilizado aqui para verificar a autorização do usuário através da sessão.
    /// </summary>
    /// <param name="context">O contexto da ação em execução.</param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Obtém o tipo do usuário armazenado na sessão atual.
        var tipoUsuario = HttpContext.Session.GetString("UsuarioTipo");
        
        // Verifica se o usuário não é um funcionário.
        if (tipoUsuario != "Funcionario")
        {
            // Se não for funcionário, redireciona para a página de Login.
            context.Result = new RedirectToActionResult("Index", "Login", null);
        }
        
        // Chama a implementação base para garantir que o fluxo de execução continue normalmente.
        base.OnActionExecuting(context);
    }

    /// <summary>
    /// Ação responsável por exibir a lista de todas as marcas cadastradas.
    /// </summary>
    /// <returns>Retorna a View contendo a lista de marcas (ViewModels).</returns>
    public IActionResult Index()
    {
        // Obtém a lista de entidades de marca através do serviço.
        var marcas = _MarcasService.ObterTodasMarcas().Result;
        
        // Mapeia a lista de entidades (Models.Marcas) para uma lista de ViewModels (MarcasViewModel).
        var marcasViewModel = _mapper.Map<IEnumerable<MarcasViewModel>>(marcas);
        
        // Retorna a view 'Index' passando o modelo de dados preparado para exibição.
        return View(marcasViewModel);
    }

    /// <summary>
    /// Ação HTTP GET para exibir o formulário de criação de uma nova marca.
    /// </summary>
    /// <returns>A View contendo um objeto vazio de MarcasViewModel.</returns>
    [HttpGet]
    public IActionResult Criar()
    {
        // Retorna a view com uma nova instância do ViewModel para os campos começarem vazios.
        return View(new MarcasViewModel());
    }

    /// <summary>
    /// Ação HTTP POST para processar os dados submetidos pelo formulário de criação.
    /// </summary>
    /// <param name="marcaViewModel">Os dados da nova marca preenchidos pelo usuário.</param>
    /// <returns>Redireciona para a Index se houver sucesso, caso contrário, retorna para a View com os erros.</returns>
    [HttpPost]
    public async Task<IActionResult> Criar(MarcasViewModel marcaViewModel)
    {
        // Verifica se o modelo recebido passou em todas as regras de validação (anotações).
        if (!ModelState.IsValid) return View(marcaViewModel);

        // Mapeia o ViewModel preenchido para a entidade de domínio 'Marcas'.
        var marca = _mapper.Map<Models.Marcas>(marcaViewModel);
        
        // Chama o serviço para criar e persistir a nova marca no banco de dados.
        await _MarcasService.CriarMarca(marca);
        
        // Redireciona o usuário de volta para a ação 'Index' (lista de marcas).
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Ação HTTP GET para exibir o formulário de edição de uma marca existente.
    /// </summary>
    /// <param name="id">O identificador único da marca a ser editada.</param>
    /// <returns>A View preenchida com os dados da marca, ou NotFound caso não seja localizada.</returns>
    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        // Busca a marca correspondente no banco de dados pelo seu ID.
        var marca = await _MarcasService.ObterMarcaPorId(id);

        // Se a marca não existir, retorna um erro 404 (Não Encontrado).
        if (marca == null) return NotFound();

        // Mapeia a entidade encontrada para um ViewModel para ser exibido na View.
        var marcaViewModel = _mapper.Map<MarcasViewModel>(marca);
        
        // Retorna a view com os dados para serem editados.
        return View(marcaViewModel);
    }

    /// <summary>
    /// Ação HTTP POST para processar os dados alterados da marca.
    /// </summary>
    /// <param name="marcaViewModel">O ViewModel contendo os dados modificados da marca.</param>
    /// <returns>Redireciona para a lista (Index) após sucesso ou devolve a View em caso de erro.</returns>
    [HttpPost]
    public async Task<IActionResult> Editar(MarcasViewModel marcaViewModel)
    {
        // Verifica se os dados submetidos são válidos de acordo com as regras de anotação (Data Annotations).
        if (!ModelState.IsValid) return View(marcaViewModel);

        // Busca a marca original no banco de dados antes de atualizar.
        var marcaAtual = await _MarcasService.ObterMarcaPorId(marcaViewModel.Id);
        
        // Caso não seja encontrada, significa que foi removida ou o ID é inválido.
        if (marcaAtual == null) return NotFound();

        // Verifica se o formulário não enviou a propriedade 'Status'.
        // Isso pode ocorrer em campos desativados ou ocultos do formulário.
        if (!Request.HasFormContentType || !Request.Form.ContainsKey(nameof(marcaViewModel.Status)))
            marcaViewModel.Status = marcaAtual.Status; // Preserva o status original da marca.

        // Mapeia o ViewModel de volta para o modelo de Entidade.
        var marca = _mapper.Map<Models.Marcas>(marcaViewModel);
        
        // Solicita ao serviço que atualize as informações da marca no banco de dados.
        await _MarcasService.AtualizarMarca(marca);
        
        // Retorna para a página principal (Index) que listará as marcas.
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Ação HTTP GET para exibir uma tela de confirmação de exclusão para uma marca.
    /// </summary>
    /// <param name="id">O ID da marca a ser excluída.</param>
    /// <returns>A View com os dados da marca ou um erro de NotFound.</returns>
    [HttpGet]
    public async Task<IActionResult> Excluir(int id)
    {
        // Localiza a marca desejada através de seu ID.
        var marca = await _MarcasService.ObterMarcaPorId(id);

        // Se ela não existir, não há o que excluir, retorna NotFound.
        if (marca == null) return NotFound();

        // Converte a entidade de domínio para ViewModel para enviar à camada de apresentação.
        var marcaViewModel = _mapper.Map<MarcasViewModel>(marca);
        
        // Apresenta a view de confirmação de exclusão.
        return View(marcaViewModel);
    }

    /// <summary>
    /// Ação HTTP POST que efetivamente realiza a exclusão da marca do sistema.
    /// Utiliza 'ActionName("Excluir")' para permitir que a URL seja 'Excluir', mesmo o método chamando 'ConfirmarExclusao'.
    /// </summary>
    /// <param name="id">O ID da marca que o usuário confirmou que deseja excluir.</param>
    /// <returns>Redireciona para a lista (Index) após a exclusão.</returns>
    [HttpPost, ActionName("Excluir")]
    public async Task<IActionResult> ConfirmarExclusao(int id)
    {
        // Envia o comando para o serviço excluir a marca com base no ID fornecido.
        await _MarcasService.ExcluirMarca(id);
        
        // Retorna à listagem geral.
        return RedirectToAction(nameof(Index));
    }
}
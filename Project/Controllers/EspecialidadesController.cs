using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using FabysUnha.Services;
using FabysUnha.ViewModels;
using AutoMapper;

namespace FabysUnha.Controllers;

/// <summary>
/// Controlador responsável pelo gerenciamento de Especialidades.
/// </summary>
public class EspecialidadesController : Controller
{
    private readonly IEspecialidadeService _especialidadeService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Construtor que recebe as dependências do serviço e do mapper.
    /// </summary>
    /// <param name="especialidadeService">Serviço com regras de negócio para especialidades</param>
    /// <param name="mapper">Ferramenta de mapeamento de objetos</param>
    public EspecialidadesController(IEspecialidadeService especialidadeService, IMapper mapper)
    {
        _especialidadeService = especialidadeService;
        _mapper = mapper;
    }

    /// <summary>
    /// Interceptador executado antes de cada Action para validar se o usuário tem permissão.
    /// </summary>
    /// <param name="context">Contexto da requisição atual</param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Apenas usuários com perfil "Funcionario" podem acessar estas rotas
        var tipoUsuario = HttpContext.Session.GetString("UsuarioTipo");
        if (tipoUsuario != "Funcionario")
        {
            context.Result = new RedirectToActionResult("Index", "Login", null);
        }
        base.OnActionExecuting(context);
    }

    /// <summary>
    /// Lista todas as especialidades cadastradas.
    /// </summary>
    /// <returns>View Index contendo as especialidades</returns>
    public async Task<IActionResult> Index()
    {
        // Busca as entidades no banco
        var especialidades = await _especialidadeService.ObterTodasEspecialidades();
        // Converte as entidades para ViewModels
        var especialidadesViewModel = _mapper.Map<IEnumerable<EspecialidadeViewModel>>(especialidades);
        return View(especialidadesViewModel);
    }

    /// <summary>
    /// Exibe o formulário para criação de uma nova especialidade.
    /// </summary>
    /// <returns>View de Criação</returns>
    [HttpGet]
    public IActionResult Criar()
    {
        return View(new EspecialidadeViewModel());
    }

    /// <summary>
    /// Processa o formulário de criação de especialidade.
    /// </summary>
    /// <param name="especialidadeViewModel">Dados da nova especialidade</param>
    /// <returns>Redireciona para Index em caso de sucesso</returns>
    [HttpPost]
    public async Task<IActionResult> Criar(EspecialidadeViewModel especialidadeViewModel)
    {
        // Verifica se os campos obrigatórios foram preenchidos corretamente
        if (!ModelState.IsValid) return View(especialidadeViewModel);

        // Mapeia o ViewModel para a Entidade correspondente
        var especialidade = _mapper.Map<Models.Especialidades>(especialidadeViewModel);
        // Persiste a entidade no banco de dados
        await _especialidadeService.CriarEspecialidade(especialidade);
        
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Exibe o formulário de edição para uma especialidade existente.
    /// </summary>
    /// <param name="id">ID da especialidade</param>
    /// <returns>View de Edição com os dados carregados</returns>
    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        // Obtém a especialidade pelo ID
        var especialidade = await _especialidadeService.ObterEspecialidadePorId(id);
        if (especialidade == null) return NotFound();

        // Mapeia a Entidade para ViewModel para exibir na tela
        var viewModel = _mapper.Map<EspecialidadeViewModel>(especialidade);
        return View(viewModel);
    }

    /// <summary>
    /// Processa as alterações enviadas no formulário de edição.
    /// </summary>
    /// <param name="id">ID da especialidade recebida pela URL</param>
    /// <param name="viewModel">Dados editados da especialidade</param>
    /// <returns>Redireciona para Index em caso de sucesso</returns>
    [HttpPost]
    public async Task<IActionResult> Editar(int id, EspecialidadeViewModel viewModel)
    {
        // Proteção contra alteração forçada do ID no form HTML
        if (id != viewModel.Id) return BadRequest();
        
        // Verifica as validações configuradas no ViewModel
        if (!ModelState.IsValid) return View(viewModel);

        // Mapeia e atualiza no banco de dados
        var especialidade = _mapper.Map<Models.Especialidades>(viewModel);
        await _especialidadeService.AtualizarEspecialidade(especialidade);
        
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Exibe a tela de confirmação de exclusão de uma especialidade.
    /// </summary>
    /// <param name="id">ID da especialidade</param>
    /// <returns>View de Confirmação de Exclusão</returns>
    [HttpGet]
    public async Task<IActionResult> Excluir(int id)
    {
        var especialidade = await _especialidadeService.ObterEspecialidadePorId(id);
        if (especialidade == null) return NotFound();

        var viewModel = _mapper.Map<EspecialidadeViewModel>(especialidade);
        return View(viewModel);
    }

    /// <summary>
    /// Conclui a exclusão da especialidade selecionada.
    /// </summary>
    /// <param name="id">ID da especialidade</param>
    /// <returns>Redireciona para Index em caso de sucesso</returns>
    [HttpPost, ActionName("Excluir")]
    public async Task<IActionResult> ExcluirConfirmado(int id)
    {
        // Deleta (ou inativa) a especialidade através do serviço
        await _especialidadeService.ExcluirEspecialidade(id);
        return RedirectToAction(nameof(Index));
    }
}
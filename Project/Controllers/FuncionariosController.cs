using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using FabysUnha.Services;
using FabysUnha.ViewModels;
using AutoMapper;

namespace FabysUnha.Controllers;

/// <summary>
/// Controlador responsável pelo gerenciamento de Funcionários.
/// Inclui operações de listagem, criação, edição e exclusão (CRUD).
/// </summary>
public class FuncionariosController : Controller
{
    // Injeção de dependências para os serviços e AutoMapper.
    private readonly IFuncionariosService _funcionariosService;
    private readonly IEspecialidadeService _especialidadeService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Construtor que recebe as dependências necessárias para o funcionamento do controlador.
    /// </summary>
    /// <param name="funcionariosService">Serviço com a regra de negócio de funcionários.</param>
    /// <param name="especialidadeService">Serviço com a regra de negócio de especialidades.</param>
    /// <param name="mapper">Ferramenta para mapear entre Entidades do Banco e ViewModels (telas).</param>
    public FuncionariosController(
        IFuncionariosService funcionariosService,
        IEspecialidadeService especialidadeService,
        IMapper mapper)
    {
        _funcionariosService = funcionariosService;
        _especialidadeService = especialidadeService;
        _mapper = mapper;
    }

    /// <summary>
    /// Método executado ANTES de qualquer Ação (Action) deste controlador.
    /// Usado aqui como um "Filtro de Autorização" simples para garantir que apenas Funcionários acessem.
    /// </summary>
    /// <param name="context">Contexto da execução atual, permite ler a sessão ou redirecionar.</param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Verifica na sessão se quem está acessando é realmente um funcionário.
        var tipoUsuario = HttpContext.Session.GetString("UsuarioTipo");
        if (tipoUsuario != "Funcionario")
        {
            // Se não for, interrompe o fluxo normal e redireciona o usuário para a tela de Login.
            context.Result = new RedirectToActionResult("Index", "Login", null);
        }
        
        // Continua com a execução padrão caso a validação passe.
        base.OnActionExecuting(context);
    }

    /// <summary>
    /// Ação que exibe a lista de todos os funcionários cadastrados.
    /// </summary>
    /// <returns>Uma View contendo a lista (ViewModel) de funcionários.</returns>
    public async Task<IActionResult> Index()
    {
        // Busca todos os funcionários através do serviço.
        var funcionarios = await _funcionariosService.ObterTodosFuncionarios();
        
        // Converte (mapeia) a lista de entidades 'Funcionarios' para uma lista de 'FuncionarioListagemViewModel'.
        // Isso é feito para enviar à View apenas os dados necessários para exibição.
        var funcionariosViewModel = _mapper.Map<IEnumerable<FuncionarioListagemViewModel>>(funcionarios);
        
        return View(funcionariosViewModel);
    }

    /// <summary>
    /// Exibe os detalhes de um funcionário específico.
    /// </summary>
    /// <param name="id">O identificador único do funcionário.</param>
    /// <returns>A View de detalhes ou a página de "Não Encontrado" (404) caso o ID seja inválido.</returns>
    [HttpGet]
    public async Task<IActionResult> Detalhes(int id)
    {
        // Busca o funcionário pelo ID.
        var funcionario = await _funcionariosService.ObterFuncionarioPorId(id);
        
        // Se o funcionário não existir no banco de dados, retorna erro 404 (Not Found).
        if (funcionario == null) return NotFound();

        // Mapeia a entidade encontrada para um ViewModel específico de Detalhes.
        var funcionarioViewModel = _mapper.Map<FuncionarioDetalhesViewModel>(funcionario);
        
        return View(funcionarioViewModel);
    }

    /// <summary>
    /// Exibe o formulário vazio para criação de um novo funcionário.
    /// </summary>
    /// <returns>A View contendo o formulário de registro.</returns>
    [HttpGet]
    public async Task<IActionResult> Criar()
    {
        // Instancia um novo ViewModel vazio para a tela.
        var funcionarioViewModel = new FuncionarioRegistroViewModel();

        // Alimenta a lista de especialidades (dropdown/select) no ViewModel.
        // É necessário para que o usuário possa escolher uma especialidade válida na tela.
        await PrepararEspecialidadesAsync(funcionarioViewModel);

        return View(funcionarioViewModel);
    }

    /// <summary>
    /// Recebe e processa os dados do formulário para salvar um novo funcionário no banco de dados.
    /// </summary>
    /// <param name="funcionarioViewModel">Os dados preenchidos no formulário (ViewModel).</param>
    /// <returns>Redireciona para a lista (Index) se der sucesso, ou retorna o formulário com os erros se falhar.</returns>
    [HttpPost]
    public async Task<IActionResult> Criar(FuncionarioRegistroViewModel funcionarioViewModel)
    {
        // Antes de validar, recarregamos a lista de especialidades, 
        // pois se a página recarregar (devido a um erro), o dropdown precisará estar preenchido novamente.
        await PrepararEspecialidadesAsync(funcionarioViewModel);

        // Verifica se os dados preenchidos na View atendem a todas as validações (DataAnnotations do ViewModel).
        if (!ModelState.IsValid) return View(funcionarioViewModel);

        try
        {
            // Validação manual da senha, garantindo que não seja vazia ou apenas espaços.
            if (string.IsNullOrWhiteSpace(funcionarioViewModel.Senha))
            {
                ModelState.AddModelError(nameof(funcionarioViewModel.Senha), "A senha é obrigatória.");
                return View(funcionarioViewModel);
            }

            // Mapeia o ViewModel (dados da tela) para a entidade Funcionario (formato de banco de dados).
            var funcionario = _mapper.Map<Models.Funcionarios>(funcionarioViewModel);
            
            // Registra o funcionário chamando a regra de negócio no serviço.
            await _funcionariosService.RegistrarFuncionario(funcionario);
            
            // Redireciona o usuário para a página inicial de funcionários (Index).
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Se algo der errado (ex: erro de banco de dados), adicionamos o erro no ModelState
            // para ser exibido na tela, e retornamos o formulário para o usuário tentar de novo.
            ModelState.AddModelError(string.Empty, $"Erro ao registrar funcionário: {ex.Message}");
            await PrepararEspecialidadesAsync(funcionarioViewModel);
            return View(funcionarioViewModel);
        }
    }

    /// <summary>
    /// Exibe o formulário preenchido com os dados de um funcionário para edição.
    /// </summary>
    /// <param name="id">O ID do funcionário a ser editado.</param>
    /// <returns>A View de edição preenchida.</returns>
    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        // Busca o funcionário pelo ID.
        var funcionario = await _funcionariosService.ObterFuncionarioPorId(id);
        
        // Se não encontrar, retorna 404 (Not Found).
        if (funcionario == null) return NotFound();

        // Mapeia os dados da entidade para o ViewModel de Edição (para popular a tela).
        var funcionarioViewModel = _mapper.Map<FuncionarioEditarViewModel>(funcionario);

        // Prepara as opções do dropdown de especialidades (incluindo a especialidade atual do funcionário).
        await PrepararEspecialidadesAsync(funcionarioViewModel);
        
        return View(funcionarioViewModel);
    }

    /// <summary>
    /// Recebe os dados alterados do formulário e atualiza o funcionário no banco de dados.
    /// </summary>
    /// <param name="id">O ID do funcionário vindo pela URL (para garantir segurança/consistência).</param>
    /// <param name="funcionarioViewModel">O modelo contendo as novas informações.</param>
    /// <returns>Redireciona para a lista se der sucesso, ou retorna ao formulário com os erros.</returns>
    [HttpPost]
    public async Task<IActionResult> Editar(int id, FuncionarioEditarViewModel funcionarioViewModel)
    {
        // Verifica se o ID passado na URL corresponde ao ID presente no formulário enviado.
        // Impede que um usuário edite o funcionário 1 submetendo um formulário do funcionário 2.
        if (id != funcionarioViewModel.Id) return BadRequest();

        // Recarrega as especialidades para o caso de precisarmos devolver a View com erros.
        await PrepararEspecialidadesAsync(funcionarioViewModel);

        // Verifica as anotações de validação (Required, StringLength, etc) definidas no ViewModel.
        if (!ModelState.IsValid) return View(funcionarioViewModel);

        try
        {
            // Checa na requisição atual se o campo "Status" foi de fato enviado no formulário.
            // Isso ajuda a saber se precisamos atualizar o status ou manter o anterior.
            bool hasStatusUpdate = Request.HasFormContentType && Request.Form.ContainsKey(nameof(funcionarioViewModel.Status));

            // Transforma os dados da View para o modelo de Entidade do banco de dados.
            var funcionario = _mapper.Map<Models.Funcionarios>(funcionarioViewModel);
            
            // Chama o serviço responsável por persistir as alterações.
            await _funcionariosService.AtualizarFuncionario(funcionario, hasStatusUpdate);
            
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Caso ocorra uma falha (ex: regra de negócio quebrada ou falha de conexão), captura a exceção,
            // adiciona a mensagem ao ModelState e devolve a View.
            ModelState.AddModelError(string.Empty, $"Erro ao atualizar funcionário: {ex.Message}");
            await PrepararEspecialidadesAsync(funcionarioViewModel);
            return View(funcionarioViewModel);
        }
    }

    /// <summary>
    /// Exibe a tela de confirmação de exclusão de um funcionário.
    /// </summary>
    /// <param name="id">ID do funcionário a ser excluído.</param>
    /// <returns>View de confirmação com os detalhes do funcionário.</returns>
    [HttpGet]
    public async Task<IActionResult> Excluir(int id)
    {
        // Busca o registro.
        var funcionario = await _funcionariosService.ObterFuncionarioPorId(id);
        if (funcionario == null) return NotFound();

        // Mapeia para um ViewModel de detalhes apenas para exibir quem será excluído.
        var funcionarioViewModel = _mapper.Map<FuncionarioDetalhesViewModel>(funcionario);
        return View(funcionarioViewModel);
    }

    /// <summary>
    /// Efetiva a exclusão do funcionário no banco de dados.
    /// Ação do tipo POST para garantir que modificações destrutivas não sejam feitas via GET.
    /// </summary>
    /// <param name="id">O ID do funcionário.</param>
    /// <returns>Redireciona para a lista de funcionários.</returns>
    [HttpPost, ActionName("Excluir")]
    public async Task<IActionResult> ExcluirConfirmado(int id)
    {
        // Solicita ao serviço que apague o registro do funcionário especificado.
        await _funcionariosService.ExcluirFuncionario(id);
        
        // Volta para a tela inicial de listagem.
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Método auxiliar privado para buscar todas as especialidades ativas e montar o dropdown (SelectList)
    /// usado na tela de CRIAÇÃO.
    /// </summary>
    /// <param name="viewModel">O ViewModel que receberá a lista.</param>
    private async Task PrepararEspecialidadesAsync(FuncionarioRegistroViewModel viewModel)
    {
        // Pede ao serviço a lista completa de especialidades, e filtra apenas as que possuem Status == Ativo.
        var ativas = (await _especialidadeService.ObterTodasEspecialidades())
            .Where(e => e.Status == FabysUnha.Enums.EspecialidadeStatus.Ativo)
            .ToList();

        // Monta um SelectList (utilizado na tag <select> do HTML) informando qual é a chave (Id) e o valor exibido (Descricao).
        viewModel.EspecialidadesList = new SelectList(
            ativas,
            nameof(Models.Especialidades.Id),
            nameof(Models.Especialidades.Descricao),
            viewModel.EspecialidadeId);
    }

    /// <summary>
    /// Método auxiliar privado para buscar as especialidades e montar o dropdown usado na tela de EDIÇÃO.
    /// Diferente da criação, aqui precisamos incluir a especialidade atual do funcionário, mesmo que ela 
    /// tenha sido desativada posteriormente (para que o valor não se perca se ele não for alterado).
    /// </summary>
    /// <param name="viewModel">O ViewModel que receberá a lista.</param>
    private async Task PrepararEspecialidadesAsync(FuncionarioEditarViewModel viewModel)
    {
        // Filtra especialidades ativas OU a especialidade que o funcionário já possui atualmente (viewModel.EspecialidadeId).
        var ativasEAtual = (await _especialidadeService.ObterTodasEspecialidades())
            .Where(e => e.Status == FabysUnha.Enums.EspecialidadeStatus.Ativo || e.Id == viewModel.EspecialidadeId)
            .ToList();

        // Monta o SelectList.
        viewModel.EspecialidadesList = new SelectList(
            ativasEAtual,
            nameof(Models.Especialidades.Id),
            nameof(Models.Especialidades.Descricao),
            viewModel.EspecialidadeId);
    }
}
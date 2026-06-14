using FabysUnha.Models;
using FabysUnha.Repositories;
using FabysUnha.ViewModels;
using FabysUnha.Enums;

namespace FabysUnha.Services;

/// <summary>
/// Serviço de regras de negócio responsável por orquestrar a lógica complexa de Agendamentos.
/// Este serviço interage com vários repositórios para compor um agendamento com serviços, produtos, clientes e funcionários.
/// </summary>
public class AgendamentosService : IAgendamentosService
{
    // Declaração das dependências (Repositórios) que o serviço utilizará para acessar o banco de dados.
    private readonly IAgendamentosRepository _agendamentosRepository;
    private readonly IClientesRepository _clientesRepository;
    private readonly IFuncionariosRepository _funcionariosRepository;
    private readonly IServicosRepository _servicosRepository; 
    private readonly IProdutosRepository _produtosRepository;

    /// <summary>
    /// Construtor que recebe os repositórios via Injeção de Dependência.
    /// Facilita o baixo acoplamento e ajuda em possíveis testes unitários da aplicação.
    /// </summary>
    public AgendamentosService(
        IAgendamentosRepository agendamentosRepository, 
        IClientesRepository clientesRepository,
        IFuncionariosRepository funcionariosRepository,
        IServicosRepository servicosRepository, 
        IProdutosRepository produtosRepository)
    {
        // Atribuição de instâncias das dependências aos campos privados da classe
        _agendamentosRepository = agendamentosRepository;
        _clientesRepository = clientesRepository;
        _funcionariosRepository = funcionariosRepository;
        _servicosRepository = servicosRepository;
        _produtosRepository = produtosRepository;
    }

    /// <summary>
    /// Obtém a lista completa de todos os agendamentos registrados na base de dados.
    /// </summary>
    /// <returns>Uma lista iterável de todos os agendamentos do salão.</returns>
    public async Task<IEnumerable<Agendamentos>> ObterTodosAgendamentos()
    {
        // Repassa a chamada para o repositório especializado de agendamentos
        return await _agendamentosRepository.ObterTodosAgendamentos();
    }

    /// <summary>
    /// Busca um agendamento específico com base em seu número identificador (ID / Nr).
    /// </summary>
    /// <param name="nr">O número (ID) correspondente ao agendamento a ser encontrado.</param>
    /// <returns>A entidade de agendamento em questão ou nulo se não achado.</returns>
    public async Task<Agendamentos?> ObterAgendamentoPorId(int nr)
    {
        // Usa o repositório para localizar o agendamento individual pelo ID
        return await _agendamentosRepository.ObterAgendamentoPorId(nr);
    }

    /// <summary>
    /// Traz do banco todos os agendamentos ligados a um determinado ID de cliente.
    /// Útil para a área restrita do cliente ver seu próprio histórico de atendimento.
    /// </summary>
    /// <param name="clienteId">ID do cliente que desejamos pesquisar o histórico.</param>
    /// <returns>Lista de agendamentos filtrada para o cliente correspondente.</returns>
    public async Task<IEnumerable<Agendamentos>> ObterAgendamentosPorCliente(int clienteId)
    {
        // Obtém todos os agendamentos da base para poder filtrar
        var agendamentos = await _agendamentosRepository.ObterTodosAgendamentos();
        
        // Aplica um filtro LINQ filtrando somente os agendamentos que contêm o ClienteId igual ao solicitado
        return agendamentos.Where(a => a.ClienteId == clienteId);
    }

    /// <summary>
    /// Responsável por processar a lógica de negócios da criação de um novo agendamento a partir de um ViewModel.
    /// Faz o mapeamento e validações antes de salvar.
    /// </summary>
    /// <param name="viewModel">Objeto de transferência de dados que contém as informações advindas do formulário da tela.</param>
    /// <returns>Tarefa representando a operação de persistência dos dados.</returns>
    public async Task CriarAgendamento(AgendamentoRegistroViewModel viewModel)
    {
        // Valida se o cliente existe; o método chamado retornará uma exceção se não existir
        var cliente = await ObterClienteObrigatorioAsync(viewModel.ClienteId);

        // Inicializa uma nova entidade de Agendamentos baseada nos dados contidos na ViewModel
        var agendamento = new Agendamentos
        {
            ClienteId = cliente.Id, // Atrela o cliente verificado ao agendamento
            Data = viewModel.DataHora,
            Status = viewModel.Status,
            Total = 0 // Inicializa o valor total zerado; este valor será incrementado na adição de serviços/produtos
        };

        // Adiciona de forma relacional os serviços e produtos escolhidos no ViewModel para as propriedades do agendamento
        await AdicionarServicosAsync(agendamento, viewModel.ServicosSelecionados);
        await AdicionarProdutosAsync(agendamento, viewModel.ProdutosSelecionados);

        // Regra de Negócio: Não permite salvar um agendamento vazio (total nulo/zero)
        if (agendamento.Total <= 0)
            throw new ArgumentException("O agendamento deve conter pelo menos um serviço ou produto.");

        // Repassa a entidade populada e validada para o repositório fazer o insert no banco
        await _agendamentosRepository.CriarAgendamento(agendamento);
    }

    /// <summary>
    /// Atualiza por completo um agendamento existente no sistema.
    /// Limpa as coleções antigas e reprocessa a adição de serviços/produtos com base no que foi modificado pelo usuário.
    /// </summary>
    /// <param name="viewModel">ViewModel contendo os dados alterados na view de edição de agendamentos.</param>
    /// <returns>Ação de atualização.</returns>
    public async Task AtualizarAgendamento(AgendamentoEditarViewModel viewModel)
    {
        // O primeiro passo é recuperar o agendamento que pretendemos editar pelo ID
        var agendamento = await _agendamentosRepository.ObterAgendamentoPorId(viewModel.Nr);
        
        // Se o banco não devolveu o agendamento, estouramos um erro pois a entidade deve existir para ser atualizada
        if (agendamento == null)
            throw new ArgumentException("Agendamento não encontrado.");

        // Anula temporariamente o relacionamento instanciado do Cliente para impedir conflitos no Entity Framework durante update
        agendamento.Cliente = null;
        
        // Valida se o ID de cliente passado para alteração pertence a um cliente válido
        await ObterClienteObrigatorioAsync(viewModel.ClienteId);

        // Atualiza as propriedades principais da entidade Agendamento que foi recuperada do banco
        agendamento.ClienteId = viewModel.ClienteId;
        agendamento.Data = viewModel.DataHora;
        agendamento.Status = viewModel.Status;
        agendamento.Total = 0; // Reiniciamos o total em zero para recalcularmos através das novas escolhas

        // Limpa (Remove) todos os serviços e produtos que estavam agendados anteriormente 
        // para inserir apenas os novos enviados pelo formulário (ViewModel)
        agendamento.Servicos_Agendados.Clear();
        agendamento.Produtos_Agendados.Clear();

        // Reprocessa as adições nas coleções de Serviços e Produtos usando a mesma lógica que é aplicada ao criar
        await AdicionarServicosAsync(agendamento, viewModel.ServicosSelecionados);
        await AdicionarProdutosAsync(agendamento, viewModel.ProdutosSelecionados);

        // Assim como no método Criar, checamos se no fim do preenchimento o agendamento tem pelo menos algum item
        if (agendamento.Total <= 0)
            throw new ArgumentException("O agendamento deve conter pelo menos um serviço ou produto.");

        // Finalizada a atualização das propriedades e relacionamentos em memória, dispara a persistência com o repositório
        await _agendamentosRepository.AtualizarAgendamento(agendamento);
    }

    /// <summary>
    /// Altera o status de um agendamento para 'Cancelado'.
    /// </summary>
    /// <param name="nr">ID ou número do agendamento a ser invalidado.</param>
    /// <returns>Ação de persistência no repositório.</returns>
    public async Task CancelarAgendamento(int nr)
    {
        // Busca o agendamento que se planeja cancelar
        var agendamento = await _agendamentosRepository.ObterAgendamentoPorId(nr);
        if (agendamento == null) throw new ArgumentException("Agendamento não encontrado.");

        // Modifica a propriedade Status apontando para a opção 'Cancelado' provida pelo enum correspondente
        agendamento.Status = AgendamentoStatus.Cancelado;
        
        // Executa um update simples salvando o status no banco de dados
        await _agendamentosRepository.AtualizarAgendamento(agendamento);
    }

    /// <summary>
    /// Dá baixa em um agendamento, marcando seu status como 'Finalizado'.
    /// </summary>
    /// <param name="nr">Número/ID do agendamento que foi completado.</param>
    /// <returns>Execução de atualização.</returns>
    public async Task FinalizarAgendamento(int nr)
    {
        // Busca a entidade de agendamento usando seu código numérico
        var agendamento = await _agendamentosRepository.ObterAgendamentoPorId(nr);
        if (agendamento == null) throw new ArgumentException("Agendamento não encontrado.");

        // Altera o estado (Status) indicando que o atendimento já foi concluído (Finalizado)
        agendamento.Status = AgendamentoStatus.Finalizado;
        
        // Aplica o salvamento da alteração no repositório de agendamentos
        await _agendamentosRepository.AtualizarAgendamento(agendamento);
    }

    /// <summary>
    /// Deleta por completo um agendamento e seus vínculos do banco de dados de maneira permanente.
    /// </summary>
    /// <param name="nr">Identificador principal do agendamento.</param>
    /// <returns>Ação de exclusão.</returns>
    public async Task ExcluirAgendamento(int nr)
    {
        // Encontra o registro exato do agendamento antes de apagar
        var agendamento = await _agendamentosRepository.ObterAgendamentoPorId(nr);
        if (agendamento == null) throw new ArgumentException("Agendamento não encontrado.");

        // Ordena ao repositório que proceda com a deleção física ou lógica definida por ele
        await _agendamentosRepository.ExcluirAgendamento(agendamento);
    }

    /// <summary>
    /// Método auxiliar privado responsável por converter os dados dos serviços de uma ViewModel em instâncias que o Entity Framework consiga salvar.
    /// Adiciona cada serviço associado na propriedade de navegação Servicos_Agendados e soma ao valor total do Agendamento.
    /// </summary>
    /// <param name="agendamento">A entidade principal Agendamento que está sendo preparada.</param>
    /// <param name="servicosSelecionados">Coleção de serviços selecionados na interface do usuário.</param>
    private async Task AdicionarServicosAsync(
        Agendamentos agendamento,
        IEnumerable<ServicoAgendadoViewModel>? servicosSelecionados)
    {
        // Caso não haja serviços a adicionar, apenas retorna e interrompe a execução
        if (servicosSelecionados == null)
            return;

        // Itera sobre a lista de ViewModel dos serviços selecionados no momento do cadastro ou edição
        foreach (var item in servicosSelecionados)
        {
            // Valida as integridades checando se os respectivos registros de Serviço e Funcionário existem na base
            var servicoDb = await ObterServicoObrigatorioAsync(item.ServicoId);
            var funcionarioDb = await ObterFuncionarioObrigatorioAsync(item.FuncionarioId);

            // Popula a tabela associativa Servicos_Agendados e adiciona à coleção do agendamento
            agendamento.Servicos_Agendados.Add(new Servicos_Agendados
            {
                ServicoId = servicoDb.Id,
                FuncionarioId = funcionarioDb.Id,
                // Converte a string de horário vinda da ViewModel para o tipo TimeSpan; se falhar, usa Zero
                Horario = TimeSpan.TryParse(item.Horario, out var ts) ? ts : TimeSpan.Zero,
                // Armazena observações retirando espaços no início e final usando Trim(); ou string vazia caso seja nulo
                Obs = item.Obs?.Trim() ?? string.Empty,
                Valor = servicoDb.Preco // O valor unitário salvo no relacionamento é recuperado a partir dos dados no banco (servicoDb)
            });

            // Incrementa o valor total do agendamento em si somando o valor do serviço processado nesta iteração
            agendamento.Total += servicoDb.Preco;
        }
    }

    /// <summary>
    /// Método auxiliar (privado) usado para mapear os produtos escolhidos no ViewModel para entidades do sistema.
    /// Adiciona à lista de Produtos_Agendados em Agendamentos e aumenta o montante total.
    /// </summary>
    /// <param name="agendamento">Agendamento em processamento atual.</param>
    /// <param name="produtosSelecionados">Os dados dos produtos selecionados fornecidos pela UI/ViewModel.</param>
    private async Task AdicionarProdutosAsync(
        Agendamentos agendamento,
        IEnumerable<ProdutoAgendadoViewModel>? produtosSelecionados)
    {
        // Se a coleção de produtos estiver vazia/nula, nada precisa ser executado.
        if (produtosSelecionados == null)
            return;

        // Varre cada produto selecionado na interface
        foreach (var item in produtosSelecionados)
        {
            // Confirma que os registros de Produto e Serviço atrelado existem no banco de dados para evitar foreign key constraint errors
            var produtoDb = await ObterProdutoObrigatorioAsync(item.ProdutoCodigo);
            var servicoDb = await ObterServicoObrigatorioAsync(item.ServicoId);

            // Cria um novo relacionamento entre o Produto, o Serviço e o Agendamento adicionando-o à lista
            agendamento.Produtos_Agendados.Add(new Produtos_Agendados
            {
                ProdutoCodigo = produtoDb.Codigo,
                ServicoId = servicoDb.Id,
                Preco = produtoDb.Preco // O preço gravado no agendamento provém do preço vigente no cadastro do Produto
            });

            // Faz a soma iterativa do preço do produto atual ao total cobrado no agendamento final
            agendamento.Total += produtoDb.Preco;
        }
    }

    /// <summary>
    /// Método auxiliar utilitário para validar e devolver um Cliente garantindo a sua existência.
    /// Caso contrário, levanta uma exceção indicando inconsistência.
    /// </summary>
    /// <param name="clienteId">Id do cliente em verificação.</param>
    /// <returns>O objeto validado contendo os dados de Clientes.</returns>
    private async Task<Clientes> ObterClienteObrigatorioAsync(int clienteId)
    {
        // Pede a informação do cliente ao repositório
        var cliente = await _clientesRepository.ObterClientePorId(clienteId);
        
        // Verifica falha na busca e impede que a operação do serviço prossiga caso seja inválido
        if (cliente == null)
            throw new ArgumentException("Cliente inválido para o agendamento.");

        // Retorna a entidade válida achada
        return cliente;
    }

    /// <summary>
    /// Método validador que busca um Funcionário e dispara uma exceção no caso de não o encontrar.
    /// Garante que o agendamento sempre vincule um profissional existente.
    /// </summary>
    /// <param name="funcionarioId">ID do profissional responsável.</param>
    /// <returns>A entidade Funcionário correspondente.</returns>
    private async Task<Funcionarios> ObterFuncionarioObrigatorioAsync(int funcionarioId)
    {
        // Chama o repositório de funcionários buscando pelo ID fornecido
        var funcionario = await _funcionariosRepository.ObterFuncionarioPorId(funcionarioId);
        
        // Aciona o erro caso a entidade não tenha retornado nada (seja null)
        if (funcionario == null)
            throw new ArgumentException($"Funcionário {funcionarioId} não encontrado.");

        return funcionario;
    }

    /// <summary>
    /// Verifica a existência de um determinado serviço na base dados antes de agendá-lo.
    /// Retorna erro claro se o ID do serviço for incorreto ou inexistente.
    /// </summary>
    /// <param name="servicoId">O ID do serviço sendo validado.</param>
    /// <returns>Uma entidade da classe Servicos que representa o serviço válido.</returns>
    private async Task<Servicos> ObterServicoObrigatorioAsync(int servicoId)
    {
        // Obtém o serviço diretamente através da conexão do seu respectivo repositório
        var servico = await _servicosRepository.ObterServicoPorId(servicoId);
        
        // Lança falha se a verificação retornar vazio (null)
        if (servico == null)
            throw new ArgumentException($"Serviço {servicoId} não encontrado.");

        return servico;
    }

    /// <summary>
    /// Executa uma verificação para saber se o produto a ser vendido / agendado consta no banco e é real.
    /// Falha a transação se o código não for encontrado.
    /// </summary>
    /// <param name="produtoCodigo">A chave primária (código) do produto.</param>
    /// <returns>Objeto da classe Produtos contendo os detalhes do item.</returns>
    private async Task<Produtos> ObterProdutoObrigatorioAsync(int produtoCodigo)
    {
        // Faz requisição de busca do produto específico utilizando o código identificador
        var produto = await _produtosRepository.ObterProdutoPorId(produtoCodigo);
        
        // Se este objeto for nulo, um Exception com uma string explícita é lançada
        if (produto == null)
            throw new ArgumentException($"Produto {produtoCodigo} não encontrado.");

        return produto;
    }

    /// <summary>
    /// Processo de Criação de Agendamentos específico para quando as ações são disparadas pela perspectiva / painel do próprio Cliente.
    /// Possui regras de estruturação particulares para mapear horários em serviços e produtos acoplados a estes serviços.
    /// </summary>
    /// <param name="viewModel">Objeto de ViewModel com o contexto das escolhas feitas pelo usuário (Cliente).</param>
    /// <param name="clienteId">O identificador do cliente fazendo a requisição, comumente vindo dos claims de Autenticação.</param>
    /// <returns>Uma ação indicando que o processo finalizou no repositório.</returns>
    public async Task CriarAgendamentoCliente(AgendamentoClienteViewModel viewModel, int clienteId)
    {
        // 1. Confirma que o cliente que tenta fazer o agendamento tem um registro idôneo
        var clienteExistente = await _clientesRepository.ObterClientePorId(clienteId);
        if (clienteExistente == null)
            throw new ArgumentException("Cliente inválido para o agendamento.");

        // 2. Validar se a lista de serviços enviada está vazia, o que proíbe o processo de seguir
        if (viewModel.Servicos == null || viewModel.Servicos.Count == 0)
            throw new ArgumentException("Selecione pelo menos um serviço.");

        // Tenta capturar o primeiro horário preenchido dentre os serviços como base para marcar a hora do agendamento principal
        var primeiraHoraStr = viewModel.Servicos.FirstOrDefault()?.Horario;
        var primeiraHora = TimeSpan.TryParse(primeiraHoraStr, out var ts) ? ts : TimeSpan.Zero;

        // 3. Montar a entidade base (Agendamento) instanciando um novo objeto com os dados globais.
        var agendamento = new Agendamentos
        {
            ClienteId = clienteId,
            // A data do atendimento combina o 'Date' (sem horas) advindo do form com a hora avaliada na variável 'primeiraHora'
            Data = viewModel.Data.Date.Add(primeiraHora),
            Status = Enums.AgendamentoStatus.Pendente, // O agendamento inserido via cliente inicia no modo "Pendente" para que a recepção possa visualizar e aprovar/controlar
            Total = 0 // Inicializamos novamente o valor como 0
        };

        // Itera todos os itens presentes na coleção de serviços advindos do agendamento feito no lado do cliente
        foreach (var servicoItem in viewModel.Servicos)
        {
            // Realiza dupla checagem usando métodos auxiliares de serviço e de funcionário para impedir que dados manipulados pela Web causem problemas internos (corrupção referencial)
            var servicoDb = await ObterServicoObrigatorioAsync(servicoItem.ServicoId);
            var funcionarioDb = await ObterFuncionarioObrigatorioAsync(servicoItem.FuncionarioId);

            // Popula os dados relacional de serviço no escopo do agendamento
            var servicoAgendado = new Servicos_Agendados
            {
                ServicoId = servicoDb.Id,
                FuncionarioId = funcionarioDb.Id,
                // Extrai o horário deste serviço específico do item (se houver variação no fluxo) e o transforma em TimeSpan
                Horario = TimeSpan.TryParse(servicoItem.Horario, out var tsCliente) ? tsCliente : TimeSpan.Zero,
                Obs = servicoItem.Obs?.Trim() ?? string.Empty,
                Valor = servicoDb.Preco // Copia o preço original contido no modelo que veio do banco, em vez de depender de valores postados pela web
            };

            // Anexa este registro na coleção mestre do agendamento e vai somando os valores deste serviço ao preço total do atendimento
            agendamento.Servicos_Agendados.Add(servicoAgendado);
            agendamento.Total += servicoDb.Preco;

            // Tratativa específica dessa ViewModel de clientes que envia Produtos atrelados dentro do objeto de Serviço em questão
            if (servicoItem.ProdutosCodigos != null)
            {
                // Faz a varredura da array de códigos de produtos atrelados
                foreach (var produtoCodigo in servicoItem.ProdutosCodigos)
                {
                    // Validações básicas: pula o laço se o código não for passado ou for numérico inválido (<= 0)
                    if (produtoCodigo == null || produtoCodigo <= 0) continue;

                    // Busca o produto e obriga ele a existir no sistema
                    var produtoDb = await ObterProdutoObrigatorioAsync(produtoCodigo.Value);

                    // Cria um vínculo em Produtos_Agendados fazendo ligação entre Agendamento <-> Servico <-> Produto
                    agendamento.Produtos_Agendados.Add(new Produtos_Agendados
                    {
                        ServicoId = servicoDb.Id, // Define para qual serviço este produto será consumido
                        ProdutoCodigo = produtoDb.Codigo, // Pega do validador DB
                        Preco = produtoDb.Preco // Garante o preço real fixado do cadastro de produto do sistema
                    });

                    // Somando os sub-produtos no custo total
                    agendamento.Total += produtoDb.Preco;
                }
            }
        }

        // Validação final de integridade de valores de negócio, não se pode abrir uma comanda de custo gratuito ou negativo via esta funcionalidade
        if (agendamento.Total <= 0)
            throw new ArgumentException("O agendamento deve conter pelo menos um serviço.");

        // Por fim, transfere ao Agendamentos Repository para persistência efetiva dessa grande cadeia em memória
        await _agendamentosRepository.CriarAgendamento(agendamento);
    }

    /// <summary>
    /// Retorna uma lista de horários livres de um funcionário para um determinado dia.
    /// Esta função ajuda a preencher combobox e seleções nos modais ou formulários.
    /// </summary>
    /// <param name="funcionarioId">O funcionário selecionado (Barbeiro, Manicure, etc.).</param>
    /// <param name="data">Data de análise para exibir os horários.</param>
    /// <returns>Uma lista contendo estruturas do tipo TimeSpan simbolizando os horários vagos do dia.</returns>
    public async Task<List<TimeSpan>> ObterHorariosDisponiveis(int funcionarioId, DateTime data)
    {
        // Encontra no banco todos os horários que o funcionário em questão já está ocupado com clientes na data pedida
        var ocupados = await _agendamentosRepository.ObterHorariosOcupados(funcionarioId, data);

        // Abertura de uma lista contendo a jornada de horário comercial, que será alimentada através de laço. 
        // Horários de funcionamento predeterminados neste cenário de regra de negócio: das 08:00 (8) às 18:00 (17) em faixas padronizadas de hora em hora.
        var todosHorarios = new List<TimeSpan>();
        
        // Loop simples do começo do expediente ao fim (horários fixos estáticos, 8h as 17h, ignorando as 18 porque loop finaliza)
        for (var hora = 8; hora < 18; hora++)
        {
            // Para cada hora iterada, acrescenta um horário na lista mestre de expediente
            todosHorarios.Add(new TimeSpan(hora, 0, 0));
        }

        // Utiliza recurso de filtro do LINQ onde filtra todos os horários montados pelo Loop, removendo da lista todo elemento de horário que constar na lista "ocupados" trazida do repositório
        return todosHorarios.Where(h => !ocupados.Contains(h)).ToList();
    }
}
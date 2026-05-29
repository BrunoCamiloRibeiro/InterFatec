using FabysUnha.Models;
using FabysUnha.Repositories;
using FabysUnha.ViewModels;
using FabysUnha.Enums;

namespace FabysUnha.Services;

public class AgendamentosService : IAgendamentosService
{
    private readonly IAgendamentosRepository _agendamentosRepository;
    private readonly IClientesRepository _clientesRepository;
    private readonly IFuncionariosRepository _funcionariosRepository;
    private readonly IServicosRepository _servicosRepository; 
    private readonly IProdutosRepository _produtosRepository;

    public AgendamentosService(
        IAgendamentosRepository agendamentosRepository, 
        IClientesRepository clientesRepository,
        IFuncionariosRepository funcionariosRepository,
        IServicosRepository servicosRepository, 
        IProdutosRepository produtosRepository)
    {
        _agendamentosRepository = agendamentosRepository;
        _clientesRepository = clientesRepository;
        _funcionariosRepository = funcionariosRepository;
        _servicosRepository = servicosRepository;
        _produtosRepository = produtosRepository;
    }

    public async Task<IEnumerable<Agendamentos>> ObterTodosAgendamentos()
    {
        return await _agendamentosRepository.ObterTodosAgendamentos();
    }

    public async Task<Agendamentos?> ObterAgendamentoPorId(int nr)
    {
        return await _agendamentosRepository.ObterAgendamentoPorId(nr);
    }

    public async Task<IEnumerable<Agendamentos>> ObterAgendamentosPorCliente(int clienteId)
    {
        var agendamentos = await _agendamentosRepository.ObterTodosAgendamentos();
        return agendamentos.Where(a => a.ClienteId == clienteId);
    }

    public async Task CriarAgendamento(AgendamentoRegistroViewModel viewModel)
    {
        var cliente = await ObterClienteObrigatorioAsync(viewModel.ClienteId);

        var agendamento = new Agendamentos
        {
            ClienteId = cliente.Id,
            Data = viewModel.DataHora,
            Status = viewModel.Status,
            Total = 0 
        };

        await AdicionarServicosAsync(agendamento, viewModel.ServicosSelecionados);
        await AdicionarProdutosAsync(agendamento, viewModel.ProdutosSelecionados);

        if (agendamento.Total <= 0)
            throw new ArgumentException("O agendamento deve conter pelo menos um serviço ou produto.");

        await _agendamentosRepository.CriarAgendamento(agendamento);
    }

    public async Task AtualizarAgendamento(AgendamentoEditarViewModel viewModel)
    {
        var agendamento = await _agendamentosRepository.ObterAgendamentoPorId(viewModel.Nr);
        if (agendamento == null)
            throw new ArgumentException("Agendamento não encontrado.");

        agendamento.Cliente = null;
        await ObterClienteObrigatorioAsync(viewModel.ClienteId);

        agendamento.ClienteId = viewModel.ClienteId;
        agendamento.Data = viewModel.DataHora;
        agendamento.Status = viewModel.Status;
        agendamento.Total = 0; 

        agendamento.Servicos_Agendados.Clear();
        agendamento.Produtos_Agendados.Clear();

        await AdicionarServicosAsync(agendamento, viewModel.ServicosSelecionados);
        await AdicionarProdutosAsync(agendamento, viewModel.ProdutosSelecionados);

        if (agendamento.Total <= 0)
            throw new ArgumentException("O agendamento deve conter pelo menos um serviço ou produto.");

        await _agendamentosRepository.AtualizarAgendamento(agendamento);
    }

    public async Task CancelarAgendamento(int nr)
    {
        var agendamento = await _agendamentosRepository.ObterAgendamentoPorId(nr);
        if (agendamento == null) throw new ArgumentException("Agendamento não encontrado.");

        agendamento.Status = AgendamentoStatus.Cancelado;
        await _agendamentosRepository.AtualizarAgendamento(agendamento);
    }

    public async Task ExcluirAgendamento(int nr)
    {
        var agendamento = await _agendamentosRepository.ObterAgendamentoPorId(nr);
        if (agendamento == null) throw new ArgumentException("Agendamento não encontrado.");

        await _agendamentosRepository.ExcluirAgendamento(agendamento);
    }

    private async Task AdicionarServicosAsync(
        Agendamentos agendamento,
        IEnumerable<ServicoAgendadoViewModel>? servicosSelecionados)
    {
        if (servicosSelecionados == null)
            return;

        foreach (var item in servicosSelecionados)
        {
            var servicoDb = await ObterServicoObrigatorioAsync(item.ServicoId);
            var funcionarioDb = await ObterFuncionarioObrigatorioAsync(item.FuncionarioId);

            agendamento.Servicos_Agendados.Add(new Servicos_Agendados
            {
                ServicoId = servicoDb.Id,
                FuncionarioId = funcionarioDb.Id,
                Horario = item.Horario,
                Obs = item.Obs?.Trim() ?? string.Empty,
                Valor = servicoDb.Preco
            });

            agendamento.Total += servicoDb.Preco;
        }
    }

    private async Task AdicionarProdutosAsync(
        Agendamentos agendamento,
        IEnumerable<ProdutoAgendadoViewModel>? produtosSelecionados)
    {
        if (produtosSelecionados == null)
            return;

        foreach (var item in produtosSelecionados)
        {
            var produtoDb = await ObterProdutoObrigatorioAsync(item.ProdutoCodigo);
            var servicoDb = await ObterServicoObrigatorioAsync(item.ServicoId);

            agendamento.Produtos_Agendados.Add(new Produtos_Agendados
            {
                ProdutoCodigo = produtoDb.Codigo,
                ServicoId = servicoDb.Id,
                Preco = produtoDb.Preco
            });

            agendamento.Total += produtoDb.Preco;
        }
    }

    private async Task<Clientes> ObterClienteObrigatorioAsync(int clienteId)
    {
        var cliente = await _clientesRepository.ObterClientePorId(clienteId);
        if (cliente == null)
            throw new ArgumentException("Cliente inválido para o agendamento.");

        return cliente;
    }

    private async Task<Funcionarios> ObterFuncionarioObrigatorioAsync(int funcionarioId)
    {
        var funcionario = await _funcionariosRepository.ObterFuncionarioPorId(funcionarioId);
        if (funcionario == null)
            throw new ArgumentException($"Funcionário {funcionarioId} não encontrado.");

        return funcionario;
    }

    private async Task<Servicos> ObterServicoObrigatorioAsync(int servicoId)
    {
        var servico = await _servicosRepository.ObterServicoPorId(servicoId);
        if (servico == null)
            throw new ArgumentException($"Serviço {servicoId} não encontrado.");

        return servico;
    }

    private async Task<Produtos> ObterProdutoObrigatorioAsync(int produtoCodigo)
    {
        var produto = await _produtosRepository.ObterProdutoPorId(produtoCodigo);
        if (produto == null)
            throw new ArgumentException($"Produto {produtoCodigo} não encontrado.");

        return produto;
    }

    public async Task CriarAgendamentoCliente(AgendamentoClienteViewModel viewModel)
    {
        // 1. Identificar ou criar o cliente pelo telefone
        var telefone = viewModel.Telefone?.Trim() ?? string.Empty;
        var nome = viewModel.Nome?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(telefone))
            throw new ArgumentException("O telefone é obrigatório.");

        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome é obrigatório.");

        var clienteExistente = await _clientesRepository.ObterClientePorTelefone(telefone);
        int clienteId;

        var isGuest = string.IsNullOrWhiteSpace(viewModel.Senha);

        if (clienteExistente != null)
        {
            clienteId = clienteExistente.Id;
            if (string.IsNullOrWhiteSpace(clienteExistente.Senha))
            {
                isGuest = true;
            }
        }
        else
        {
            var novoCliente = new Clientes
            {
                Nome = nome,
                Telefone = telefone,
                Status = Enums.PessoaStatus.Ativo,
                Senha = viewModel.Senha?.Trim() ?? string.Empty
            };
            await _clientesRepository.RegistrarCliente(novoCliente);

            var clienteCriado = await _clientesRepository.ObterClientePorTelefone(telefone);
            if (clienteCriado == null)
                throw new InvalidOperationException("Erro ao registrar o cliente.");

            clienteId = clienteCriado.Id;
        }

        // 2. Validar serviços
        if (viewModel.Servicos == null || viewModel.Servicos.Count == 0)
            throw new ArgumentException("Selecione pelo menos um serviço.");

        // 3. Montar o agendamento
        var agendamento = new Agendamentos
        {
            ClienteId = clienteId,
            Data = viewModel.Data,
            Status = Enums.AgendamentoStatus.Pendente,
            Total = 0
        };

        foreach (var servicoItem in viewModel.Servicos)
        {
            var servicoDb = await ObterServicoObrigatorioAsync(servicoItem.ServicoId);
            var funcionarioDb = await ObterFuncionarioObrigatorioAsync(servicoItem.FuncionarioId);

            var servicoAgendado = new Servicos_Agendados
            {
                ServicoId = servicoDb.Id,
                FuncionarioId = funcionarioDb.Id,
                Horario = servicoItem.Horario,
                Obs = servicoItem.Obs?.Trim() ?? string.Empty,
                Valor = servicoDb.Preco
            };

            agendamento.Servicos_Agendados.Add(servicoAgendado);
            agendamento.Total += servicoDb.Preco;

            // Produtos dentro deste serviço
            if (servicoItem.ProdutosCodigos != null)
            {
                foreach (var produtoCodigo in servicoItem.ProdutosCodigos)
                {
                    if (produtoCodigo <= 0) continue;

                    var produtoDb = await ObterProdutoObrigatorioAsync(produtoCodigo);

                    agendamento.Produtos_Agendados.Add(new Produtos_Agendados
                    {
                        ServicoId = servicoDb.Id,
                        ProdutoCodigo = produtoDb.Codigo,
                        Preco = produtoDb.Preco
                    });

                    agendamento.Total += produtoDb.Preco;
                }
            }
        }

        if (agendamento.Total <= 0)
            throw new ArgumentException("O agendamento deve conter pelo menos um serviço.");

        if (isGuest)
        {
            agendamento.CodigoRastreio = GerarCodigoRastreio();
        }

        await _agendamentosRepository.CriarAgendamento(agendamento);
    }

    /// <summary>
    /// Gera um código de rastreio único no formato FA-XXXX (ex: FA-9832)
    /// </summary>
    private string GerarCodigoRastreio()
    {
        var random = new Random();
        var numero = random.Next(1000, 10000);
        return $"FA-{numero}";
    }

    public async Task<List<TimeSpan>> ObterHorariosDisponiveis(int funcionarioId, DateTime data)
    {
        var ocupados = await _agendamentosRepository.ObterHorariosOcupados(funcionarioId, data);

        // Horários de funcionamento: 08:00 às 18:00, slots de 1 hora
        var todosHorarios = new List<TimeSpan>();
        for (var hora = 8; hora < 18; hora++)
        {
            todosHorarios.Add(new TimeSpan(hora, 0, 0));
        }

        return todosHorarios.Where(h => !ocupados.Contains(h)).ToList();
    }
}
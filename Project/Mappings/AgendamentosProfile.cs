using AutoMapper;
using FabysUnha.Models;
using FabysUnha.ViewModels;

namespace FabysUnha.Mappings;

/// <summary>
/// Perfil de mapeamento do AutoMapper para a entidade de Agendamentos.
/// Responsável por definir como os dados são convertidos entre os Modelos (Entidades) e os ViewModels.
/// </summary>
public class AgendamentosProfile : Profile
{
    /// <summary>
    /// Construtor padrão onde as regras de mapeamento são configuradas.
    /// </summary>
    public AgendamentosProfile()
    {
        // Mapeia a entidade Servicos_Agendados para ServicoAgendadoViewModel.
        // Utiliza o .ForMember para customizar o mapeamento de propriedades específicas.
        CreateMap<Servicos_Agendados, ServicoAgendadoViewModel>()
            // Verifica se o serviço não é nulo antes de extrair a descrição; caso contrário, retorna string vazia.
            .ForMember(dest => dest.ServicoNome, opt => opt.MapFrom(src => src.Servico != null ? src.Servico.Descricao : string.Empty))
            // Verifica se o funcionário não é nulo antes de extrair o nome; caso contrário, retorna string vazia.
            .ForMember(dest => dest.FuncionarioNome, opt => opt.MapFrom(src => src.Funcionario != null ? src.Funcionario.Nome : string.Empty));

        // Mapeia a entidade Produtos_Agendados para ProdutoAgendadoViewModel.
        CreateMap<Produtos_Agendados, ProdutoAgendadoViewModel>()
            // Extrai o nome do produto vinculado, tratando nulos com segurança.
            .ForMember(dest => dest.ProdutoNome, opt => opt.MapFrom(src => src.Produto != null ? src.Produto.Nome : string.Empty))
            // Extrai a descrição do serviço vinculado àquele produto, descendo na hierarquia de navegação com tratamento de nulos.
            .ForMember(dest => dest.ServicoNome, opt => opt.MapFrom(src => src.ServicoAgendado != null && src.ServicoAgendado.Servico != null ? src.ServicoAgendado.Servico.Descricao : string.Empty));

        // Mapeia a entidade principal Agendamentos para o ViewModel usado em listagens.
        CreateMap<Agendamentos, AgendamentoListagemViewModel>()
            // Mapeia o campo Data (modelo) para DataHora (ViewModel).
            .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.Data))
            // O Status é mapeado diretamente da fonte.
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            // Mapeia o nome do cliente associado ao agendamento.
            .ForMember(dest => dest.ClienteNome, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nome : string.Empty))
            // Conta a quantidade de serviços que foram agendados.
            .ForMember(dest => dest.QuantidadeServicos, opt => opt.MapFrom(src => src.Servicos_Agendados.Count))
            // Conta a quantidade de produtos associados ao agendamento.
            .ForMember(dest => dest.QuantidadeProdutos, opt => opt.MapFrom(src => src.Produtos_Agendados.Count));

        // Mapeia Agendamentos para o ViewModel que detalha um agendamento específico.
        CreateMap<Agendamentos, AgendamentoDetalhesViewModel>()
            .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.Data))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.ClienteNome, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nome : string.Empty))
            // Mapeia as coleções aninhadas de serviços e produtos para suas propriedades equivalentes no ViewModel.
            .ForMember(dest => dest.ServicosAgendados, opt => opt.MapFrom(src => src.Servicos_Agendados))
            .ForMember(dest => dest.ProdutosAgendados, opt => opt.MapFrom(src => src.Produtos_Agendados));

        // Mapeia Agendamentos para o ViewModel responsável por criar um novo registro.
        CreateMap<Agendamentos, AgendamentoRegistroViewModel>()
            .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.Data))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            // Mapeia coleções selecionadas a partir do modelo principal para o ViewModel de criação.
            .ForMember(dest => dest.ServicosSelecionados, opt => opt.MapFrom(src => src.Servicos_Agendados))
            .ForMember(dest => dest.ProdutosSelecionados, opt => opt.MapFrom(src => src.Produtos_Agendados));

        // Mapeia Agendamentos para o ViewModel focado em edição de registros existentes.
        CreateMap<Agendamentos, AgendamentoEditarViewModel>()
            // Transporta as chaves primárias e estrangeiras fundamentais para a edição.
            .ForMember(dest => dest.Nr, opt => opt.MapFrom(src => src.Nr))
            .ForMember(dest => dest.ClienteId, opt => opt.MapFrom(src => src.ClienteId))
            .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.Data))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            // Popula os serviços e produtos previamente vinculados para que possam ser modificados na interface.
            .ForMember(dest => dest.ServicosSelecionados, opt => opt.MapFrom(src => src.Servicos_Agendados))
            .ForMember(dest => dest.ProdutosSelecionados, opt => opt.MapFrom(src => src.Produtos_Agendados));
    }
}
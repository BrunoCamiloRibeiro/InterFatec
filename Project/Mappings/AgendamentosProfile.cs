using AutoMapper;
using FabysUnha.Models;
using FabysUnha.ViewModels;

namespace FabysUnha.Mappings;

public class AgendamentosProfile : Profile
{
    public AgendamentosProfile()
    {
        CreateMap<Servicos_Agendados, ServicoAgendadoViewModel>()
            .ForMember(dest => dest.ServicoNome, opt => opt.MapFrom(src => src.Servico != null ? src.Servico.Descricao : string.Empty))
            .ForMember(dest => dest.FuncionarioNome, opt => opt.MapFrom(src => src.Funcionario != null ? src.Funcionario.Nome : string.Empty));

        CreateMap<Produtos_Agendados, ProdutoAgendadoViewModel>()
            .ForMember(dest => dest.ProdutoNome, opt => opt.MapFrom(src => src.Produto != null ? src.Produto.Nome : string.Empty))
            .ForMember(dest => dest.ServicoNome, opt => opt.MapFrom(src => src.Servico != null ? src.Servico.Descricao : string.Empty));

        CreateMap<Agendamentos, AgendamentoListagemViewModel>()
            .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.Data))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.ClienteNome, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nome : string.Empty))
            .ForMember(dest => dest.QuantidadeServicos, opt => opt.MapFrom(src => src.Servicos_Agendados.Count))
            .ForMember(dest => dest.QuantidadeProdutos, opt => opt.MapFrom(src => src.Produtos_Agendados.Count));

        CreateMap<Agendamentos, AgendamentoDetalhesViewModel>()
            .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.Data))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.ClienteNome, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nome : string.Empty))
            .ForMember(dest => dest.ServicosAgendados, opt => opt.MapFrom(src => src.Servicos_Agendados))
            .ForMember(dest => dest.ProdutosAgendados, opt => opt.MapFrom(src => src.Produtos_Agendados));

        CreateMap<Agendamentos, AgendamentoRegistroViewModel>()
            .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.Data))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.ServicosSelecionados, opt => opt.MapFrom(src => src.Servicos_Agendados))
            .ForMember(dest => dest.ProdutosSelecionados, opt => opt.MapFrom(src => src.Produtos_Agendados));

        CreateMap<Agendamentos, AgendamentoEditarViewModel>()
            .ForMember(dest => dest.Nr, opt => opt.MapFrom(src => src.Nr))
            .ForMember(dest => dest.ClienteId, opt => opt.MapFrom(src => src.ClienteId))
            .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.Data))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.ServicosSelecionados, opt => opt.MapFrom(src => src.Servicos_Agendados))
            .ForMember(dest => dest.ProdutosSelecionados, opt => opt.MapFrom(src => src.Produtos_Agendados));
    }
}
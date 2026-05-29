using AutoMapper;
using FabysUnha.Models;
using FabysUnha.ViewModels;

namespace FabysUnha.Mappings;

public class FuncionariosProfile : Profile
{
    public FuncionariosProfile()
    {
        CreateMap<Funcionarios, FuncionarioRegistroViewModel>()
            .ForMember(dest => dest.Senha, opt => opt.MapFrom(src => src.Senha))
            .ReverseMap();

        CreateMap<Funcionarios, FuncionarioDetalhesViewModel>()
            .ForMember(dest => dest.EspecialidadeNome, opt => opt.MapFrom(src => src.Especialidade != null ? src.Especialidade.Descricao : string.Empty))
            .ForMember(dest => dest.ServicosAgendados, opt => opt.MapFrom(src => src.Servicos_Agendados))
            .ReverseMap();

        CreateMap<Funcionarios, FuncionarioListagemViewModel>()
            .ForMember(dest => dest.EspecialidadeNome, opt => opt.MapFrom(src => src.Especialidade != null ? src.Especialidade.Descricao : string.Empty))
            .ReverseMap();
        CreateMap<Funcionarios, FuncionarioEditarViewModel>().ReverseMap();
    }
}
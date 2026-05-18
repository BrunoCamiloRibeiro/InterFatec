using AutoMapper;
using FabysUnha.Models;
using FabysUnha.ViewModels.Servicos;

namespace FabysUnha.Mappings;

public class ServicosProfile : Profile
{
    public ServicosProfile()
    {

        CreateMap<Servicos, ServicoListagemViewModel>()
            .ForMember(dest => dest.PrecoFormatado, opt => opt.MapFrom(src => src.Preco.ToString("C")))
            .ForMember(dest => dest.TempoFormatado, opt => opt.MapFrom(src => src.Tempo.ToString(@"hh\h\ mm\m")));

        CreateMap<Servicos, ServicoDetalhesViewModel>()
            .ForMember(dest => dest.PrecoFormatado, opt => opt.MapFrom(src => src.Preco.ToString("C")))
            .ForMember(dest => dest.TempoFormatado, opt => opt.MapFrom(src => src.Tempo.ToString(@"hh\h\ mm\m")));

        CreateMap<ServicoRegistroViewModel, Servicos>();
        
        CreateMap<ServicoEditarViewModel, Servicos>().ReverseMap();
    }
}
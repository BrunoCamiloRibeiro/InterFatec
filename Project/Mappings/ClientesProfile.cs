using AutoMapper;
using FabysUnha.Models;
using FabysUnha.ViewModels;

namespace FabysUnha.Mappings;

public class ClientesProfile : Profile
{
    public ClientesProfile()
    {
        CreateMap<Clientes, ClienteRegistroViewModel>().ReverseMap();
        CreateMap<Clientes, ClienteDetalhesViewModel>()
            .ForMember(dest => dest.TotalAgendamentos, opt => opt.MapFrom(src => src.Agendamentos.Count))
            .ForMember(dest => dest.DataUltimoAgendamento, opt => opt.MapFrom(src =>
                src.Agendamentos
                    .OrderByDescending(agendamento => agendamento.Data)
                    .Select(agendamento => (DateTime?)agendamento.Data)
                    .FirstOrDefault()))
            .ReverseMap();
        CreateMap<Clientes, ClienteListagemViewModel>()
            .ForMember(dest => dest.TotalAgendamentos, opt => opt.MapFrom(src => src.Agendamentos.Count))
            .ForMember(dest => dest.DataUltimoAgendamento, opt => opt.MapFrom(src =>
                src.Agendamentos
                    .OrderByDescending(agendamento => agendamento.Data)
                    .Select(agendamento => (DateTime?)agendamento.Data)
                    .FirstOrDefault()))
            .ReverseMap();
        CreateMap<Clientes, ClienteEditarViewModel>().ReverseMap();
    }
}
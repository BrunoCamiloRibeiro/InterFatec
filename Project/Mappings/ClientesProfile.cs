using AutoMapper;
using FabysUnha.Models;
using FabysUnha.ViewModels;

namespace FabysUnha.Mappings;

public class ClientesProfile : Profile
{
    public ClientesProfile()
    {
        CreateMap<Clientes, ClienteRegistroViewModel>().ReverseMap();
        CreateMap<Clientes, ClienteDetalhesViewModel>().ReverseMap();
        CreateMap<Clientes, ClienteListagemViewModel>().ReverseMap();
        CreateMap<Clientes, ClienteEditarViewModel>().ReverseMap();
    }
}
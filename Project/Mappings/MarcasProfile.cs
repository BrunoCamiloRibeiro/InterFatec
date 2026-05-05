using AutoMapper;
using FabysUnha.Models;
using FabysUnha.ViewModels;

namespace FabysUnha.Mappings;

public class MarcasProfile : Profile
{
    public MarcasProfile()
    {
        CreateMap<Marcas, MarcasViewModel>().ReverseMap();
    }
}
using AutoMapper;
using FabysUnha.Models;
using FabysUnha.ViewModels;

namespace FabysUnha.Mappings;

public class EspecialidadesProfile : Profile
{
    public EspecialidadesProfile()
    {
        CreateMap<Especialidades, EspecialidadeViewModel>().ReverseMap();
    }
}
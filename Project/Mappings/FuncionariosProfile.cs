using AutoMapper;
using FabysUnha.Models;
using FabysUnha.ViewModels;

namespace FabysUnha.Mappings;

public class FuncionariosProfile : Profile
{
    public FuncionariosProfile()
    {
        CreateMap<Funcionarios, FuncionarioRegistroViewModel>().ReverseMap();
        CreateMap<Funcionarios, FuncionarioDetalhesViewModel>().ReverseMap();
        CreateMap<Funcionarios, FuncionarioListagemViewModel>().ReverseMap();
        CreateMap<Funcionarios, FuncionarioEditarViewModel>().ReverseMap();
    }
}
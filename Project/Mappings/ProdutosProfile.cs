using AutoMapper;
using FabysUnha.Models;
using FabysUnha.ViewModels;

namespace FabysUnha.Mappings;

public class ProdutosProfile : Profile
{
    public ProdutosProfile()
    {
        CreateMap<Produtos, ProdutoListagemViewModel>()
            .ForMember(dest => dest.NomeMarca, opt => opt.MapFrom(src => src.Marca != null ? src.Marca.Nome : "Sem Marca"))
            .ForMember(dest => dest.PrecoFormatado, opt => opt.MapFrom(src => src.Preco.ToString("C")));

        CreateMap<Produtos, ProdutoDetalhesViewModel>()
            .ForMember(dest => dest.NomeMarca, opt => opt.MapFrom(src => src.Marca != null ? src.Marca.Nome : "Sem Marca"))
            .ForMember(dest => dest.PrecoFormatado, opt => opt.MapFrom(src => src.Preco.ToString("C")));

        CreateMap<ProdutoRegistroViewModel, Produtos>();
        CreateMap<ProdutoEditarViewModel, Produtos>().ReverseMap();
    }
}
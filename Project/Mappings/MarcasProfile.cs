using AutoMapper;
using FabysUnha.Models;
using FabysUnha.ViewModels;

namespace FabysUnha.Mappings;

/// <summary>
/// Perfil de mapeamento do AutoMapper para a entidade Marcas.
/// </summary>
public class MarcasProfile : Profile
{
    /// <summary>
    /// Construtor contendo as regras de mapeamento utilizadas para marcas de produtos.
    /// </summary>
    public MarcasProfile()
    {
        // Estabelece mapeamento bidirecional (ida e volta) entre a entidade de dados 'Marcas' e o 'MarcasViewModel'.
        // Isso facilita salvar novos registros e exibi-los na interface gráfica.
        CreateMap<Marcas, MarcasViewModel>().ReverseMap();
    }
}
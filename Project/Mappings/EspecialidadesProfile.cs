using AutoMapper;
using FabysUnha.Models;
using FabysUnha.ViewModels;

namespace FabysUnha.Mappings;

/// <summary>
/// Perfil de mapeamento do AutoMapper para a entidade Especialidades.
/// </summary>
public class EspecialidadesProfile : Profile
{
    /// <summary>
    /// Construtor padrão que configura a conversão de objetos relacionados a Especialidades.
    /// </summary>
    public EspecialidadesProfile()
    {
        // Cria um mapeamento direto e inverso (ReverseMap) entre a entidade Especialidades
        // e seu respectivo ViewModel (EspecialidadeViewModel).
        // Útil para que as propriedades com nomes iguais sejam copiadas automaticamente.
        CreateMap<Especialidades, EspecialidadeViewModel>().ReverseMap();
    }
}
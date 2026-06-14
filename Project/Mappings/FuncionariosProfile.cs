using AutoMapper;
using FabysUnha.Models;
using FabysUnha.ViewModels;

namespace FabysUnha.Mappings;

/// <summary>
/// Perfil de mapeamento do AutoMapper para a entidade de Funcionários.
/// Define as regras de transferência de dados entre Models e ViewModels.
/// </summary>
public class FuncionariosProfile : Profile
{
    /// <summary>
    /// Construtor que estabelece as configurações de mapeamento para as operações com Funcionários.
    /// </summary>
    public FuncionariosProfile()
    {
        // Mapeamento bidirecional para registro de novos funcionários.
        CreateMap<Funcionarios, FuncionarioRegistroViewModel>()
            // Mapeia explicitamente o campo Senha da entidade para o ViewModel.
            .ForMember(dest => dest.Senha, opt => opt.MapFrom(src => src.Senha))
            .ReverseMap();

        // Mapeamento bidirecional focado em exibir os detalhes de um funcionário existente.
        CreateMap<Funcionarios, FuncionarioDetalhesViewModel>()
            // Extrai a descrição da especialidade, validando se o objeto não é nulo.
            .ForMember(dest => dest.EspecialidadeNome, opt => opt.MapFrom(src => src.Especialidade != null ? src.Especialidade.Descricao : string.Empty))
            // Mapeia a coleção de serviços que foram agendados para este funcionário.
            .ForMember(dest => dest.ServicosAgendados, opt => opt.MapFrom(src => src.Servicos_Agendados))
            .ReverseMap();

        // Mapeamento bidirecional para a tela de listagem de funcionários.
        CreateMap<Funcionarios, FuncionarioListagemViewModel>()
            // Simplifica a exibição de dados estrangeiros buscando a descrição da Especialidade vinculada.
            .ForMember(dest => dest.EspecialidadeNome, opt => opt.MapFrom(src => src.Especialidade != null ? src.Especialidade.Descricao : string.Empty))
            .ReverseMap();

        // Mapeamento básico bidirecional para atualizar as informações de um funcionário.
        CreateMap<Funcionarios, FuncionarioEditarViewModel>().ReverseMap();
    }
}
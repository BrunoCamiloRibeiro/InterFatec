using AutoMapper;
using FabysUnha.Models;
using FabysUnha.ViewModels;

namespace FabysUnha.Mappings;

/// <summary>
/// Perfil de mapeamento do AutoMapper para a entidade de Clientes.
/// Facilita a conversão de objetos entre as camadas do sistema.
/// </summary>
public class ClientesProfile : Profile
{
    /// <summary>
    /// Construtor padrão que inicializa os mapeamentos necessários.
    /// </summary>
    public ClientesProfile()
    {
        // Cria mapeamento bidirecional (ReverseMap) entre a entidade Clientes e ClienteRegistroViewModel.
        // O ReverseMap permite converter em ambos os sentidos.
        CreateMap<Clientes, ClienteRegistroViewModel>().ReverseMap();

        // Mapeia de Clientes para o ViewModel de Detalhes.
        CreateMap<Clientes, ClienteDetalhesViewModel>()
            // Calcula o total de agendamentos realizados pelo cliente.
            .ForMember(dest => dest.TotalAgendamentos, opt => opt.MapFrom(src => src.Agendamentos.Count))
            // Obtém a data do último agendamento através da ordenação descendente pela Data.
            .ForMember(dest => dest.DataUltimoAgendamento, opt => opt.MapFrom(src =>
                src.Agendamentos
                    // Ordena agendamentos do mais recente para o mais antigo.
                    .OrderByDescending(agendamento => agendamento.Data)
                    // Projeta apenas a Data (convertendo para DateTime anulável).
                    .Select(agendamento => (DateTime?)agendamento.Data)
                    // Pega a primeira data (a mais recente) ou null se não houver agendamentos.
                    .FirstOrDefault()))
            .ReverseMap();

        // Mapeia de Clientes para o ViewModel usado na exibição em formato de lista (grids, tabelas).
        CreateMap<Clientes, ClienteListagemViewModel>()
            // Extrai o número total de agendamentos do cliente contando a coleção associada.
            .ForMember(dest => dest.TotalAgendamentos, opt => opt.MapFrom(src => src.Agendamentos.Count))
            // Descobre a data do agendamento mais recente vinculado a este cliente.
            .ForMember(dest => dest.DataUltimoAgendamento, opt => opt.MapFrom(src =>
                src.Agendamentos
                    // Ordena descrescentemente pela data para que o mais recente seja o primeiro da lista.
                    .OrderByDescending(agendamento => agendamento.Data)
                    // Seleciona a propriedade de data com tipagem anulável para o caso de estar vazio.
                    .Select(agendamento => (DateTime?)agendamento.Data)
                    // Retorna o primeiro registro ou null.
                    .FirstOrDefault()))
            .ReverseMap();

        // Mapeia de Clientes para o ViewModel de edição de forma bidirecional.
        CreateMap<Clientes, ClienteEditarViewModel>().ReverseMap();
    }
}
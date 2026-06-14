using AutoMapper;
using FabysUnha.Models;
using FabysUnha.ViewModels.Servicos;

namespace FabysUnha.Mappings;

/// <summary>
/// Perfil de mapeamento do AutoMapper responsável pelas conversões da entidade Serviços.
/// </summary>
public class ServicosProfile : Profile
{
    /// <summary>
    /// Construtor que agrupa a definição de mapeamentos dos Serviços, além de formatação de valores.
    /// </summary>
    public ServicosProfile()
    {

        // Mapeia a entidade Servicos para a visualização em lista.
        CreateMap<Servicos, ServicoListagemViewModel>()
            // Converte o valor numérico do preço para formato de moeda corrente local (ex: R$ 0,00).
            .ForMember(dest => dest.PrecoFormatado, opt => opt.MapFrom(src => src.Preco.ToString("C")))
            // Formata o TimeSpan para exibir o tempo de execução de uma forma amigável, como '01h 30m'.
            .ForMember(dest => dest.TempoFormatado, opt => opt.MapFrom(src => src.Tempo.ToString(@"hh\h\ mm\m")));

        // Mapeia de Servicos para o ViewModel usado na página de Detalhes.
        CreateMap<Servicos, ServicoDetalhesViewModel>()
            // Reaplica a mesma formatação monetária ("C" representa Currency) para a visualização de detalhes.
            .ForMember(dest => dest.PrecoFormatado, opt => opt.MapFrom(src => src.Preco.ToString("C")))
            // Formata a duração do serviço mantendo consistência de exibição visual.
            .ForMember(dest => dest.TempoFormatado, opt => opt.MapFrom(src => src.Tempo.ToString(@"hh\h\ mm\m")));

        // Mapeia de ViewModel para Entidade, operando como destino para dados submetidos ao criar novo serviço.
        CreateMap<ServicoRegistroViewModel, Servicos>();
        
        // Mapeamento bidirecional permitindo que os dados do serviço sejam abertos no formulário e salvos após alteração.
        CreateMap<ServicoEditarViewModel, Servicos>().ReverseMap();
    }
}
using AutoMapper;
using FabysUnha.Models;
using FabysUnha.ViewModels;

namespace FabysUnha.Mappings;

/// <summary>
/// Perfil de mapeamento do AutoMapper para a entidade de Produtos.
/// Responsável por converter e formatar informações de produtos entre a camada de apresentação e a de dados.
/// </summary>
public class ProdutosProfile : Profile
{
    /// <summary>
    /// Configurações dos mapas utilizados para manipular dados de Produtos.
    /// </summary>
    public ProdutosProfile()
    {
        // Mapeia de Produtos para o modelo de listagem usado na visualização geral.
        CreateMap<Produtos, ProdutoListagemViewModel>()
            // Verifica a associação de Marca. Se não houver, define "Sem Marca" como valor padrão.
            .ForMember(dest => dest.NomeMarca, opt => opt.MapFrom(src => src.Marca != null ? src.Marca.Nome : "Sem Marca"))
            // Converte e formata a propriedade decimal 'Preco' como uma string no formato de moeda local ("C").
            .ForMember(dest => dest.PrecoFormatado, opt => opt.MapFrom(src => src.Preco.ToString("C")));

        // Mapeia de Produtos para o modelo usado ao detalhar o registro específico de um produto.
        CreateMap<Produtos, ProdutoDetalhesViewModel>()
            // Pega o nome da marca associada, se existir.
            .ForMember(dest => dest.NomeMarca, opt => opt.MapFrom(src => src.Marca != null ? src.Marca.Nome : "Sem Marca"))
            // Transforma o número 'Preco' para formato monetário visível ao usuário final.
            .ForMember(dest => dest.PrecoFormatado, opt => opt.MapFrom(src => src.Preco.ToString("C")));

        // Cria mapeamento unidirecional do ViewModel de registro para a entidade Produtos.
        // Utilizado no momento da criação de um novo produto.
        CreateMap<ProdutoRegistroViewModel, Produtos>();

        // Mapeia bidirecionalmente entre o ViewModel de edição e a entidade principal,
        // garantindo que os dados possam ser lidos no form de edição e salvos de volta no banco.
        CreateMap<ProdutoEditarViewModel, Produtos>().ReverseMap();
    }
}
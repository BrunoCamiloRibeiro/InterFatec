using FabysUnha.Models;
using FabysUnha.Repositories;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace FabysUnha.Services;

/// <summary>
/// Serviço de Produtos responsável pelas regras de negócio, manipulação de arquivos (upload de imagens) e comunicação com o repositório.
/// </summary>
public class ProdutosService : IProdutosService
{
    private readonly IProdutosRepository _produtoRepository;
    private readonly IWebHostEnvironment _env;

    /// <summary>
    /// Construtor principal da classe de serviço para <see cref="Produtos"/>.
    /// </summary>
    /// <param name="produtoRepository">A abstração do repositório para lidar com o acesso a dados.</param>
    /// <param name="env">A interface do ambiente de hospedagem web para auxiliar no armazenamento físico dos arquivos estáticos.</param>
    public ProdutosService(IProdutosRepository produtoRepository, IWebHostEnvironment env)
    {
        _produtoRepository = produtoRepository;
        _env = env;
    }

    /// <summary>
    /// Consulta no banco de dados e retorna todos os produtos disponíveis.
    /// </summary>
    /// <returns>Uma lista dos produtos da base de dados.</returns>
    public async Task<IEnumerable<Produtos>> ObterTodosProdutos()
    {
        return await _produtoRepository.ObterTodosProdutos();
    }

    /// <summary>
    /// Obtém um produto individual pela sua chave (código).
    /// </summary>
    /// <param name="id">O código ou ID do produto no sistema.</param>
    /// <returns>O produto buscado ou nulo se não encontrado.</returns>
    public async Task<Produtos?> ObterProdutoPorId(int id)
    {
        return await _produtoRepository.ObterProdutoPorId(id);
    }

    /// <summary>
    /// Aplica regras de negócio para a criação de um novo produto e realiza o upload da sua imagem, caso exista.
    /// </summary>
    /// <param name="produto">O modelo de dados do produto.</param>
    /// <param name="imagemUpload">O arquivo submetido em um formulário HTTP contendo a imagem do produto (opcional).</param>
    public async Task CriarProduto(Produtos produto, IFormFile? imagemUpload)
    {
        // Regra de negócio: O valor monetário não pode ser zero ou negativo.
        if(produto.Preco <= 0) throw new ArgumentException("O preço do produto deve ser maior que zero.");
        
        // Regra de negócio: Todo produto precisa ter uma associação direta com uma marca válida.
        if(produto.MarcaId <= 0) throw new ArgumentException("O produto deve estar associado a uma marca.");
        
        // Invoca o método privado responsável por tratar e armazenar o arquivo no servidor.
        produto.PathImagem = await SalvarImagemUploadAsync(imagemUpload, produto.PathImagem);
        
        // Se após a tentativa de salvamento o caminho permanecer nulo/vazio, designa um placeholder padrão.
        if(string.IsNullOrWhiteSpace(produto.PathImagem)) 
        {
            produto.PathImagem = "/images/placeholder.jpg";
        }

        // Persiste as informações validadas através da camada de dados.
        await _produtoRepository.CriarProduto(produto);
    }

    /// <summary>
    /// Atualiza as informações de um produto, aplicando regras sobre preço, status e eventual substituição de imagem.
    /// </summary>
    /// <param name="produto">A entidade contendo as modificações feitas pelo usuário.</param>
    /// <param name="imagemUpload">Novo arquivo de imagem a ser upado para o servidor.</param>
    /// <param name="hasStatusUpdate">Indicador booleano que diz se o status deve ser atualizado ou mantido o anterior.</param>
    public async Task AtualizarProduto(Produtos produto, IFormFile? imagemUpload, bool hasStatusUpdate)
    {
        // Revalidação das restrições de consistência: preço e relação da marca.
        if(produto.Preco <= 0) throw new ArgumentException("O preço do produto deve ser maior que zero.");
        if(produto.MarcaId <= 0) throw new ArgumentException("O produto deve estar associado a uma marca.");

        // Recupera o registro original direto do banco de dados a fim de fazer comparações e mesclas.
        var produtoAtual = await _produtoRepository.ObterProdutoPorId(produto.Codigo);
        if (produtoAtual == null) throw new ArgumentException("Produto não encontrado.");

        // Se a chamada à API ou interface não propôs alterar o status, restauramos o valor antigo do BD.
        if (!hasStatusUpdate)
        {
            produto.Status = produtoAtual.Status;
        }

        // Gerencia a imagem. Se enviaram um arquivo válido e de tamanho acima de 0 bytes...
        if (imagemUpload != null && imagemUpload.Length > 0)
        {
            // Salva o novo arquivo gerando um link e ignora o arquivo anterior (o envio de fallback é null).
            produto.PathImagem = await SalvarImagemUploadAsync(imagemUpload, null);
        }
        else if (string.IsNullOrWhiteSpace(produto.PathImagem))
        {
            // Caso não tenham mandado um arquivo e o caminho no objeto atual vier em branco, preservamos a imagem anterior.
            produto.PathImagem = produtoAtual.PathImagem;
        }

        // Garantia adicional: Caso alguma destas checagens falhe, assume uma imagem de espaço reservado.
        if(string.IsNullOrWhiteSpace(produto.PathImagem)) 
        {
            produto.PathImagem = "/images/placeholder.jpg";
        }

        // Efetiva a gravação da atualização do produto no banco.
        await _produtoRepository.AtualizarProduto(produto);
    }

    /// <summary>
    /// Método interno utilitário para gravar arquivos submetidos pelo usuário no diretório físico do servidor.
    /// </summary>
    /// <param name="imagemUpload">O binário/stream do upload HTTP.</param>
    /// <param name="fallbackPath">O caminho que deve ser retornado como padrão se nenhum arquivo foi mandado.</param>
    /// <returns>O caminho relativo estático (URL) em que a imagem foi armazenada, pronta para uso no HTML/Frontend.</returns>
    private async Task<string?> SalvarImagemUploadAsync(IFormFile? imagemUpload, string? fallbackPath)
    {
        // Apenas processa se o arquivo existe e contém dados.
        if (imagemUpload != null && imagemUpload.Length > 0)
        {
            // Constrói o caminho completo onde as imagens dos produtos residem dentro de wwwroot (WebRootPath).
            var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "produtos");
            
            // Certifica-se de que a pasta de destino existe; se não, cria.
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
            
            // Gera um nome único (GUID) para impedir conflitos caso usuários enviem arquivos com o mesmo nome (Ex: 'foto.png').
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + imagemUpload.FileName;
            
            // Combina o diretório de destino com o novo nome único do arquivo.
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            
            // Cria e abre um FileStream de criação e copia assincronamente os dados do IFormFile para dentro dele.
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imagemUpload.CopyToAsync(fileStream);
            }
            
            // Retorna o caminho da forma como o navegador deve acessá-lo.
            return "/images/produtos/" + uniqueFileName;
        }
        
        // Se a verificação inicial falhou, devolve o caminho de fallback original.
        return fallbackPath;
    }

    /// <summary>
    /// Deleta um produto por completo baseado no seu código de identificação.
    /// </summary>
    /// <param name="id">Código identificador exclusivo do produto no sistema.</param>
    public async Task ExcluirProduto(int id)
    {
        // Instancia as propriedades originais do registro para que possamos passa-lo ao comando de exclusão.
        var produto = await _produtoRepository.ObterProdutoPorId(id);
        
        // Confirmação para evitar NullReference caso o ID não exista na tabela do banco.
        if (produto != null)
        {
            await _produtoRepository.ExcluirProduto(produto);
        }
    }
}
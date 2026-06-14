using Microsoft.EntityFrameworkCore;
using FabysUnha.Models;
using FabysUnha.Models.SqlViews;

namespace FabysUnha.Data;

/// <summary>
/// Contexto do Banco de Dados principal da aplicação. Herda de DbContext,
/// que é a classe central do Entity Framework Core para interagir com os dados.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Construtor padrão do DbContext, repassando as opções de configuração 
    /// (como qual banco usar, string de conexão) para a classe base do EF Core.
    /// </summary>
    /// <param name="options">As opções de configuração do contexto.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        // Nenhuma configuração adicional no construtor.
    }    

    /// <summary>
    /// Tabela de Pessoas. Utilizada como base para dados comuns de Clientes e Funcionários.
    /// </summary>
    public DbSet<Pessoas> Pessoas { get; set; }

    /// <summary>
    /// Tabela de Clientes, que logicamente estende de Pessoas.
    /// </summary>
    public DbSet<Clientes> Clientes { get; set; }

    /// <summary>
    /// Tabela de Funcionários, contendo detalhes como salário e especialidade.
    /// </summary>
    public DbSet<Funcionarios> Funcionarios { get; set; }

    
    /// <summary>
    /// Tabela de Marcas, para agrupar e categorizar produtos.
    /// </summary>
    public DbSet<Marcas> Marcas { get; set; }

    /// <summary>
    /// Tabela de Especialidades, que define os cargos e habilidades dos funcionários.
    /// </summary>
    public DbSet<Especialidades> Especialidades { get; set; }

    /// <summary>
    /// Tabela de Produtos cadastrados para venda ou uso no estabelecimento.
    /// </summary>
    public DbSet<Produtos> Produtos { get; set; }

    /// <summary>
    /// Tabela de Serviços que o estabelecimento oferece.
    /// </summary>
    public DbSet<Servicos> Servicos { get; set; }

    /// <summary>
    /// Tabela de Agendamentos. Registra um encontro marcado por um cliente.
    /// </summary>
    public DbSet<Agendamentos> Agendamentos { get; set; }

    /// <summary>
    /// Tabela associativa que vincula um Serviço agendado a um Agendamento principal e ao Funcionário responsável.
    /// </summary>
    public DbSet<Servicos_Agendados> Servicos_Agendados { get; set; }

    /// <summary>
    /// Tabela associativa que vincula os Produtos que possivelmente foram gastos ou comprados durante um serviço.
    /// </summary>
    public DbSet<Produtos_Agendados> Produtos_Agendados { get; set; }

    /// <summary>
    /// Método chamado pelo Entity Framework Core durante a inicialização do contexto
    /// para configurar o esquema do banco de dados, mapeamentos de tabelas, chaves e relacionamentos (Fluent API).
    /// </summary>
    /// <param name="modelBuilder">Construtor de modelos usado para configurar as entidades.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Chama a implementação base para garantir configurações nativas
        base.OnModelCreating(modelBuilder);
        
        // =========================================================
        // CONFIGURAÇÃO DAS ENTIDADES (TABELAS) E SEUS RELACIONAMENTOS
        // =========================================================

        // Configuração da entidade Pessoas usando Fluent API.
        modelBuilder.Entity<Pessoas>(entity =>
        {
            // Define explicitamente o nome da tabela no banco de dados.
            entity.ToTable("Pessoas");
            
            // Mapeia as propriedades da classe para as colunas na tabela.
            entity.Property(p => p.Id).HasColumnName("id");
            entity.Property(p => p.Nome).HasColumnName("Nome");
            entity.Property(p => p.Telefone).HasColumnName("Telefone");
            entity.Property(p => p.Status).HasColumnName("status");
            entity.Property(p => p.Senha).HasColumnName("senha");
            
            // Define a chave primária da tabela.
            entity.HasKey(p => p.Id);
        });

        // Configuração da entidade Clientes.
        modelBuilder.Entity<Clientes>(entity =>
        {
            entity.ToTable("Clientes");
            
            // Define que o ID do Cliente corresponde à coluna 'pessoa_id' no banco, 
            // indicando uma relação de herança ou 1:1 com a tabela Pessoas.
            entity.Property(c => c.Id).HasColumnName("pessoa_id");
            
            // Configura um relacionamento Um-para-Muitos (1:N):
            // Um Cliente pode ter Muitos Agendamentos.
            entity.HasMany(c => c.Agendamentos)
                .WithOne(a => a.Cliente) // Cada Agendamento tem Um Cliente
                .HasForeignKey(a => a.ClienteId); // A chave estrangeira fica em Agendamento
        });

        // Configuração da entidade Funcionarios.
        modelBuilder.Entity<Funcionarios>(entity =>
        {
            entity.ToTable("Funcionarios");
            entity.Property(f => f.Id).HasColumnName("pessoa_id");
            
            // Define a precisão do campo decimal (10 dígitos totais, 2 casas decimais) para evitar erros de arredondamento com dinheiro.
            entity.Property(f => f.Salario).HasColumnName("salario").HasPrecision(10, 2);
            entity.Property(f => f.EspecialidadeId).HasColumnName("especialidade_id");
            
            // Relacionamento N:1 - Muitos Funcionários possuem Uma Especialidade.
            entity.HasOne(f => f.Especialidade)
                .WithMany(e => e.Funcionarios)
                .HasForeignKey(f => f.EspecialidadeId);
                
            // Relacionamento 1:N - Um Funcionário pode estar associado a Muitos Serviços Agendados.
            entity.HasMany(f => f.Servicos_Agendados)
                .WithOne(sa => sa.Funcionario)
                .HasForeignKey(sa => sa.FuncionarioId);
        });

        modelBuilder.Entity<Marcas>(entity =>
        {
            entity.ToTable("Marcas");
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Id).HasColumnName("id");
            entity.Property(m => m.Nome).HasColumnName("nome");
            entity.Property(m => m.Status).HasColumnName("status");
            entity.HasMany(m => m.Produtos)
                .WithOne(p => p.Marca)
                .HasForeignKey(p => p.MarcaId);
        });

        modelBuilder.Entity<Especialidades>(entity =>
        {
            entity.ToTable("Especialidades");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descricao).HasColumnName("descricao");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.HasMany(e => e.Funcionarios)
                .WithOne(f => f.Especialidade)
                .HasForeignKey(f => f.EspecialidadeId);
        });

        modelBuilder.Entity<Produtos>(entity =>
        {
            entity.ToTable("Produtos");
            entity.HasKey(p => p.Codigo);
            entity.Property(p => p.Codigo).HasColumnName("codigo");
            entity.Property(p => p.Nome).HasColumnName("nome");
            entity.Property(p => p.MarcaId).HasColumnName("marca_id");
            entity.Property(p => p.Preco).HasColumnName("preco").HasPrecision(10, 2);
            entity.Property(p => p.PathImagem).HasColumnName("PathImagem");
            entity.Property(p => p.Status).HasColumnName("status");
            entity.HasOne(p => p.Marca)
                .WithMany(m => m.Produtos)
                .HasForeignKey(p => p.MarcaId);
            entity.HasMany(p => p.Produtos_Agendados)
                .WithOne(pa => pa.Produto)
                .HasForeignKey(pa => pa.ProdutoCodigo);
        });

        modelBuilder.Entity<Servicos>(entity =>
        {
            entity.ToTable("Servicos");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id).HasColumnName("id");
            entity.Property(s => s.Preco).HasColumnName("preco").HasPrecision(10, 2);
            entity.Property(s => s.Descricao).HasColumnName("descricao");
            entity.Property(s => s.Tempo).HasColumnName("tempo");
            entity.Property(s => s.Status).HasColumnName("status");
            entity.HasMany(s => s.Servicos_Agendados)
                .WithOne(sa => sa.Servico)
                .HasForeignKey(sa => sa.ServicoId);
        });

        modelBuilder.Entity<Agendamentos>(entity =>
        {
            entity.ToTable("Agendamentos");
            entity.HasKey(a => a.Nr);
            entity.Property(a => a.Nr).HasColumnName("nr");
            entity.Property(a => a.Data).HasColumnName("data");
            entity.Property(a => a.Total).HasColumnName("total").HasPrecision(10, 2);
            entity.Property(a => a.ClienteId).HasColumnName("cliente_id");
            entity.Property(a => a.Status).HasColumnName("status");

            entity.HasOne(a => a.Cliente)
                .WithMany(c => c.Agendamentos)
                .HasForeignKey(a => a.ClienteId);
            entity.HasMany(a => a.Servicos_Agendados)
                .WithOne(sa => sa.Agendamento)
                .HasForeignKey(sa => sa.AgendamentoNr);
            entity.HasMany(a => a.Produtos_Agendados)
                .WithOne(pa => pa.Agendamento)
                .HasForeignKey(pa => pa.AgendamentoNr);
        });

        modelBuilder.Entity<Servicos_Agendados>(entity =>
        {
            entity.ToTable("Servicos_Agendados");
            entity.HasKey(sa => new { sa.AgendamentoNr, sa.ServicoId });
            entity.Property(sa => sa.AgendamentoNr).HasColumnName("agendamento_nr");
            entity.Property(sa => sa.ServicoId).HasColumnName("servico_id");
            entity.Property(sa => sa.Obs).HasColumnName("obs");
            entity.Property(sa => sa.Horario).HasColumnName("horario");
            entity.Property(sa => sa.FuncionarioId).HasColumnName("funcionario_id");
            entity.Property(sa => sa.Valor).HasColumnName("valor").HasPrecision(10, 2);
            entity.HasOne(sa => sa.Agendamento)
                .WithMany(a => a.Servicos_Agendados)
                .HasForeignKey(sa => sa.AgendamentoNr);
            entity.HasOne(sa => sa.Servico)
                .WithMany(s => s.Servicos_Agendados)
                .HasForeignKey(sa => sa.ServicoId);
            entity.HasOne(sa => sa.Funcionario)
                .WithMany(f => f.Servicos_Agendados)
                .HasForeignKey(sa => sa.FuncionarioId);
            entity.HasMany(sa => sa.Produtos_Agendados)
                .WithOne(pa => pa.ServicoAgendado)
                .HasForeignKey(pa => new { pa.AgendamentoNr, pa.ServicoId });
        });

        modelBuilder.Entity<Produtos_Agendados>(entity =>
        {
            entity.ToTable("Produtos_Agendados");
            entity.HasKey(pa => new { pa.AgendamentoNr, pa.ServicoId, pa.ProdutoCodigo });
            entity.Property(pa => pa.AgendamentoNr).HasColumnName("agendamento_nr");
            entity.Property(pa => pa.ServicoId).HasColumnName("servico_id");
            entity.Property(pa => pa.ProdutoCodigo).HasColumnName("produto_codigo");
            entity.Property(pa => pa.Preco).HasColumnName("preco").HasPrecision(10, 2);
            entity.HasOne(pa => pa.Agendamento)
                .WithMany(a => a.Produtos_Agendados)
                .HasForeignKey(pa => pa.AgendamentoNr);
            entity.HasOne(pa => pa.Produto)
                .WithMany(p => p.Produtos_Agendados)
                .HasForeignKey(pa => pa.ProdutoCodigo);
        });

        // =========================================================
        // CONFIGURAÇÃO DE VIEWS DO BANCO DE DADOS
        // =========================================================
        // Entidades que representam Views não possuem chave primária (HasNoKey)
        // e são mapeadas para objetos de visualização usando ToView.
        modelBuilder.Entity<ListaFuncionariosView>(entity =>
        {
            entity.HasNoKey(); // Indica ao EF Core que não há chave primária
            entity.ToView("vw_ListaFuncionarios"); // Mapeia para uma View existente no banco
            entity.Property(e => e.Salario).HasPrecision(10, 2);
            entity.Property(e => e.StatusId).HasColumnName("Status_Id");
            entity.Property(e => e.StatusDescricao).HasColumnName("Status_Descricao");
        });

        modelBuilder.Entity<ListaClientesView>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("vw_ListaClientes");
            entity.Property(e => e.StatusId).HasColumnName("Status_Id");
            entity.Property(e => e.StatusDescricao).HasColumnName("Status_Descricao");
        });

        modelBuilder.Entity<ListaMarcasView>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("vw_ListaMarcas");
            entity.Property(e => e.StatusDescricao).HasColumnName("Status_Descricao");
        });

        modelBuilder.Entity<ListaServicosView>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("vw_ListaServicos");
            entity.Property(e => e.Preco).HasPrecision(10, 2);
            entity.Property(e => e.StatusId).HasColumnName("Status_Id");
            entity.Property(e => e.StatusDescricao).HasColumnName("Status_Descricao");
        });

        modelBuilder.Entity<ListaProdutosView>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("vw_ListarProdutos");
            entity.Property(e => e.Nome).HasColumnName("Produto");
            entity.Property(e => e.Preco).HasPrecision(10, 2);
            entity.Property(e => e.StatusId).HasColumnName("Status_Id");
            entity.Property(e => e.StatusDescricao).HasColumnName("Status_Descricao");
        });

        modelBuilder.Entity<ListaEspecialidadesView>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("vw_ListarEspecialidades");
        });

        modelBuilder.Entity<ListaAgendamentosView>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("vw_ListaAgendamento");
            entity.Property(e => e.NumeroAgendamento).HasColumnName("NumeroAgendamento");
            entity.Property(e => e.Total).HasPrecision(10, 2);
            entity.Property(e => e.StatusDescricao).HasColumnName("Status_Descricao");
        });

        modelBuilder.Entity<ListaServicoAgendamentoView>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("vw_ListaServicoAgendamento");
            entity.Property(e => e.NumeroAgendamento).HasColumnName("NumeroAgendamento");
            entity.Property(e => e.Observacao).HasColumnName("Observacao");
            entity.Property(e => e.Horario).HasColumnName("Horario");
            entity.Property(e => e.Valor).HasPrecision(10, 2);
        });

        modelBuilder.Entity<ListaProdutoAgendamentoView>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("vw_ListaProdutoAgendamento");
            entity.Property(e => e.NumeroAgendamento).HasColumnName("NumeroAgendamento");
            entity.Property(e => e.Observacao).HasColumnName("Observacao");
            entity.Property(e => e.Preco).HasColumnName("Preco").HasPrecision(10, 2);
        });

        modelBuilder.Entity<FuncionarioProducaoView>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("vw_FuncionarioProducao");
        });

        modelBuilder.Entity<ProdutosPorMarcaView>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("vw_ProdutosPorMarca");
        });
    }
}


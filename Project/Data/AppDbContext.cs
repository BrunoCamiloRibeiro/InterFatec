using Microsoft.EntityFrameworkCore;
using FabysUnha.Models;

namespace FabysUnha.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }    

    public DbSet<Pessoas> Pessoas { get; set; }
    public DbSet<Clientes> Clientes { get; set; }
    public DbSet<Funcionarios> Funcionarios { get; set; }

    
    public DbSet<Marcas> Marcas { get; set; }

    public DbSet<Especialidades> Especialidades { get; set; }
    public DbSet<Produtos> Produtos { get; set; }
    public DbSet<Servicos> Servicos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Pessoas>().ToTable("Pessoas");
        modelBuilder.Entity<Clientes>().ToTable("Clientes");
        modelBuilder.Entity<Funcionarios>().ToTable("Funcionarios");

        modelBuilder.Entity<Servicos_Agendados>()
        .HasKey(sa => new { sa.AgendamentoNr, sa.ServicoId });

        modelBuilder.Entity<Produtos_Agendados>()
            .HasKey(pa => new { pa.AgendamentoNr, pa.ProdutoCodigo });
    }
}


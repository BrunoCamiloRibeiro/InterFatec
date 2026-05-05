using Microsoft.EntityFrameworkCore;
using FabysUnha.Models;

namespace FabysUnha.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }    

    public DbSet<Marcas> Marcas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Servicos_Agendados>()
        .HasKey(sa => new { sa.AgendamentoNr, sa.ServicoId });

        modelBuilder.Entity<Produtos_Agendados>()
            .HasKey(pa => new { pa.AgendamentoNr, pa.ProdutoCodigo });
    }
}


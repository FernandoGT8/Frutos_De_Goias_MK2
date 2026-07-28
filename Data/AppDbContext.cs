using FrutosDeGoias.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FrutosDeGoias.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Estabelecimento> Estabelecimentos { get; set; }
    public DbSet<ProducaoAgricola> Producoes { get; set; }

    // Sobrescrita do método OnModelCreating para aplicar a Fluent API
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProducaoAgricola>(entity =>
        {
            // 1. Limitação estrita de tamanho (Evita NVARCHAR(MAX))
            entity.Property(e => e.Cidade)
                  .IsRequired()
                  .HasMaxLength(150); 

            entity.Property(e => e.Fruta)
                  .IsRequired()
                  .HasMaxLength(100);

            // 2. Precisão matemática garantida no SQL Server (18 dígitos, 2 casas decimais)
            entity.Property(e => e.QuantidadeToneladas)
                  .HasColumnType("decimal(18,2)");

            // 3. O CORAÇÃO DA PERFORMANCE: Índice Composto
            // Cobre exatamente as colunas usadas no ".GroupBy(p => new { p.Cidade, p.Fruta })" do Controller
            entity.HasIndex(e => new { e.Cidade, e.Fruta })
                  .HasDatabaseName("IX_ProducaoAgricola_Cidade_Fruta");
        });
    }
}
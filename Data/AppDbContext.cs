using FrutosDeGoias.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FrutosDeGoias.Api.Data;

public class AppDbContext : DbContext
{
    // Construtor que recebe as opções de conexão injetadas pelo sistema
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Representa a tabela física no SQL Server
    public DbSet<Estabelecimento> Estabelecimentos { get; set; }

    public DbSet<ProducaoAgricola> Producoes { get; set; }
}
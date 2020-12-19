using api_sis_evenda.estoque.Models;
using Microsoft.EntityFrameworkCore;

namespace api_sis_evenda.vendas.Models
{
    public class ContextVendas : DbContext
    {
        public DbSet<Produto> Produtos { get; set; }

        public ContextVendas(DbContextOptions<ContextVendas> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>()
            .HasAlternateKey(a => a.Codigo);
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace api_sis_evenda.estoque.Models
{
    public class Context : DbContext
    {
        public DbSet<Produto> Produtos { get; set; }

        public Context(DbContextOptions<Context> options) :base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>().Property(p => p.Nome).IsRequired();
            modelBuilder.Entity<Produto>().Property(p => p.Nome).HasMaxLength(20);

            modelBuilder.Entity<Produto>().HasIndex(p => p.Codigo).IsUnique();
            modelBuilder.Entity<Produto>().Property(p => p.Codigo).IsRequired();
            modelBuilder.Entity<Produto>().Property(p => p.Codigo).HasMaxLength(5);

            modelBuilder.Entity<Produto>().Property(p => p.Preco).IsRequired();

            modelBuilder.Entity<Produto>().Property(p => p.Quantidade).IsRequired();
            
        }
    }
}


using CleanArch.Domain;
using Microsoft.EntityFrameworkCore;

namespace CleanArch.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Conta> Contas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Conta>(entity =>
            {
                entity.HasKey(e => e.Numero);
                entity.Property(e => e.Numero)
                    .HasConversion(
                        v => v.Value,
                        v => new NumeroConta(v))
                    .HasColumnType("varchar(6)");
                entity.Property(e => e.Saldo).HasColumnType("decimal(18,2)");
            });
        }
    }
}

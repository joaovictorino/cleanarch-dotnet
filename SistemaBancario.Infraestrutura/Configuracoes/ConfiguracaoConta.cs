using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaBancario.Dominio.Entidades;
using SistemaBancario.Dominio.ObjetosValor;

namespace SistemaBancario.Infraestrutura.Configuracoes
{
    public class ConfiguracaoConta : IEntityTypeConfiguration<Conta>
    {
        public void Configure(EntityTypeBuilder<Conta> construtor)
        {
            construtor.HasKey(c => c.Id);

            construtor.Property(c => c.Id)
                .HasColumnType("CHAR(36)")
                .ValueGeneratedNever();

            construtor.Property(c => c.Numero)
                .HasConversion(
                    v => v.Valor, 
                    v => new NumeroConta(v))
                .HasMaxLength(6)
                .HasColumnType("VARCHAR(6)")
                .IsRequired();

            construtor.Property(c => c.NomeCliente)
                .HasMaxLength(200)
                .HasColumnType("VARCHAR(200)")
                .IsRequired();

            construtor.Property(c => c.Saldo)
                .HasColumnType("DECIMAL(18,2)")
                .IsRequired();

            construtor.Property(c => c.DataCriacao)
                .HasColumnType("DATETIME")
                .IsRequired();

            construtor.HasIndex(c => c.Numero)
                .IsUnique()
                .HasDatabaseName("IX_Conta_Numero_Unique");

            construtor.HasMany(c => c.TransacoesOrigem)
                .WithOne(t => t.ContaOrigem)
                .OnDelete(DeleteBehavior.Restrict);
            
            construtor.HasMany(c => c.TransacoesDestino)
                .WithOne(t => t.ContaDestino)
                .OnDelete(DeleteBehavior.Restrict);

            construtor.ToTable("Contas");
        }
    }
}
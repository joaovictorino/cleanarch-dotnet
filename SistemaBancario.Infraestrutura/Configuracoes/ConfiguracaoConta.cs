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
            construtor.HasKey(x => x.Id);
            
            construtor.Property(x => x.Id)
                .HasColumnType("CHAR(36)")
                .ValueGeneratedNever();
            
            construtor.Property(x => x.Numero)
                .HasConversion(
                    v => v.Valor,
                    v => new NumeroConta(v))
                .HasMaxLength(6)
                .HasColumnType("VARCHAR(6)")
                .IsRequired();

            construtor.Property(x => x.NomeCliente)
                .HasMaxLength(200)
                .HasColumnType("VARCHAR(200)")
                .IsRequired();

            construtor.Property(x => x.Saldo)
                .HasColumnType("DECIMAL(18,2)");

            construtor.Property(x => x.DataCriacao)
                .HasColumnType("DATETIME");

            construtor.HasIndex(x => x.Numero)
                .IsUnique()
                .HasDatabaseName("IX_Conta_Numero");

            construtor.HasMany(x => x.TransacoesOrigem)
                .WithOne(x => x.ContaOrigem)
                .OnDelete(DeleteBehavior.Restrict);

            construtor.HasMany(x => x.TransacoesDestino)
                .WithOne(x => x.ContaDestino)
                .OnDelete(DeleteBehavior.Restrict);

            construtor.ToTable("Contas");
        }
    }
}

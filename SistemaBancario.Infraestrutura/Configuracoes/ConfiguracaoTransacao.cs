using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaBancario.Dominio.Entidades;

namespace SistemaBancario.Infraestrutura.Configuracoes
{
    public class ConfiguracaoTransacao : IEntityTypeConfiguration<Transacao>
    {
        public void Configure(EntityTypeBuilder<Transacao> construtor)
        {
            construtor.HasKey(x => x.Id);

            // Configuração específica para MySQL
            construtor.Property(x => x.Id)
                .HasColumnType("CHAR(36)")
                .ValueGeneratedNever();

            construtor.Property(x => x.IdContaOrigem)
                .HasColumnType("CHAR(36)")
                .IsRequired();

            construtor.Property(x => x.IdContaDestino)
                .HasColumnType("CHAR(36)")
                .IsRequired();

            construtor.Property(x => x.Valor)
                .HasColumnType("DECIMAL(18,2)");

            construtor.Property(x => x.DataHoraTransacao)
                .HasColumnType("DATETIME");

            construtor.Property(x => x.CodigoTransacao)
                .HasMaxLength(50)
                .HasColumnType("VARCHAR(50)")
                .IsRequired();

            construtor.HasOne(x => x.ContaOrigem)
                .WithMany(x => x.TransacoesOrigem)
                .HasForeignKey(x => x.IdContaOrigem)
                .OnDelete(DeleteBehavior.Restrict);

            construtor.HasOne(x => x.ContaDestino)
                .WithMany(x => x.TransacoesDestino)
                .HasForeignKey(x => x.IdContaDestino)
                .OnDelete(DeleteBehavior.Restrict);

            construtor.HasIndex(x => x.CodigoTransacao)
                .IsUnique()
                .HasDatabaseName("IX_Transacao_Codigo");

            construtor.ToTable("Transacoes");
        }
    }
}

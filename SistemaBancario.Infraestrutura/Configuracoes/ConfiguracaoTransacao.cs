using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaBancario.Dominio.Entidades;

namespace SistemaBancario.Infraestrutura.Configuracoes
{
    public class ConfiguracaoTransacao : IEntityTypeConfiguration<Transacao>
    {
        public void Configure(EntityTypeBuilder<Transacao> construtor)
        {
            construtor.HasKey(t => t.Id);

            construtor.Property(t => t.Id)
                .HasColumnType("CHAR(36)")
                .ValueGeneratedNever();

            construtor.Property(t => t.IdContaOrigem)
                .HasColumnType("CHAR(36)")
                .IsRequired();

            construtor.Property(t => t.IdContaDestino)
                .HasColumnType("CHAR(36)")
                .IsRequired();
    
            construtor.Property(t => t.Valor)
                .HasColumnType("DECIMAL(18,2)")
                .IsRequired();

            construtor.Property(t => t.DataHoraTransacao)
                .HasColumnType("DATETIME")
                .IsRequired();
            
            construtor.Property(t => t.CodigoTransacao)
                .HasMaxLength(50)
                .HasColumnType("VARCHAR(50)")
                .IsRequired();

            construtor.HasOne(t => t.ContaOrigem)
                .WithMany(c => c.TransacoesOrigem)
                .HasForeignKey(t => t.IdContaOrigem)
                .OnDelete(DeleteBehavior.Restrict);

            construtor.HasOne(t => t.ContaDestino)
                .WithMany(c => c.TransacoesDestino)
                .HasForeignKey(t => t.IdContaDestino)
                .OnDelete(DeleteBehavior.Restrict);

            construtor.HasIndex(t => t.CodigoTransacao)
                .IsUnique()
                .HasDatabaseName("IX_Transacao_CodigoTransacao_Unique");

            construtor.ToTable("Transacoes");
        }
    }
}
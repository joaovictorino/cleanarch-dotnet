using Microsoft.EntityFrameworkCore;
using SistemaBancario.Dominio.Entidades;
using SistemaBancario.Infraestrutura.Configuracoes;

namespace SistemaBancario.Infraestrutura.Dados
{
    public class ContextoBancario : DbContext
    {
        public DbSet<Conta> Contas { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }

        public ContextoBancario(DbContextOptions<ContextoBancario> opcoes) : base(opcoes) { }

        protected override void OnModelCreating(ModelBuilder construtor)
        {
            construtor.ApplyConfiguration(new ConfiguracaoConta());
            construtor.ApplyConfiguration(new ConfiguracaoTransacao());
            
            base.OnModelCreating(construtor);
        }
    }
}

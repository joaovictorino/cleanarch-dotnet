using SistemaBancario.Dominio.Entidades;
using SistemaBancario.Dominio.Interfaces;
using SistemaBancario.Infraestrutura.Dados;

namespace SistemaBancario.Infraestrutura.Repositorios
{
    public class RepositorioTransacao : IRepositorioTransacao
    {
        private readonly ContextoBancario _contexto;

        public RepositorioTransacao(ContextoBancario contexto)
        {
            _contexto = contexto;
        }

        public async Task AdicionarAsync(Transacao transacao)
        {
            await _contexto.Transacoes.AddAsync(transacao);
        }
    }
}

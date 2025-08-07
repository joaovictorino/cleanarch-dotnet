using Microsoft.EntityFrameworkCore;
using SistemaBancario.Dominio.Entidades;
using SistemaBancario.Dominio.Interfaces;
using SistemaBancario.Dominio.ObjetosValor;
using SistemaBancario.Infraestrutura.Dados;

namespace SistemaBancario.Infraestrutura.Repositorios
{
    public class RepositorioConta : IRepositorioConta
    {
        private readonly ContextoBancario _contexto;

        public RepositorioConta(ContextoBancario contexto)
        {
            _contexto = contexto;
        }

        public async Task<Conta?> ObterPorIdAsync(Guid id)
        {
            return await _contexto.Contas
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Conta?> ObterPorNumeroAsync(NumeroConta numero)
        {
            return await _contexto.Contas
                .FirstOrDefaultAsync(x => x.Numero == numero.Valor);
        }

        public async Task<List<Conta>> ObterTodosAsync()
        {
            return await _contexto.Contas.ToListAsync();
        }

        public async Task AdicionarAsync(Conta conta)
        {
            await _contexto.Contas.AddAsync(conta);
        }

        public async Task AtualizarAsync(Conta conta)
        {
            _contexto.Contas.Update(conta);
        }

        public async Task<bool> ExisteAsync(NumeroConta numero)
        {
            return await _contexto.Contas
                .AnyAsync(x => x.Numero == numero.Valor);
        }
    }
}


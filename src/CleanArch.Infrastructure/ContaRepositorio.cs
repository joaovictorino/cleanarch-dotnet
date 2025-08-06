using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArch.Domain;
using Microsoft.EntityFrameworkCore;

namespace CleanArch.Infrastructure
{
    public class ContaRepositorio : IContaRepositorio
    {
        private readonly AppDbContext _context;

        public ContaRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Conta>> GetAllAsync()
        {
            return await _context.Contas.ToListAsync();
        }

        public async Task<Conta?> GetByNumeroAsync(String numero)
        {
            var numeroConta = new NumeroConta(numero);
            return await _context.Contas.FirstOrDefaultAsync(c => c.Numero == numeroConta);
        }

        public async Task SalvarAsync(Conta conta)
        {
            var contaExistente = await _context.Contas.FindAsync(conta.Numero);
            if (contaExistente == null)
            {
                await this._context.Contas.AddAsync(conta);
            }
            else
            {
                this._context.Entry(contaExistente).CurrentValues.SetValues(conta);
            }
            await _context.SaveChangesAsync();
        }
    }
}

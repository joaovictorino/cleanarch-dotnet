using SistemaBancario.Dominio.Entidades;
using SistemaBancario.Dominio.ObjetosValor;

namespace SistemaBancario.Dominio.Interfaces
{
    public interface IRepositorioConta
    {
        Task<Conta?> ObterPorIdAsync(Guid id);
        Task<Conta?> ObterPorNumeroAsync(NumeroConta numero);
        Task<List<Conta>> ObterTodosAsync();
        Task AdicionarAsync(Conta conta);
        Task AtualizarAsync(Conta conta);
        Task<bool> ExisteAsync(NumeroConta numero);
    }
}

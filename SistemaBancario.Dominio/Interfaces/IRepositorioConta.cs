using SistemaBancario.Dominio.Entidades;
using SistemaBancario.Dominio.ObjetosValor;

namespace SistemaBancario.Dominio.Interfaces
{
    public interface IRepositorioConta
    {
        Task<Conta?> ObterPorNumeroAsync(string numero);
        Task<List<Conta>> ObterTodosAsync();
        Task AdicionarAsync(Conta conta);
        Task AtualizarAsync(Conta conta);
        Task<bool> ExisteAsync(string numero);
    }
}

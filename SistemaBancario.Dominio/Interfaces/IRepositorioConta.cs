using SistemaBancario.Dominio.Entidades;

namespace SistemaBancario.Dominio.Interfaces
{
    public interface IRepositorioConta
    {
        Task<Conta?> ObterPorNumeroAsync(string numero);
        Task<List<Conta>> ListarAsync();
        Task AdicionarAsync(Conta conta);
        Task AtualizarAsync(Conta conta);
        Task<bool> ExisteAsync(string numero);
    }
}

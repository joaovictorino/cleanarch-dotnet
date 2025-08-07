using SistemaBancario.Dominio.Entidades;

namespace SistemaBancario.Dominio.Interfaces
{
    public interface IRepositorioTransacao
    {
        Task AdicionarAsync(Transacao transacao);
    }
}

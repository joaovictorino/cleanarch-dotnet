using SistemaBancario.Dominio.Entidades;

namespace SistemaBancario.Dominio.Interfaces
{
    public interface ITransferirValor
    {
        Transacao Transferir(Conta origem, Conta destino, decimal valor);
    }
}


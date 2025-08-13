using SistemaBancario.Dominio.Entidades;
using SistemaBancario.Dominio.Interfaces;

namespace SistemaBancario.Dominio.Servicos
{
    public class TransferirValor : ITransferirValor
    {
        public Transacao Transferir(Conta origem, Conta destino, decimal valor)
        {
            if (origem == null)
                throw new ArgumentNullException(nameof(origem), "Conta de origem não pode ser nula.");
            if (destino == null)
                throw new ArgumentNullException(nameof(destino), "Conta de destino não pode ser nula.");
            if (valor <= 0)
                throw new ArgumentException("O valor da transferência deve ser positivo.");

            origem.Sacar(valor);
            destino.Depositar(valor);

            return new Transacao(origem, destino, valor);
        }
    }
}

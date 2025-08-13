using SistemaBancario.Dominio.Entidades;

namespace SistemaBancario.Dominio.Servicos
{
    public class TransferirValor
    {
        public Transacao Transferir(Conta contaOrigem, Conta contaDestino, decimal valor)
        {
            if (contaOrigem == null)
                throw new ArgumentNullException(nameof(contaOrigem), "Conta de origem não pode ser nula.");
            if (contaDestino == null)
                throw new ArgumentNullException(nameof(contaDestino), "Conta de destino não pode ser nula.");
            if (valor <= 0)
                throw new ArgumentException("O valor da transferência deve ser positivo.");

            contaOrigem.Sacar(valor);
            contaDestino.Depositar(valor);

            return new Transacao(contaOrigem, contaDestino, valor);
        }
    }
}

using SistemaBancario.Dominio.Entidades;

namespace SistemaBancario.Dominio.Servicos
{
    public class TransferirValor
    {
      public Transacao Transferir(Conta contaOrigem, Conta contaDestino, decimal valor)
      {
        contaOrigem.Debitar(valor);
        contaDestino.Creditar(valor);
        return new Transacao(contaOrigem.Id, contaDestino.Id, valor);
      }
    }
}

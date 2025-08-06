
using System;

namespace CleanArch.Domain
{
    public class TransferenciaValor
    {
        public Recibo Transferir(Conta origem, Conta destino, decimal valor)
        {
            if (origem == null) throw new ArgumentNullException(nameof(origem));
            if (destino == null) throw new ArgumentNullException(nameof(destino));

            origem.Debitar(valor);
            destino.Creditar(valor);

            return new Recibo(valor);
        }
    }
}

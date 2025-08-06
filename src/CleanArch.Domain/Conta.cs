using System;

namespace CleanArch.Domain
{
    public class Conta
    {
        public NumeroConta Numero { get; set; }
        public decimal Saldo { get; set; }

        public Conta(String numero, decimal saldo)
        {
            Numero = new NumeroConta(numero);
            Saldo = saldo;
        }

        private Conta(){}

        public void Creditar(decimal valor)
        {
            if (valor <= 0)
            {
                throw new ArgumentException("O valor a ser creditado deve ser positivo.");
            }
            Saldo += valor;
        }

        public void Debitar(decimal valor)
        {
            if (valor <= 0)
            {
                throw new ArgumentException("O valor a ser debitado deve ser positivo.");
            }
            if (Saldo < valor)
            {
                throw new InvalidOperationException("Saldo insuficiente.");
            }
            Saldo -= valor;
        }
    }
}

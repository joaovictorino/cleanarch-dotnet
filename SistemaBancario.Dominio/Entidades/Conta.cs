using SistemaBancario.Dominio.ObjetosValor;

namespace SistemaBancario.Dominio.Entidades
{
    public class Conta
    {
        public Guid Id { get; private set; }
        public NumeroConta Numero { get; private set; }
        public string NomeCliente { get; private set; }
        public decimal Saldo { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public List<Transacao> TransacoesOrigem { get; private set; } = new();
        public List<Transacao> TransacoesDestino { get; private set; } = new();

        private Conta(Guid id, NumeroConta numero, string nomeCliente, decimal saldo, DateTime dataCriacao)
        {
            Id = id;
            Numero = numero;
            NomeCliente = nomeCliente;
            Saldo = saldo;
            DataCriacao = dataCriacao;
        }

        public Conta(string numero, string nomeCliente, decimal saldo = 0)
        {
            Id = Guid.NewGuid();
            Numero = new NumeroConta(numero);
            NomeCliente = nomeCliente;
            Saldo = saldo;
            DataCriacao = DateTime.Now;
        }

        public void Depositar(decimal valor)
        {
            if (valor <= 0)
                throw new ArgumentException("O valor do depósito deve ser positivo.");

            Saldo += valor;
        }

        public void Sacar(decimal valor)
        {
            if (valor <= 0)
                throw new ArgumentException("O valor do saque deve ser positivo.");
            
            if (valor > Saldo)
                throw new InvalidOperationException("Saldo insuficiente para operação.");

            Saldo -= valor;
        }
    }
}
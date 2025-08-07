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

        public Conta(NumeroConta numero, string nomeCliente, decimal saldo = 0)
        {
            Id = Guid.NewGuid();
            Numero = numero;
            NomeCliente = nomeCliente;
            Saldo = saldo;
            DataCriacao = DateTime.UtcNow;
        }

        public void Debitar(decimal valor)
        {
            if (valor <= 0)
                throw new ArgumentException("Valor deve ser maior que zero");

            if (Saldo < valor)
                throw new InvalidOperationException("Saldo insuficiente");

            Saldo -= valor;
        }

        public void Creditar(decimal valor)
        {
            if (valor <= 0)
                throw new ArgumentException("Valor deve ser maior que zero");

            Saldo += valor;
        }
    }
}

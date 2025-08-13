namespace SistemaBancario.Dominio.Entidades
{
    public class Transacao
    {
        public Guid Id { get; private set; }
        public Guid IdContaOrigem { get; private set; }
        public Guid IdContaDestino { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime DataHoraTransacao { get; private set; }
        public string CodigoTransacao { get; private set; }

        public Conta ContaOrigem { get; private set; } = null!;
        public Conta ContaDestino { get; private set; } = null!;

        private Transacao(Guid id, Guid idContaOrigem, Guid idContaDestino, decimal valor,
                         DateTime dataHoraTransacao, string codigoTransacao)
        {
            Id = id;
            IdContaOrigem = idContaOrigem;
            IdContaDestino = idContaDestino;
            Valor = valor;
            DataHoraTransacao = dataHoraTransacao;
            CodigoTransacao = codigoTransacao;
        }

        public Transacao(Conta contaOrigem, Conta contaDestino, decimal valor)
        {
            Id = Guid.NewGuid();
            ContaOrigem = contaOrigem;
            ContaDestino = contaDestino;
            IdContaOrigem = contaOrigem.Id;
            IdContaDestino = contaDestino.Id;
            Valor = valor;
            DataHoraTransacao = DateTime.Now;
            CodigoTransacao = GerarCodigoTransacao();
        }

        private string GerarCodigoTransacao()
        {
            return $"TXN{DataHoraTransacao:yyyyMMddHHmmss}{Random.Shared.Next(1000, 9999)}";
        }
    }
}

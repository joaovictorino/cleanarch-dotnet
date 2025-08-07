namespace SistemaBancario.Dominio.Entidades
{
    public class Transacao
    {
        public Guid Id { get; private set; }
        public Guid IdContaOrigem { get; private set; }
        public Guid IdContaDestino { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime DataTransacao { get; private set; }
        public string CodigoTransacao { get; private set; }

        public Conta ContaOrigem { get; private set; }
        public Conta ContaDestino { get; private set; }

        public Transacao(Guid idContaOrigem, Guid idContaDestino, decimal valor)
        {
            Id = Guid.NewGuid();
            IdContaOrigem = idContaOrigem;
            IdContaDestino = idContaDestino;
            Valor = valor;
            DataTransacao = DateTime.UtcNow;
            CodigoTransacao = GerarCodigoTransacao();
        }

        private string GerarCodigoTransacao()
        {
            return $"TXN{DateTime.UtcNow:yyyyMMddHHmmss}{Random.Shared.Next(1000, 9999)}";
        }
    }
}

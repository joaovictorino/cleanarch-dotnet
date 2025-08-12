namespace SistemaBancario.Aplicacao.DTOs
{
    public class ResultadoTransferenciaDTO
    {
        public string CodigoTransacao { get; set; } = string.Empty;
        public DateTime DataTransacao { get; set; }
        public decimal Valor { get; set; }
        public string NumeroContaOrigem { get; set; } = string.Empty;
        public string NumeroContaDestino { get; set; } = string.Empty;
        public string Mensagem { get; set; } = string.Empty;
    }
}

namespace SistemaBancario.Aplicacao.DTOs
{
    public class TransferenciaDto
    {
        public string NumeroContaOrigem { get; set; } = string.Empty;
        public string NumeroContaDestino { get; set; } = string.Empty;
        public decimal Valor { get; set; }
    }
}

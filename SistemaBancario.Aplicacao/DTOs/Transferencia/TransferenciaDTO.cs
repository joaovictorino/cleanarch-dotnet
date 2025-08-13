namespace SistemaBancario.Aplicacao.DTOs.Transferencia
{
    public class TransferenciaDTO
    {
        public string NumeroContaOrigem { get; set; } = string.Empty;
        public string NumeroContaDestino { get; set; } = string.Empty;
        public decimal Valor { get; set; }
    }
}

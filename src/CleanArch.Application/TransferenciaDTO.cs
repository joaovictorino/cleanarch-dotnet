namespace CleanArch.Application
{
    public class TransferenciaDTO
    {
        public string ContaOrigem { get; set; } = string.Empty;
        public string ContaDestino { get; set; } = string.Empty;
        public decimal Valor { get; set; }
    }
}
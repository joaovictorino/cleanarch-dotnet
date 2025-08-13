namespace SistemaBancario.Aplicacao.DTOs
{
    public class ResultadoTransferenciaDTO
    {
        public string CodigoTransacao { get; set; } = string.Empty;
        public DateTime DataHoraTransacao { get; set; }
        public decimal Valor { get; set; }
        public string Mensagem { get; set; } = string.Empty;
    }
}

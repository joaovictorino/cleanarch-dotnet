namespace SistemaBancario.Aplicacao.DTOs
{
    public class CriarContaDto
    {
        public string NumeroConta { get; set; } = string.Empty;
        public string NomeCliente { get; set; } = string.Empty;
        public decimal SaldoInicial { get; set; }
    }
}

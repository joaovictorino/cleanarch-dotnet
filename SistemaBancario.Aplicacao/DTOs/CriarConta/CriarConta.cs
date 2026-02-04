namespace SistemaBancario.Aplicacao.DTOs.CriarConta
{
    public class CriarConta
    {
        public string NumeroConta { get; set; } = string.Empty;
        public string NomeCliente { get; set; } = string.Empty;
        public decimal SaldoInicial { get; set; }
    }
}

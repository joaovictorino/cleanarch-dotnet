namespace SistemaBancario.Aplicacao.DTOs.ConsultarConta
{
    public class ResultadoListar
    {
        public string Numero { get; set; } = string.Empty;
        public string NomeCliente { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
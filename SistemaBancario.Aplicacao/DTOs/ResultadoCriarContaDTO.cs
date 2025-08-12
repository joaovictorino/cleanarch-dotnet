namespace SistemaBancario.Aplicacao.DTOs
{
    public class ResultadoCriarContaDTO
    {
        public Guid Id { get; set; }
        public string NumeroConta { get; set; } = string.Empty;
        public string NomeCliente { get; set; } = string.Empty;
    }
}
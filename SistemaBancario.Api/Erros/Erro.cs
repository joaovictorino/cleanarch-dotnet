namespace SistemaBancario.Api.Erros
{
    public class Erro
    {
        public string Mensagem { get; set; }

        public Erro(string mensagem)
        {
            Mensagem = mensagem;
        }
    }
}
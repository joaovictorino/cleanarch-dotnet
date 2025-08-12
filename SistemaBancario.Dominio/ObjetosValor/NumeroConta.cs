namespace SistemaBancario.Dominio.ObjetosValor
{
    public class NumeroConta
    {
        public string Valor { get; private set; }

        public NumeroConta(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ArgumentException("Número da conta não pode ser vazio");

            if (valor.Length != 6 || !valor.All(char.IsDigit))
                throw new ArgumentException("Número da conta deve ter exatamente 6 dígitos");

            Valor = valor;
        }

        public override string ToString()
        {
            return Valor;
        }

        public static implicit operator string(NumeroConta numeroConta)
        {
            return numeroConta.Valor;
        }

        public static implicit operator NumeroConta(string valor)
        {
            return new NumeroConta(valor);
        }
    }
}

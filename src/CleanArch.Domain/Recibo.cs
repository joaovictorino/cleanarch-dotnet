
using System;

namespace CleanArch.Domain
{
    public class Recibo
    {
        public Guid Id { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime Data { get; private set; }

        public Recibo(decimal valor)
        {
            Id = Guid.NewGuid();
            Valor = valor;
            Data = DateTime.UtcNow;
        }
    }
}

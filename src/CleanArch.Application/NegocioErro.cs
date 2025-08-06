
using System;

namespace CleanArch.Application
{
    public class NegocioErro : Exception
    {
        public NegocioErro(string message) : base(message)
        {
        }
    }
}

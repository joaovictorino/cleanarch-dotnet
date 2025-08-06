
using System;
using System.Text.RegularExpressions;

namespace CleanArch.Domain
{
    public class NumeroConta
    {
        public string Value { get; private set; }

        public NumeroConta(string value)
        {
            if (!IsValid(value))
            {
                throw new ArgumentException("Número de conta inválido");
            }
            Value = value;
        }

        private static bool IsValid(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            return Regex.IsMatch(value, @"^\d{6}$");
        }

        public override string ToString()
        {
            return Value;
        }
    }
}

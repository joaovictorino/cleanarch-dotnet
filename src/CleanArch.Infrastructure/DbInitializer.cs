using CleanArch.Domain;
using System;
using System.Linq;

namespace CleanArch.Infrastructure
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Contas.Any())
            {
                return;
            }

            var contas = new Conta[]
            {
                new Conta("123456", 1000),
                new Conta("654321", 500)
            };

            foreach (Conta c in contas)
            {
                context.Contas.Add(c);
            }
            context.SaveChanges();
        }
    }
}

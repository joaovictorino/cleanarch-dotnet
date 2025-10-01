using SistemaBancario.Dominio.Entidades;
using SistemaBancario.Dominio.Interfaces;

namespace SistemaBancario.Aplicacao.Tests.Mocks;

internal class RepositorioTransacaoEmMemoria : IRepositorioTransacao
{
    private readonly List<Transacao> _transacoes = new();
    private readonly object _lock = new();

    public IReadOnlyCollection<Transacao> Transacoes
    {
        get
        {
            lock (_lock)
            {
                return _transacoes.ToList();
            }
        }
    }

    public Task AdicionarAsync(Transacao transacao)
    {
        lock (_lock)
        {
            _transacoes.Add(transacao);
        }

        return Task.CompletedTask;
    }
}

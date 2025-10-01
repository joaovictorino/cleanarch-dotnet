using SistemaBancario.Dominio.Entidades;
using SistemaBancario.Dominio.Interfaces;

namespace SistemaBancario.Aplicacao.Tests.Mocks;

internal class RepositorioContaEmMemoria : IRepositorioConta
{
    private readonly Dictionary<Guid, Conta> _contasPorId = new();
    private readonly Dictionary<string, Guid> _indicePorNumero = new(StringComparer.Ordinal);
    private readonly object _lock = new();

    public IReadOnlyCollection<Conta> Contas
    {
        get
        {
            lock (_lock)
            {
                return _contasPorId.Values.ToList();
            }
        }
    }

    public Task AdicionarAsync(Conta conta)
    {
        lock (_lock)
        {
            _contasPorId[conta.Id] = conta;
            _indicePorNumero[conta.Numero] = conta.Id;
        }

        return Task.CompletedTask;
    }

    public void Atualizar(Conta conta)
    {
        lock (_lock)
        {
            if (!_contasPorId.ContainsKey(conta.Id))
            {
                return;
            }

            _contasPorId[conta.Id] = conta;
            _indicePorNumero[conta.Numero] = conta.Id;
        }
    }

    public Task<bool> ExisteAsync(string numero)
    {
        lock (_lock)
        {
            return Task.FromResult(_indicePorNumero.ContainsKey(numero));
        }
    }

    public Task<List<Conta>> ListarAsync()
    {
        lock (_lock)
        {
            return Task.FromResult(_contasPorId.Values.Select(conta => conta).ToList());
        }
    }

    public Task<Conta?> ObterPorNumeroAsync(string numero)
    {
        lock (_lock)
        {
            if (_indicePorNumero.TryGetValue(numero, out var id) && _contasPorId.TryGetValue(id, out var conta))
            {
                return Task.FromResult<Conta?>(conta);
            }

            return Task.FromResult<Conta?>(null);
        }
    }
}

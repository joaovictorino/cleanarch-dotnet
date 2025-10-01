using SistemaBancario.Dominio.Interfaces;

namespace SistemaBancario.Aplicacao.Tests.Mocks;

internal class UnidadeTrabalhoEmMemoria : IUnidadeTrabalho
{
    public int TransacoesIniciadas { get; private set; }
    public int TransacoesConfirmadas { get; private set; }
    public int TransacoesDesfeitas { get; private set; }
    public int Salvamentos { get; private set; }

    public Task IniciarTransacaoAsync()
    {
        TransacoesIniciadas++;
        return Task.CompletedTask;
    }

    public Task ConfirmarTransacaoAsync()
    {
        TransacoesConfirmadas++;
        return Task.CompletedTask;
    }

    public Task DesfazerTransacaoAsync()
    {
        TransacoesDesfeitas++;
        return Task.CompletedTask;
    }

    public Task<int> SalvarAlteracoesAsync()
    {
        Salvamentos++;
        return Task.FromResult(0);
    }
}

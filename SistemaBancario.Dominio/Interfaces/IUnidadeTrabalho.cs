namespace SistemaBancario.Dominio.Interfaces
{
    public interface IUnidadeTrabalho
    {
        Task IniciarTransacaoAsync();
        Task<int> SalvarAlteracoesAsync();
        Task ConfirmarTransacaoAsync();
        Task DesfazerTransacaoAsync();
    }
}

namespace SistemaBancario.Dominio.Interfaces
{
    public interface IUnidadeTrabalho
    {
        Task<int> SalvarAlteracoesAsync();
        Task IniciarTransacaoAsync();
        Task ConfirmarTransacaoAsync();
        Task DesfazerTransacaoAsync();
    }
}

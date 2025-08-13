using Microsoft.EntityFrameworkCore.Storage;
using SistemaBancario.Dominio.Interfaces;
using SistemaBancario.Infraestrutura.Dados;

namespace SistemaBancario.Infraestrutura
{
    public class UnidadeTrabalho : IUnidadeTrabalho
    {
        private readonly ContextoBancario _contexto;
        private IDbContextTransaction? _transacao;

        public UnidadeTrabalho(ContextoBancario contexto)
        {
            _contexto = contexto;
        }

        public async Task<int> SalvarAlteracoesAsync()
        {
            return await _contexto.SaveChangesAsync();
        }

        public async Task IniciarTransacaoAsync()
        {
            _transacao = await _contexto.Database.BeginTransactionAsync();
        }

        public async Task ConfirmarTransacaoAsync()
        {
            try
            {
                await SalvarAlteracoesAsync();
                
                if (_transacao != null)
                    await _transacao.CommitAsync();
            }
            catch (Exception)
            {
                await DesfazerTransacaoAsync();
                throw;
            }
            finally
            {
                if(_transacao != null){
                  await _transacao.DisposeAsync();
                  _transacao = null;
                }
            }
        }

        public async Task DesfazerTransacaoAsync()
        {
          if(_transacao != null){
            await _transacao.RollbackAsync();
            await _transacao.DisposeAsync();
            _transacao = null;
          }
        }
    }
}

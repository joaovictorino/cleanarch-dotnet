using SistemaBancario.Aplicacao.DTOs;
using SistemaBancario.Dominio.Entidades;
using SistemaBancario.Dominio.Interfaces;

namespace SistemaBancario.Aplicacao.Servicos
{
    public class ServicoConta
    {

        private readonly IRepositorioConta _repositorioConta;
        private readonly IUnidadeTrabalho _unidadeTrabalho;

        public ServicoConta(IRepositorioConta repositorioConta, IUnidadeTrabalho unidadeTrabalho)
        {
            _repositorioConta = repositorioConta;
            _unidadeTrabalho = unidadeTrabalho;
        }

        public async Task<ResultadoCriarContaDTO> CriarAsync(CriarContaDTO dto)
        {
            try
            {
                await _unidadeTrabalho.IniciarTransacaoAsync();

                if (await _repositorioConta.ExisteAsync(dto.NumeroConta))
                    throw new InvalidOperationException("Já existe uma conta com este número.");

                var conta = new Conta(dto.NumeroConta, dto.NomeCliente, dto.SaldoInicial);

                await _repositorioConta.AdicionarAsync(conta);
                await _unidadeTrabalho.SalvarAlteracoesAsync();
                await _unidadeTrabalho.ConfirmarTransacaoAsync();

                return new ResultadoCriarContaDTO{
                    Id = conta.Id,
                    NumeroConta = conta.Numero,
                    NomeCliente = conta.NomeCliente 
                }; 
            }
            catch (Exception)
            {
                await _unidadeTrabalho.DesfazerTransacaoAsync();
                throw;
            }
        }

        public async Task<List<Conta>> ObterTodasContasAsync()
        {
            return await _repositorioConta.ObterTodosAsync();
        }

        public async Task<Conta?> ObterContaPorNumeroAsync(string numeroConta)
        {
            return await _repositorioConta.ObterPorNumeroAsync(numeroConta);
        }
    }
}
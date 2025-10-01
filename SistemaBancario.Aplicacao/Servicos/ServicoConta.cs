using SistemaBancario.Aplicacao.DTOs.ConsultarConta;
using SistemaBancario.Aplicacao.DTOs.CriarConta;
using SistemaBancario.Aplicacao.Interfaces;
using SistemaBancario.Dominio.Entidades;
using SistemaBancario.Dominio.Interfaces;

namespace SistemaBancario.Aplicacao.Servicos
{
    public class ServicoConta : IServicoConta
    {
        private readonly IRepositorioConta _repositorioConta;
        private readonly IUnidadeTrabalho _unidadeTrabalho;
        private readonly IMapeamentoConta _mapeamentoConta;

        public ServicoConta(
            IRepositorioConta repositorioConta,
            IUnidadeTrabalho unidadeTrabalho,
            IMapeamentoConta mapeamentoConta)
        {
            _repositorioConta = repositorioConta;
            _unidadeTrabalho = unidadeTrabalho;
            _mapeamentoConta = mapeamentoConta;
        }

        public async Task CriarAsync(CriarContaDTO dto)
        {
            try
            {
                await _unidadeTrabalho.IniciarTransacaoAsync();

                if (await _repositorioConta.ExisteAsync(dto.NumeroConta))
                    throw new InvalidOperationException("Já existe uma conta com este número.");

                var conta = new Conta(dto.NumeroConta, dto.NomeCliente, dto.SaldoInicial);

                await _repositorioConta.AdicionarAsync(conta);

                await _unidadeTrabalho.ConfirmarTransacaoAsync();
            }
            catch (Exception)
            {
                await _unidadeTrabalho.DesfazerTransacaoAsync();
                throw;
            }
        }

        public async Task<List<ResultadoListar>> ListarAsync()
        {
            List<Conta> contas = await _repositorioConta.ListarAsync();
            return _mapeamentoConta.ToDTO(contas);
        }

        public async Task<ResultadoObterPorNumero?> ObterPorNumeroAsync(string numeroConta)
        {
            Conta? conta = await _repositorioConta.ObterPorNumeroAsync(numeroConta);
            return _mapeamentoConta.ToDTO(conta);

        }
    }
}

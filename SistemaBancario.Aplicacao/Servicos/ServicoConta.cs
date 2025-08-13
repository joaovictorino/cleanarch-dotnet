using AutoMapper;
using SistemaBancario.Aplicacao.DTOs.ConsultarConta;
using SistemaBancario.Aplicacao.DTOs.CriarConta;
using SistemaBancario.Dominio.Entidades;
using SistemaBancario.Dominio.Interfaces;

namespace SistemaBancario.Aplicacao.Servicos
{
    public class ServicoConta
    {
        private readonly IRepositorioConta _repositorioConta;
        private readonly IUnidadeTrabalho _unidadeTrabalho;
        private readonly IMapper _mapper;

        public ServicoConta(
            IRepositorioConta repositorioConta,
            IUnidadeTrabalho unidadeTrabalho,
            IMapper mapper)
        {
            _repositorioConta = repositorioConta;
            _unidadeTrabalho = unidadeTrabalho;
            _mapper = mapper;
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
            var contas = await _repositorioConta.ListarAsync();
            return _mapper.Map<List<ResultadoListar>>(contas);
        }

        public async Task<ResultadoObterPorNumero?> ObterPorNumeroAsync(string numeroConta)
        {
            Conta? conta = await _repositorioConta.ObterPorNumeroAsync(numeroConta);
            return _mapper.Map<ResultadoObterPorNumero>(conta);
        }
    }
}
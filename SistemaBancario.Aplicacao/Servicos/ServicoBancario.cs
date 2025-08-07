using SistemaBancario.Aplicacao.DTOs;
using SistemaBancario.Dominio.Entidades;
using SistemaBancario.Dominio.Interfaces;
using SistemaBancario.Dominio.ObjetosValor;
using SistemaBancario.Dominio.Servicos;

namespace SistemaBancario.Aplicacao.Servicos
{
    public class ServicoBancario
    {
        private readonly IRepositorioConta _repositorioConta;
        private readonly IRepositorioTransacao _repositorioTransacao;
        private readonly IUnidadeTrabalho _unidadeTrabalho;

        public ServicoBancario(
            IRepositorioConta repositorioConta,
            IRepositorioTransacao repositorioTransacao,
            IUnidadeTrabalho unidadeTrabalho)
        {
            _repositorioConta = repositorioConta;
            _repositorioTransacao = repositorioTransacao;
            _unidadeTrabalho = unidadeTrabalho;
        }

        public async Task<Conta> CriarContaAsync(CriarContaDto dto)
        {
            var numeroConta = new NumeroConta(dto.NumeroConta);
            
            if (await _repositorioConta.ExisteAsync(numeroConta))
                throw new InvalidOperationException("Conta já existe");

            var conta = new Conta(numeroConta, dto.NomeCliente, dto.SaldoInicial);
            
            await _repositorioConta.AdicionarAsync(conta);
            await _unidadeTrabalho.SalvarAlteracoesAsync();
            
            return conta;
        }

        public async Task<ResultadoTransferenciaDto> TransferirAsync(TransferenciaDto dto)
        {
            var contaOrigem = await _repositorioConta.ObterPorNumeroAsync(dto.NumeroContaOrigem);
            var contaDestino = await _repositorioConta.ObterPorNumeroAsync(dto.NumeroContaDestino);

            if (contaOrigem == null)
                throw new InvalidOperationException("Conta de origem não encontrada");

            if (contaDestino == null)
                throw new InvalidOperationException("Conta de destino não encontrada");

            if (contaOrigem.Id == contaDestino.Id)
                throw new InvalidOperationException("Conta de origem e destino não podem ser iguais");

            TransferirValor transferencia = new TransferirValor();
            var transacao = transferencia.Transferir(contaOrigem, contaDestino, dto.Valor);
            
            await _repositorioTransacao.AdicionarAsync(transacao);
            await _repositorioConta.AtualizarAsync(contaOrigem);
            await _repositorioConta.AtualizarAsync(contaDestino);
            
            await _unidadeTrabalho.SalvarAlteracoesAsync();

            return new ResultadoTransferenciaDto
            {
                CodigoTransacao = transacao.CodigoTransacao,
                DataTransacao = transacao.DataTransacao,
                Valor = dto.Valor,
                NumeroContaOrigem = contaOrigem.Numero,
                NumeroContaDestino = contaDestino.Numero,
                Mensagem = "Transferência realizada com sucesso"
            };
        }

        public async Task<List<Conta>> ObterTodasContasAsync()
        {
            return await _repositorioConta.ObterTodosAsync();
        }

        public async Task<Conta?> ObterContaPorNumeroAsync(string numeroConta)
        {
            return await _repositorioConta.ObterPorNumeroAsync(new NumeroConta(numeroConta));
        }
    }
}

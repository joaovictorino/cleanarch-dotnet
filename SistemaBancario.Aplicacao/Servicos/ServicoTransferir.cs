using SistemaBancario.Aplicacao.DTOs;
using SistemaBancario.Dominio.Interfaces;
using SistemaBancario.Dominio.Servicos;

namespace SistemaBancario.Aplicacao.Servicos
{
    public class ServicoTransferir
    {
        private readonly IRepositorioConta _repositorioConta;
        private readonly IRepositorioTransacao _repositorioTransacao;
        private readonly IUnidadeTrabalho _unidadeTrabalho;

        public ServicoTransferir(
            IRepositorioConta repositorioConta,
            IRepositorioTransacao repositorioTransacao,
            IUnidadeTrabalho unidadeTrabalho)
        {
            _repositorioConta = repositorioConta;
            _repositorioTransacao = repositorioTransacao;
            _unidadeTrabalho = unidadeTrabalho;
        }

        public async Task<ResultadoTransferenciaDTO> TransferirAsync(TransferenciaDTO dto)
        {
            try
            {
                await _unidadeTrabalho.IniciarTransacaoAsync();

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
                await _unidadeTrabalho.ConfirmarTransacaoAsync();

                return new ResultadoTransferenciaDTO
                {
                    CodigoTransacao = transacao.CodigoTransacao,
                    DataHoraTransacao = transacao.DataHoraTransacao,
                    Valor = dto.Valor,
                    NumeroContaOrigem = contaOrigem.Numero,
                    NumeroContaDestino = contaDestino.Numero,
                    Mensagem = "Transferência realizada com sucesso"
                };
                
            }
            catch (Exception)
            {
                await _unidadeTrabalho.DesfazerTransacaoAsync();
                throw;
            }
        }
    }
}
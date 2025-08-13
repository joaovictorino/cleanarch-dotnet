using SistemaBancario.Aplicacao.DTOs.Transferencia;
using SistemaBancario.Aplicacao.Interfaces;
using SistemaBancario.Dominio.Entidades;
using SistemaBancario.Dominio.Interfaces;
using SistemaBancario.Dominio.Servicos;

namespace SistemaBancario.Aplicacao.Servicos
{
    public class ServicoTransferencia
    {
        private readonly IRepositorioConta _repositorioConta;
        private readonly IRepositorioTransacao _repositorioTransacao;
        private readonly IUnidadeTrabalho _unidadeTrabalho;
        private readonly IMapeamentoTransacao _mapeamentoTransacao;


        public ServicoTransferencia(
            IRepositorioConta repositorioConta,
            IRepositorioTransacao repositorioTransacao,
            IUnidadeTrabalho unidadeTrabalho,
            IMapeamentoTransacao mapeamentoTransacao)
        {
            _repositorioConta = repositorioConta;
            _repositorioTransacao = repositorioTransacao;
            _unidadeTrabalho = unidadeTrabalho;
            _mapeamentoTransacao = mapeamentoTransacao;
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
                Transacao transacao = transferencia.Transferir(contaOrigem, contaDestino, dto.Valor);

                await _repositorioTransacao.AdicionarAsync(transacao);
                _repositorioConta.Atualizar(contaOrigem);
                _repositorioConta.Atualizar(contaDestino);

                await _unidadeTrabalho.ConfirmarTransacaoAsync();

                return _mapeamentoTransacao.ToDTO(transacao);                
            }
            catch (Exception)
            {
                await _unidadeTrabalho.DesfazerTransacaoAsync();
                throw;
            }
        }
    }
}
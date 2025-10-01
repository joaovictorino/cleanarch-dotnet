using SistemaBancario.Aplicacao.DTOs.Transferencia;
using SistemaBancario.Aplicacao.Interfaces;
using SistemaBancario.Dominio.Entidades;
using SistemaBancario.Dominio.Interfaces;
namespace SistemaBancario.Aplicacao.Servicos
{
    public class ServicoTransferencia : IServicoTransferencia
    {
        private readonly IRepositorioConta _repositorioConta;
        private readonly IRepositorioTransacao _repositorioTransacao;
        private readonly IUnidadeTrabalho _unidadeTrabalho;
        private readonly IMapeamentoTransacao _mapeamentoTransacao;
        private readonly ITransferirValor _transferirValor;


        public ServicoTransferencia(
            IRepositorioConta repositorioConta,
            IRepositorioTransacao repositorioTransacao,
            IUnidadeTrabalho unidadeTrabalho,
            IMapeamentoTransacao mapeamentoTransacao,
            ITransferirValor transferirValor)
        {
            _repositorioConta = repositorioConta;
            _repositorioTransacao = repositorioTransacao;
            _unidadeTrabalho = unidadeTrabalho;
            _mapeamentoTransacao = mapeamentoTransacao;
            _transferirValor = transferirValor;
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

                Transacao transacao = _transferirValor.Transferir(contaOrigem, contaDestino, dto.Valor);

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

using AutoMapper;
using SistemaBancario.Aplicacao.DTOs.Transferencia;
using SistemaBancario.Dominio.Interfaces;
using SistemaBancario.Dominio.Servicos;

namespace SistemaBancario.Aplicacao.Servicos
{
    public class ServicoTransferencia
    {
        private readonly IRepositorioConta _repositorioConta;
        private readonly IRepositorioTransacao _repositorioTransacao;
        private readonly IUnidadeTrabalho _unidadeTrabalho;
        private readonly IMapper _mapper;


        public ServicoTransferencia(
            IRepositorioConta repositorioConta,
            IRepositorioTransacao repositorioTransacao,
            IUnidadeTrabalho unidadeTrabalho,
            IMapper mapper)
        {
            _repositorioConta = repositorioConta;
            _repositorioTransacao = repositorioTransacao;
            _unidadeTrabalho = unidadeTrabalho;
            _mapper = mapper;
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

                await _unidadeTrabalho.ConfirmarTransacaoAsync();

                return _mapper.Map<ResultadoTransferenciaDTO>(transacao);                
            }
            catch (Exception)
            {
                await _unidadeTrabalho.DesfazerTransacaoAsync();
                throw;
            }
        }
    }
}
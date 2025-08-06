
using System;
using System.Threading.Tasks;
using CleanArch.Domain;

namespace CleanArch.Application
{
    public class TransferenciaServico
    {
        private readonly IContaRepositorio _repositorio;
        private readonly TransferenciaValor _transferenciaValor;

        public TransferenciaServico(IContaRepositorio repositorio, TransferenciaValor transferenciaValor)
        {
            _repositorio = repositorio;
            _transferenciaValor = transferenciaValor;
        }

        public async Task<Guid> Transferir(TransferenciaDTO dto)
        {
            var contaOrigem = await _repositorio.GetByNumeroAsync(dto.ContaOrigem);
            if (contaOrigem == null)
            {
                throw new NegocioErro("Conta de origem não encontrada");
            }

            var contaDestino = await _repositorio.GetByNumeroAsync(dto.ContaDestino);
            if (contaDestino == null)
            {
                throw new NegocioErro("Conta de destino não encontrada");
            }

            var recibo = _transferenciaValor.Transferir(contaOrigem, contaDestino, dto.Valor);

            await _repositorio.SalvarAsync(contaOrigem);
            await _repositorio.SalvarAsync(contaDestino);

            return recibo.Id;
        }
    }
}

using AutoMapper;
using SistemaBancario.Aplicacao.DTOs.Transferencia;
using SistemaBancario.Aplicacao.Interfaces;
using SistemaBancario.Dominio.Entidades;

namespace SistemaBancario.Infraestrutura.Mapeamentos
{
    public class MapeamentoTransacao : IMapeamentoTransacao
    {
        private readonly IMapper _mapper;

        public MapeamentoTransacao(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ResultadoTransferencia ToDTO(Transacao transacao)
        {
            return _mapper.Map<ResultadoTransferencia>(transacao);
        }
    }
}
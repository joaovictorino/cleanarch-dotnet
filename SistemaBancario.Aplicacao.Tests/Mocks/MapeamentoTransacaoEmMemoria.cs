using SistemaBancario.Aplicacao.DTOs.Transferencia;
using SistemaBancario.Aplicacao.Interfaces;
using SistemaBancario.Dominio.Entidades;

namespace SistemaBancario.Aplicacao.Tests.Mocks;

internal class MapeamentoTransacaoEmMemoria : IMapeamentoTransacao
{
    public ResultadoTransferenciaDTO ToDTO(Transacao transacao)
    {
        return new ResultadoTransferenciaDTO
        {
            CodigoTransacao = transacao.CodigoTransacao,
            DataHoraTransacao = transacao.DataHoraTransacao
        };
    }
}

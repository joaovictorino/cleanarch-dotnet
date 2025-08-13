using SistemaBancario.Aplicacao.DTOs.Transferencia;
using SistemaBancario.Dominio.Entidades;

namespace SistemaBancario.Aplicacao.Interfaces
{
    public interface IMapeamentoTransacao
    {
        ResultadoTransferenciaDTO ToDTO(Transacao transacao);
    }
}

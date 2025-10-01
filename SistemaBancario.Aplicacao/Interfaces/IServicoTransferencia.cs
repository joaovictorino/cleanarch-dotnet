using SistemaBancario.Aplicacao.DTOs.Transferencia;

namespace SistemaBancario.Aplicacao.Interfaces;

public interface IServicoTransferencia
{
    Task<ResultadoTransferenciaDTO> TransferirAsync(TransferenciaDTO dto);
}

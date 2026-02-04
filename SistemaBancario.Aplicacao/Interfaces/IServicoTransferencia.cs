using SistemaBancario.Aplicacao.DTOs.Transferencia;

namespace SistemaBancario.Aplicacao.Interfaces;

public interface IServicoTransferencia
{
    Task<ResultadoTransferencia> TransferirAsync(Transferencia dto);
}

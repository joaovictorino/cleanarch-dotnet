using SistemaBancario.Aplicacao.DTOs.ConsultarConta;
using SistemaBancario.Aplicacao.DTOs.CriarConta;

namespace SistemaBancario.Aplicacao.Interfaces;

public interface IServicoConta
{
    Task CriarAsync(CriarConta dto);
    Task<List<ResultadoListar>> ListarAsync();
    Task<ResultadoObterPorNumero?> ObterPorNumeroAsync(string numeroConta);
}

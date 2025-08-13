using Microsoft.AspNetCore.Mvc;
using SistemaBancario.Aplicacao.Servicos;
using SistemaBancario.Aplicacao.DTOs.CriarConta;
using SistemaBancario.Aplicacao.DTOs.Transferencia;
using SistemaBancario.Api.Erros;
using SistemaBancario.Aplicacao.DTOs.ConsultarConta;

namespace SistemaBancario.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContasController : ControllerBase
    {
        private readonly ServicoConta _servicoConta;
        private readonly ServicoTransferencia _servicoTransferencia;

        public ContasController(ServicoConta servicoConta, ServicoTransferencia servicoTransferencia)
        {
            _servicoConta = servicoConta;
            _servicoTransferencia = servicoTransferencia;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ResultadoListar>))]
        [ProducesResponseType(500, Type = typeof(Erro))]
        public async Task<IActionResult> Listar()
        {
            var contas = await _servicoConta.ListarAsync();
            return Ok(contas);
        }

        [HttpGet("{numeroConta}")]
        [ProducesResponseType(200, Type = typeof(ResultadoObterPorNumero))]
        [ProducesResponseType(500, Type = typeof(Erro))]
        public async Task<IActionResult> ObterPorNumero(string numeroConta)
        {
            var conta = await _servicoConta.ObterPorNumeroAsync(numeroConta);

            if (conta == null)
                return NotFound("Conta não encontrada");

            return Ok(conta);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400, Type = typeof(Erro))]
        [ProducesResponseType(500, Type = typeof(Erro))]
        public async Task<IActionResult> Criar([FromBody] CriarContaDTO dto)
        {
            await _servicoConta.CriarAsync(dto);
            return Created();
        }

        [HttpPost("/transferir")]
        [ProducesResponseType(200, Type = typeof(ResultadoTransferenciaDTO))]
        [ProducesResponseType(400, Type = typeof(Erro))]
        [ProducesResponseType(500, Type = typeof(Erro))]
        public async Task<IActionResult> Transferir([FromBody] TransferenciaDTO dto)
        {
            var resultado = await _servicoTransferencia.TransferirAsync(dto);
            return Ok(resultado);
        }
    }
}

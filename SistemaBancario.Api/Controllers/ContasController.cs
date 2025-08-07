using Microsoft.AspNetCore.Mvc;
using SistemaBancario.Aplicacao.Servicos;
using SistemaBancario.Aplicacao.DTOs;

namespace SistemaBancario.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContasController : ControllerBase
    {
        private readonly ServicoBancario _servicoBancario;

        public ContasController(ServicoBancario servicoBancario)
        {
            _servicoBancario = servicoBancario;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodas()
        {
            try
            {
                var contas = await _servicoBancario.ObterTodasContasAsync();
                return Ok(contas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{numeroConta}")]
        public async Task<IActionResult> ObterPorNumero(string numeroConta)
        {
            try
            {
                var conta = await _servicoBancario.ObterContaPorNumeroAsync(numeroConta);
                
                if (conta == null)
                    return NotFound("Conta não encontrada");

                return Ok(conta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarContaDto dto)
        {
            try
            {
                var conta = await _servicoBancario.CriarContaAsync(dto);
                return CreatedAtAction(nameof(ObterPorNumero), 
                    new { numeroConta = conta.Numero.Valor }, conta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/transferir")]
        public async Task<IActionResult> Transferir([FromBody] TransferenciaDto dto)
        {
            try
            {
                var resultado = await _servicoBancario.TransferirAsync(dto);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

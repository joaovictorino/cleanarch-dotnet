using Microsoft.AspNetCore.Mvc;
using SistemaBancario.Aplicacao.Servicos;
using SistemaBancario.Aplicacao.DTOs;

namespace SistemaBancario.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContasController : ControllerBase
    {
        private readonly ServicoConta _servicoConta;
        private readonly ServicoTransferir _servicoTransferir;

        public ContasController(ServicoConta servicoConta, ServicoTransferir servicoTransferir)
        {
            _servicoConta = servicoConta;
            _servicoTransferir = servicoTransferir;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodas()
        {
            try
            {
                var contas = await _servicoConta.ObterTodasContasAsync();
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
                var conta = await _servicoConta.ObterContaPorNumeroAsync(numeroConta);
                
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
        public async Task<IActionResult> Criar([FromBody] CriarContaDTO dto)
        {
            try
            {
                var conta = await _servicoConta.CriarAsync(dto);
                return CreatedAtAction(nameof(ObterPorNumero), 
                    new { numeroConta = conta.NumeroConta }, conta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/transferir")]
        public async Task<IActionResult> Transferir([FromBody] TransferenciaDTO dto)
        {
            try
            {
                var resultado = await _servicoTransferir.TransferirAsync(dto);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

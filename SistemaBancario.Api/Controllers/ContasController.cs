using Microsoft.AspNetCore.Mvc;
using SistemaBancario.Aplicacao.Servicos;
using SistemaBancario.Aplicacao.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;

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
        public async Task<IActionResult> Listar()
        {
            try
            {
                var contas = await _servicoConta.ListarAsync();
                return Ok(contas);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpGet("{numeroConta}")]
        public async Task<IActionResult> ObterPorNumero(string numeroConta)
        {
            try
            {
                var conta = await _servicoConta.ObterPorNumeroAsync(numeroConta);
                
                if (conta == null)
                    return NotFound("Conta não encontrada");

                return Ok(conta);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
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
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpPost("/transferir")]
        public async Task<IActionResult> Transferir([FromBody] TransferenciaDTO dto)
        {
            try
            {
                var resultado = await _servicoTransferencia.TransferirAsync(dto);
                return Ok(resultado);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
            }
        }
    }
}

using System;
using System.Threading.Tasks;
using CleanArch.Application;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferenciaController : ControllerBase
    {
        private readonly TransferenciaServico _transferenciaServico;

        public TransferenciaController(TransferenciaServico transferenciaServico)
        {
            _transferenciaServico = transferenciaServico;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TransferenciaDTO dto)
        {
            try
            {
                var reciboId = await _transferenciaServico.Transferir(dto);
                return Ok(new { ReciboId = reciboId });
            }
            catch (NegocioErro ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Error = "Ocorreu um erro inesperado." });
            }
        }
    }
}

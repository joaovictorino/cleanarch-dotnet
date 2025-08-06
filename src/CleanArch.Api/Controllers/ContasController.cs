using CleanArch.Application;
using CleanArch.Domain;
using CleanArch.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArch.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContasController : ControllerBase
    {
        private readonly IContaRepositorio _contaRepositorio;

        public ContasController(IContaRepositorio contaRepositorio)
        {
            _contaRepositorio = contaRepositorio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContaDTO>>> GetContas()
        {
            var contas = await _contaRepositorio.GetAllAsync();
            return Ok(contas);
        }

        [HttpGet("{numero}")]
        public async Task<ActionResult<ContaDTO>> GetConta(string numero)
        {
            var conta = await _contaRepositorio.GetByNumeroAsync(numero);
            if (conta == null)
            {
                return NotFound();
            }
            return Ok(conta);
        }

        [HttpPost]
        public async Task<ActionResult<ContaDTO>> PostConta(ContaDTO createContaDto)
        {
            Conta conta = new Conta(createContaDto.Numero, createContaDto.Saldo);
            await _contaRepositorio.SalvarAsync(conta);
            return Ok(new { mensagem = "sucesso" });
        }
    }
}

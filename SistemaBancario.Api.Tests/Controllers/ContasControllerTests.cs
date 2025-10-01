using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SistemaBancario.Api.Controllers;
using SistemaBancario.Api.Erros;
using SistemaBancario.Aplicacao.DTOs.ConsultarConta;
using SistemaBancario.Aplicacao.DTOs.CriarConta;
using SistemaBancario.Aplicacao.DTOs.Transferencia;
using SistemaBancario.Aplicacao.Interfaces;

namespace SistemaBancario.Api.Tests.Controllers;

public class ContasControllerTests
{
    [Fact]
    public void Controller_DeveEstarConfiguradaComAtributosDeApi()
    {
        var controllerType = typeof(ContasController);

        Assert.NotNull(controllerType.GetCustomAttribute<ApiControllerAttribute>());

        var routeAttribute = controllerType.GetCustomAttribute<RouteAttribute>();
        Assert.NotNull(routeAttribute);
        Assert.Equal("api/[controller]", routeAttribute!.Template);
    }

    [Fact]
    public async Task Listar_DeveRetornarOkComColecaoDeContas()
    {
        var contas = new List<ResultadoListar>
        {
            new() { Numero = "123", NomeCliente = "Cliente 1", Saldo = 10m },
            new() { Numero = "456", NomeCliente = "Cliente 2", Saldo = 20m }
        };
        var servicoContaMock = new Mock<IServicoConta>();
        servicoContaMock.Setup(s => s.ListarAsync()).ReturnsAsync(contas);

        var controller = CriarController(servicoContaMock: servicoContaMock);

        IActionResult resultado = await controller.Listar();

        var okResult = Assert.IsType<OkObjectResult>(resultado);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode ?? StatusCodes.Status200OK);
        Assert.Same(contas, okResult.Value);
        servicoContaMock.Verify(s => s.ListarAsync(), Times.Once);
    }

    [Fact]
    public async Task ObterPorNumero_ContaExistente_DeveRetornarOk()
    {
        var conta = new ResultadoObterPorNumero
        {
            Numero = "789",
            NomeCliente = "Cliente 3",
            Saldo = 30m,
            QuantidadeTransacoes = 2
        };
        var servicoContaMock = new Mock<IServicoConta>();
        servicoContaMock.Setup(s => s.ObterPorNumeroAsync(conta.Numero)).ReturnsAsync(conta);

        var controller = CriarController(servicoContaMock: servicoContaMock);

        IActionResult resultado = await controller.ObterPorNumero(conta.Numero);

        var okResult = Assert.IsType<OkObjectResult>(resultado);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode ?? StatusCodes.Status200OK);
        Assert.Same(conta, okResult.Value);
    }

    [Fact]
    public async Task ObterPorNumero_ContaInexistente_DeveRetornarNotFound()
    {
        var servicoContaMock = new Mock<IServicoConta>();
        servicoContaMock.Setup(s => s.ObterPorNumeroAsync(It.IsAny<string>())).ReturnsAsync((ResultadoObterPorNumero?)null);

        var controller = CriarController(servicoContaMock: servicoContaMock);

        IActionResult resultado = await controller.ObterPorNumero("000");

        var notFound = Assert.IsType<NotFoundObjectResult>(resultado);
        Assert.Equal(StatusCodes.Status404NotFound, notFound.StatusCode ?? StatusCodes.Status404NotFound);
        Assert.Equal("Conta não encontrada", notFound.Value);
    }

    [Fact]
    public async Task Criar_DeveRetornarCreatedQuandoSucesso()
    {
        var dto = new CriarContaDTO
        {
            NumeroConta = "111",
            NomeCliente = "Cliente Novo",
            SaldoInicial = 100m
        };
        var servicoContaMock = new Mock<IServicoConta>();

        var controller = CriarController(servicoContaMock: servicoContaMock);

        IActionResult resultado = await controller.Criar(dto);

        var created = Assert.IsType<CreatedResult>(resultado);
        Assert.Equal(StatusCodes.Status201Created, created.StatusCode ?? StatusCodes.Status201Created);
        servicoContaMock.Verify(s => s.CriarAsync(It.Is<CriarContaDTO>(c => c == dto)), Times.Once);
    }

    [Fact]
    public async Task Transferir_DeveRetornarOkComResultado()
    {
        var dto = new TransferenciaDTO
        {
            NumeroContaDestino = "222",
            NumeroContaOrigem = "111",
            Valor = 50m
        };
        var resultadoEsperado = new ResultadoTransferenciaDTO
        {
            CodigoTransacao = Guid.NewGuid().ToString(),
            DataHoraTransacao = DateTime.UtcNow
        };

        var servicoTransferenciaMock = new Mock<IServicoTransferencia>();
        servicoTransferenciaMock.Setup(s => s.TransferirAsync(It.Is<TransferenciaDTO>(t => t == dto))).ReturnsAsync(resultadoEsperado);

        var controller = CriarController(servicoTransferenciaMock: servicoTransferenciaMock);

        IActionResult resultado = await controller.Transferir(dto);

        var okResult = Assert.IsType<OkObjectResult>(resultado);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode ?? StatusCodes.Status200OK);
        Assert.Same(resultadoEsperado, okResult.Value);
        servicoTransferenciaMock.Verify(s => s.TransferirAsync(It.Is<TransferenciaDTO>(t => t == dto)), Times.Once);
    }

    [Fact]
    public void Listar_DeveDeclararContratoRestCorreto()
    {
        var metodo = typeof(ContasController).GetMethod(nameof(ContasController.Listar));
        Assert.NotNull(metodo);

        Assert.Single(metodo!.GetCustomAttributes(typeof(HttpGetAttribute), inherit: false));

        var responseTypes = metodo.GetCustomAttributes(typeof(ProducesResponseTypeAttribute), inherit: false)
            .Cast<ProducesResponseTypeAttribute>()
            .ToList();

        Assert.Contains(responseTypes, attr => attr.StatusCode == StatusCodes.Status200OK && attr.Type == typeof(List<ResultadoListar>));
        Assert.Contains(responseTypes, attr => attr.StatusCode == StatusCodes.Status500InternalServerError && attr.Type == typeof(Erro));
    }

    [Fact]
    public void ObterPorNumero_DeveDeclararContratoRestCorreto()
    {
        var metodo = typeof(ContasController).GetMethod(nameof(ContasController.ObterPorNumero));
        Assert.NotNull(metodo);

        Assert.Single(metodo!.GetCustomAttributes(typeof(HttpGetAttribute), inherit: false));

        var responseTypes = metodo.GetCustomAttributes(typeof(ProducesResponseTypeAttribute), inherit: false)
            .Cast<ProducesResponseTypeAttribute>()
            .ToList();

        Assert.Contains(responseTypes, attr => attr.StatusCode == StatusCodes.Status200OK && attr.Type == typeof(ResultadoObterPorNumero));
        Assert.Contains(responseTypes, attr => attr.StatusCode == StatusCodes.Status500InternalServerError && attr.Type == typeof(Erro));
    }

    [Fact]
    public void Criar_DeveDeclararContratoRestCorreto()
    {
        var metodo = typeof(ContasController).GetMethod(nameof(ContasController.Criar));
        Assert.NotNull(metodo);

        Assert.Single(metodo!.GetCustomAttributes(typeof(HttpPostAttribute), inherit: false));

        var responseTypes = metodo.GetCustomAttributes(typeof(ProducesResponseTypeAttribute), inherit: false)
            .Cast<ProducesResponseTypeAttribute>()
            .ToList();

        Assert.Contains(responseTypes, attr => attr.StatusCode == StatusCodes.Status201Created);
        Assert.Contains(responseTypes, attr => attr.StatusCode == StatusCodes.Status400BadRequest && attr.Type == typeof(Erro));
        Assert.Contains(responseTypes, attr => attr.StatusCode == StatusCodes.Status500InternalServerError && attr.Type == typeof(Erro));
    }

    [Fact]
    public void Transferir_DeveDeclararContratoRestCorreto()
    {
        var metodo = typeof(ContasController).GetMethod(nameof(ContasController.Transferir));
        Assert.NotNull(metodo);

        var httpPost = metodo!.GetCustomAttributes(typeof(HttpPostAttribute), inherit: false)
            .Cast<HttpPostAttribute>()
            .SingleOrDefault();
        Assert.NotNull(httpPost);
        Assert.Equal("/transferir", httpPost!.Template);

        var responseTypes = metodo.GetCustomAttributes(typeof(ProducesResponseTypeAttribute), inherit: false)
            .Cast<ProducesResponseTypeAttribute>()
            .ToList();

        Assert.Contains(responseTypes, attr => attr.StatusCode == StatusCodes.Status200OK && attr.Type == typeof(ResultadoTransferenciaDTO));
        Assert.Contains(responseTypes, attr => attr.StatusCode == StatusCodes.Status400BadRequest && attr.Type == typeof(Erro));
        Assert.Contains(responseTypes, attr => attr.StatusCode == StatusCodes.Status500InternalServerError && attr.Type == typeof(Erro));
    }

    private static ContasController CriarController(
        Mock<IServicoConta>? servicoContaMock = null,
        Mock<IServicoTransferencia>? servicoTransferenciaMock = null)
    {
        var conta = servicoContaMock ?? new Mock<IServicoConta>();
        var transferencia = servicoTransferenciaMock ?? new Mock<IServicoTransferencia>();
        return new ContasController(conta.Object, transferencia.Object);
    }
}

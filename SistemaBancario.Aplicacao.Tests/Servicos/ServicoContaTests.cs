using SistemaBancario.Aplicacao.DTOs.CriarConta;
using SistemaBancario.Aplicacao.DTOs.ConsultarConta;
using SistemaBancario.Aplicacao.Servicos;
using SistemaBancario.Aplicacao.Tests.Mocks;
using SistemaBancario.Dominio.Entidades;

namespace SistemaBancario.Aplicacao.Tests.Servicos;

public class ServicoContaTests
{
    [Fact]
    public async Task CriarAsync_ContaInexistente_DevePersistirEConfirmarTransacao()
    {
        var repositorio = new RepositorioContaEmMemoria();
        var unidadeTrabalho = new UnidadeTrabalhoEmMemoria();
        var mapeamento = new MapeamentoContaEmMemoria();
        var servico = new ServicoConta(repositorio, unidadeTrabalho, mapeamento);
        var dto = new CriarContaDTO
        {
            NumeroConta = "123456",
            NomeCliente = "Cliente Teste",
            SaldoInicial = 100m
        };

        await servico.CriarAsync(dto);

        Assert.Single(repositorio.Contas);
        var conta = repositorio.Contas.First();
        Assert.Equal(dto.NumeroConta, conta.Numero);
        Assert.Equal(dto.NomeCliente, conta.NomeCliente);
        Assert.Equal(dto.SaldoInicial, conta.Saldo);
        Assert.Equal(1, unidadeTrabalho.TransacoesIniciadas);
        Assert.Equal(1, unidadeTrabalho.TransacoesConfirmadas);
        Assert.Equal(0, unidadeTrabalho.TransacoesDesfeitas);
    }

    [Fact]
    public async Task CriarAsync_ContaJaExiste_DeveDesfazerTransacao()
    {
        var repositorio = new RepositorioContaEmMemoria();
        var unidadeTrabalho = new UnidadeTrabalhoEmMemoria();
        var mapeamento = new MapeamentoContaEmMemoria();
        var servico = new ServicoConta(repositorio, unidadeTrabalho, mapeamento);
        var contaExistente = new Conta("654321", "Cliente Existente", 50m);
        await repositorio.AdicionarAsync(contaExistente);

        var dto = new CriarContaDTO
        {
            NumeroConta = contaExistente.Numero,
            NomeCliente = "Cliente Diferente",
            SaldoInicial = 200m
        };

        var excecao = await Assert.ThrowsAsync<InvalidOperationException>(() => servico.CriarAsync(dto));
        Assert.Equal("Já existe uma conta com este número.", excecao.Message);
        Assert.Single(repositorio.Contas);
        Assert.Equal(1, unidadeTrabalho.TransacoesIniciadas);
        Assert.Equal(0, unidadeTrabalho.TransacoesConfirmadas);
        Assert.Equal(1, unidadeTrabalho.TransacoesDesfeitas);
    }

    [Fact]
    public async Task ListarAsync_DeveRetornarDtosMapeados()
    {
        var repositorio = new RepositorioContaEmMemoria();
        var unidadeTrabalho = new UnidadeTrabalhoEmMemoria();
        var mapeamento = new MapeamentoContaEmMemoria();
        var servico = new ServicoConta(repositorio, unidadeTrabalho, mapeamento);

        var conta1 = new Conta("111111", "Cliente 1", 10m);
        var conta2 = new Conta("222222", "Cliente 2", 20m);
        await repositorio.AdicionarAsync(conta1);
        await repositorio.AdicionarAsync(conta2);

        List<ResultadoListar> resultado = await servico.ListarAsync();

        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, dto => dto.Numero == conta1.Numero && dto.Saldo == conta1.Saldo);
        Assert.Contains(resultado, dto => dto.Numero == conta2.Numero && dto.NomeCliente == conta2.NomeCliente);
    }

    [Fact]
    public async Task ObterPorNumeroAsync_ContaExistente_DeveRetornarDto()
    {
        var repositorio = new RepositorioContaEmMemoria();
        var unidadeTrabalho = new UnidadeTrabalhoEmMemoria();
        var mapeamento = new MapeamentoContaEmMemoria();
        var servico = new ServicoConta(repositorio, unidadeTrabalho, mapeamento);

        var conta = new Conta("333333", "Cliente 3", 30m);
        conta.TransacoesOrigem.Add(new Transacao(conta, new Conta("444444", "Cliente 4", 40m), 5m));
        conta.TransacoesDestino.Add(new Transacao(new Conta("555555", "Cliente 5", 50m), conta, 10m));
        await repositorio.AdicionarAsync(conta);

        ResultadoObterPorNumero? resultado = await servico.ObterPorNumeroAsync(conta.Numero);

        Assert.NotNull(resultado);
        Assert.Equal(conta.Numero, resultado!.Numero);
        Assert.Equal(conta.Saldo, resultado.Saldo);
        Assert.Equal(2, resultado.QuantidadeTransacoes);
    }

    [Fact]
    public async Task ObterPorNumeroAsync_ContaInexistente_DeveRetornarNull()
    {
        var repositorio = new RepositorioContaEmMemoria();
        var unidadeTrabalho = new UnidadeTrabalhoEmMemoria();
        var mapeamento = new MapeamentoContaEmMemoria();
        var servico = new ServicoConta(repositorio, unidadeTrabalho, mapeamento);

        ResultadoObterPorNumero? resultado = await servico.ObterPorNumeroAsync("999999");

        Assert.Null(resultado);
    }
}

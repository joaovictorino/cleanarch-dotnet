using SistemaBancario.Aplicacao.DTOs.Transferencia;
using SistemaBancario.Aplicacao.Servicos;
using SistemaBancario.Aplicacao.Tests.Mocks;
using SistemaBancario.Dominio.Entidades;
using SistemaBancario.Dominio.Servicos;

namespace SistemaBancario.Aplicacao.Tests.Servicos;

public class ServicoTransferenciaTests
{
    [Fact]
    public async Task TransferirAsync_DadosValidos_DeveDebitarCreditarEPersistir()
    {
        var repositorioConta = new RepositorioContaEmMemoria();
        var repositorioTransacao = new RepositorioTransacaoEmMemoria();
        var unidadeTrabalho = new UnidadeTrabalhoEmMemoria();
        var mapeamento = new MapeamentoTransacaoEmMemoria();
        var transferirValor = new TransferirValor();
        var servico = new ServicoTransferencia(repositorioConta, repositorioTransacao, unidadeTrabalho, mapeamento, transferirValor);

        var contaOrigem = new Conta("111111", "Origem", 100m);
        var contaDestino = new Conta("222222", "Destino", 50m);
        await repositorioConta.AdicionarAsync(contaOrigem);
        await repositorioConta.AdicionarAsync(contaDestino);

        var dto = new TransferenciaDTO
        {
            NumeroContaOrigem = contaOrigem.Numero,
            NumeroContaDestino = contaDestino.Numero,
            Valor = 40m
        };

        var resultado = await servico.TransferirAsync(dto);

        Assert.Equal(60m, contaOrigem.Saldo);
        Assert.Equal(90m, contaDestino.Saldo);
        Assert.Single(repositorioTransacao.Transacoes);
        Assert.False(string.IsNullOrWhiteSpace(resultado.CodigoTransacao));
        Assert.Equal(repositorioTransacao.Transacoes.First().CodigoTransacao, resultado.CodigoTransacao);
        Assert.Equal(1, unidadeTrabalho.TransacoesIniciadas);
        Assert.Equal(1, unidadeTrabalho.TransacoesConfirmadas);
        Assert.Equal(0, unidadeTrabalho.TransacoesDesfeitas);
    }

    [Fact]
    public async Task TransferirAsync_ContaOrigemInexistente_DeveDesfazerTransacao()
    {
        var repositorioConta = new RepositorioContaEmMemoria();
        var repositorioTransacao = new RepositorioTransacaoEmMemoria();
        var unidadeTrabalho = new UnidadeTrabalhoEmMemoria();
        var mapeamento = new MapeamentoTransacaoEmMemoria();
        var transferirValor = new TransferirValor();
        var servico = new ServicoTransferencia(repositorioConta, repositorioTransacao, unidadeTrabalho, mapeamento, transferirValor);

        var dto = new TransferenciaDTO
        {
            NumeroContaOrigem = "333333",
            NumeroContaDestino = "444444",
            Valor = 10m
        };

        var excecao = await Assert.ThrowsAsync<InvalidOperationException>(() => servico.TransferirAsync(dto));
        Assert.Equal("Conta de origem não encontrada", excecao.Message);
        Assert.Equal(1, unidadeTrabalho.TransacoesIniciadas);
        Assert.Equal(0, unidadeTrabalho.TransacoesConfirmadas);
        Assert.Equal(1, unidadeTrabalho.TransacoesDesfeitas);
        Assert.Empty(repositorioTransacao.Transacoes);
    }

    [Fact]
    public async Task TransferirAsync_ContaDestinoInexistente_DeveDesfazerTransacao()
    {
        var repositorioConta = new RepositorioContaEmMemoria();
        var repositorioTransacao = new RepositorioTransacaoEmMemoria();
        var unidadeTrabalho = new UnidadeTrabalhoEmMemoria();
        var mapeamento = new MapeamentoTransacaoEmMemoria();
        var transferirValor = new TransferirValor();
        var servico = new ServicoTransferencia(repositorioConta, repositorioTransacao, unidadeTrabalho, mapeamento, transferirValor);

        var contaOrigem = new Conta("123123", "Origem", 100m);
        await repositorioConta.AdicionarAsync(contaOrigem);

        var dto = new TransferenciaDTO
        {
            NumeroContaOrigem = contaOrigem.Numero,
            NumeroContaDestino = "999999",
            Valor = 10m
        };

        var excecao = await Assert.ThrowsAsync<InvalidOperationException>(() => servico.TransferirAsync(dto));
        Assert.Equal("Conta de destino não encontrada", excecao.Message);
        Assert.Equal(1, unidadeTrabalho.TransacoesIniciadas);
        Assert.Equal(0, unidadeTrabalho.TransacoesConfirmadas);
        Assert.Equal(1, unidadeTrabalho.TransacoesDesfeitas);
        Assert.Empty(repositorioTransacao.Transacoes);
    }

    [Fact]
    public async Task TransferirAsync_MesmaConta_DeveDesfazerTransacao()
    {
        var repositorioConta = new RepositorioContaEmMemoria();
        var repositorioTransacao = new RepositorioTransacaoEmMemoria();
        var unidadeTrabalho = new UnidadeTrabalhoEmMemoria();
        var mapeamento = new MapeamentoTransacaoEmMemoria();
        var transferirValor = new TransferirValor();
        var servico = new ServicoTransferencia(repositorioConta, repositorioTransacao, unidadeTrabalho, mapeamento, transferirValor);

        var conta = new Conta("555555", "Cliente", 100m);
        await repositorioConta.AdicionarAsync(conta);

        var dto = new TransferenciaDTO
        {
            NumeroContaOrigem = conta.Numero,
            NumeroContaDestino = conta.Numero,
            Valor = 10m
        };

        var excecao = await Assert.ThrowsAsync<InvalidOperationException>(() => servico.TransferirAsync(dto));
        Assert.Equal("Conta de origem e destino não podem ser iguais", excecao.Message);
        Assert.Equal(1, unidadeTrabalho.TransacoesIniciadas);
        Assert.Equal(0, unidadeTrabalho.TransacoesConfirmadas);
        Assert.Equal(1, unidadeTrabalho.TransacoesDesfeitas);
        Assert.Empty(repositorioTransacao.Transacoes);
    }

    [Fact]
    public async Task TransferirAsync_SaldoInsuficiente_DeveDesfazerTransacao()
    {
        var repositorioConta = new RepositorioContaEmMemoria();
        var repositorioTransacao = new RepositorioTransacaoEmMemoria();
        var unidadeTrabalho = new UnidadeTrabalhoEmMemoria();
        var mapeamento = new MapeamentoTransacaoEmMemoria();
        var transferirValor = new TransferirValor();
        var servico = new ServicoTransferencia(repositorioConta, repositorioTransacao, unidadeTrabalho, mapeamento, transferirValor);

        var contaOrigem = new Conta("666666", "Origem", 20m);
        var contaDestino = new Conta("777777", "Destino", 0m);
        await repositorioConta.AdicionarAsync(contaOrigem);
        await repositorioConta.AdicionarAsync(contaDestino);

        var dto = new TransferenciaDTO
        {
            NumeroContaOrigem = contaOrigem.Numero,
            NumeroContaDestino = contaDestino.Numero,
            Valor = 50m
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => servico.TransferirAsync(dto));
        Assert.Equal(20m, contaOrigem.Saldo);
        Assert.Equal(0m, contaDestino.Saldo);
        Assert.Equal(1, unidadeTrabalho.TransacoesIniciadas);
        Assert.Equal(0, unidadeTrabalho.TransacoesConfirmadas);
        Assert.Equal(1, unidadeTrabalho.TransacoesDesfeitas);
        Assert.Empty(repositorioTransacao.Transacoes);
    }
}

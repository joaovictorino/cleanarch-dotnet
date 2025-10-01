using SistemaBancario.Dominio.Entidades;
using SistemaBancario.Dominio.Servicos;

namespace SistemaBancario.Dominio.Tests.Servicos;

public class TransferirValorTests
{
    [Fact]
    public void Transferir_WhenDadosValidos_DeveAtualizarSaldosEGerarTransacao()
    {
        var contaOrigem = new Conta("123456", "Cliente Origem", 500m);
        var contaDestino = new Conta("654321", "Cliente Destino", 200m);
        var servico = new TransferirValor();

        var transacao = servico.Transferir(contaOrigem, contaDestino, 150m);

        Assert.Equal(350m, contaOrigem.Saldo);
        Assert.Equal(350m, contaDestino.Saldo);
        Assert.Equal(contaOrigem.Id, transacao.IdContaOrigem);
        Assert.Equal(contaDestino.Id, transacao.IdContaDestino);
        Assert.Equal(150m, transacao.Valor);
        Assert.Same(contaOrigem, transacao.ContaOrigem);
        Assert.Same(contaDestino, transacao.ContaDestino);
        Assert.StartsWith("TXN", transacao.CodigoTransacao);
        Assert.True(transacao.DataHoraTransacao > DateTime.Now.AddMinutes(-1));
        Assert.NotEqual(Guid.Empty, transacao.Id);
    }

    [Fact]
    public void Transferir_QuandoContaOrigemNula_DeveLancarArgumentNullException()
    {
        var contaDestino = new Conta("654321", "Cliente Destino", 200m);
        var servico = new TransferirValor();

        var exception = Assert.Throws<ArgumentNullException>(() => servico.Transferir(null!, contaDestino, 100m));

        Assert.Equal("origem", exception.ParamName);
    }

    [Fact]
    public void Transferir_QuandoContaDestinoNula_DeveLancarArgumentNullException()
    {
        var contaOrigem = new Conta("123456", "Cliente Origem", 200m);
        var servico = new TransferirValor();

        var exception = Assert.Throws<ArgumentNullException>(() => servico.Transferir(contaOrigem, null!, 100m));

        Assert.Equal("destino", exception.ParamName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-15)]
    public void Transferir_QuandoValorNaoEhPositivo_DeveLancarArgumentException(decimal valor)
    {
        var contaOrigem = new Conta("123456", "Cliente Origem", 200m);
        var contaDestino = new Conta("654321", "Cliente Destino", 200m);
        var servico = new TransferirValor();

        var exception = Assert.Throws<ArgumentException>(() => servico.Transferir(contaOrigem, contaDestino, valor));

        Assert.Equal("O valor da transferência deve ser positivo.", exception.Message);
    }

    [Fact]
    public void Transferir_QuandoSaldoInsuficiente_DevePropagarInvalidOperationException()
    {
        var contaOrigem = new Conta("123456", "Cliente Origem", 50m);
        var contaDestino = new Conta("654321", "Cliente Destino", 200m);
        var servico = new TransferirValor();

        var exception = Assert.Throws<InvalidOperationException>(() => servico.Transferir(contaOrigem, contaDestino, 100m));

        Assert.Equal("Saldo insuficiente para operação.", exception.Message);
    }
}

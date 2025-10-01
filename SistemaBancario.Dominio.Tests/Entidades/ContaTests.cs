using SistemaBancario.Dominio.Entidades;

namespace SistemaBancario.Dominio.Tests.Entidades;

public class ContaTests
{
    [Fact]
    public void Depositar_QuandoValorPositivo_DeveAumentarSaldo()
    {
        var conta = new Conta("123456", "Cliente Teste", 100m);

        conta.Depositar(50m);

        Assert.Equal(150m, conta.Saldo);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Depositar_QuandoValorNaoPositivo_DeveLancarArgumentException(decimal valor)
    {
        var conta = new Conta("123456", "Cliente Teste", 100m);

        var exception = Assert.Throws<ArgumentException>(() => conta.Depositar(valor));

        Assert.Equal("O valor do depósito deve ser positivo.", exception.Message);
    }

    [Fact]
    public void Sacar_QuandoValorPositivoESaldoSuficiente_DeveReduzirSaldo()
    {
        var conta = new Conta("123456", "Cliente Teste", 200m);

        conta.Sacar(50m);

        Assert.Equal(150m, conta.Saldo);
    }

    [Fact]
    public void Sacar_QuandoValorIgualSaldo_DeveZerarSaldo()
    {
        var conta = new Conta("123456", "Cliente Teste", 200m);

        conta.Sacar(200m);

        Assert.Equal(0m, conta.Saldo);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Sacar_QuandoValorNaoPositivo_DeveLancarArgumentException(decimal valor)
    {
        var conta = new Conta("123456", "Cliente Teste", 200m);

        var exception = Assert.Throws<ArgumentException>(() => conta.Sacar(valor));

        Assert.Equal("O valor do saque deve ser positivo.", exception.Message);
    }

    [Fact]
    public void Sacar_QuandoValorMaiorQueSaldo_DeveLancarInvalidOperationException()
    {
        var conta = new Conta("123456", "Cliente Teste", 100m);

        var exception = Assert.Throws<InvalidOperationException>(() => conta.Sacar(150m));

        Assert.Equal("Saldo insuficiente para operação.", exception.Message);
    }
}

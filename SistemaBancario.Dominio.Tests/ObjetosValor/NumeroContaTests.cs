using SistemaBancario.Dominio.ObjetosValor;

namespace SistemaBancario.Dominio.Tests.ObjetosValor;

public class NumeroContaTests
{
    [Fact]
    public void Constructor_WhenValorIsValid_ShouldCreateNumeroConta()
    {
        var numeroConta = new NumeroConta("123456");

        Assert.Equal("123456", numeroConta.Valor);
        Assert.Equal("123456", numeroConta.ToString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WhenValorIsNullOrWhitespace_ShouldThrowArgumentException(string? value)
    {
        var exception = Assert.Throws<ArgumentException>(() => new NumeroConta(value!));

        Assert.Equal("Número da conta não pode ser vazio", exception.Message);
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("1234567")]
    [InlineData("12A456")]
    public void Constructor_WhenValorHasInvalidFormat_ShouldThrowArgumentException(string value)
    {
        var exception = Assert.Throws<ArgumentException>(() => new NumeroConta(value));

        Assert.Equal("Número da conta deve ter exatamente 6 dígitos", exception.Message);
    }

    [Fact]
    public void ImplicitConversion_FromString_ShouldReturnNumeroConta()
    {
        NumeroConta numeroConta = "654321";

        Assert.Equal("654321", numeroConta.Valor);
    }

    [Fact]
    public void ImplicitConversion_ToString_ShouldReturnUnderlyingValue()
    {
        var numeroConta = new NumeroConta("654321");

        string value = numeroConta;

        Assert.Equal("654321", value);
    }
}

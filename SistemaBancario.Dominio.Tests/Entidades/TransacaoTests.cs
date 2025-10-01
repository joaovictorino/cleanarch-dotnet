using System.Text.RegularExpressions;
using SistemaBancario.Dominio.Entidades;

namespace SistemaBancario.Dominio.Tests.Entidades;

public class TransacaoTests
{
    [Fact]
    public void Constructor_DevePreencherCamposComDadosDasContas()
    {
        var contaOrigem = new Conta("123456", "Cliente Origem", 300m);
        var contaDestino = new Conta("654321", "Cliente Destino", 200m);

        var dataInicio = DateTime.Now;
        var transacao = new Transacao(contaOrigem, contaDestino, 75m);
        var dataFim = DateTime.Now;

        Assert.NotEqual(Guid.Empty, transacao.Id);
        Assert.Equal(contaOrigem.Id, transacao.IdContaOrigem);
        Assert.Equal(contaDestino.Id, transacao.IdContaDestino);
        Assert.Equal(75m, transacao.Valor);
        Assert.True(transacao.DataHoraTransacao >= dataInicio);
        Assert.True(transacao.DataHoraTransacao <= dataFim);
        Assert.Same(contaOrigem, transacao.ContaOrigem);
        Assert.Same(contaDestino, transacao.ContaDestino);
        Assert.Matches(new Regex("^TXN\\d{18}$"), transacao.CodigoTransacao);
    }
}

using SistemaBancario.Aplicacao.DTOs.ConsultarConta;
using SistemaBancario.Aplicacao.Interfaces;
using SistemaBancario.Dominio.Entidades;

namespace SistemaBancario.Aplicacao.Tests.Mocks;

internal class MapeamentoContaEmMemoria : IMapeamentoConta
{
    public List<ResultadoListar> ToDTO(List<Conta> contas)
    {
        return contas.Select(conta => new ResultadoListar
        {
            Numero = conta.Numero,
            NomeCliente = conta.NomeCliente,
            Saldo = conta.Saldo,
            DataCriacao = conta.DataCriacao
        }).ToList();
    }

    public ResultadoObterPorNumero? ToDTO(Conta? conta)
    {
        if (conta == null)
        {
            return null;
        }

        return new ResultadoObterPorNumero
        {
            Numero = conta.Numero,
            NomeCliente = conta.NomeCliente,
            Saldo = conta.Saldo,
            DataCriacao = conta.DataCriacao,
            QuantidadeTransacoes = conta.TransacoesOrigem.Count + conta.TransacoesDestino.Count
        };
    }
}

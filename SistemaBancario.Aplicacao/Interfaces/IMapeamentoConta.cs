using SistemaBancario.Aplicacao.DTOs.ConsultarConta;
using SistemaBancario.Dominio.Entidades;

namespace SistemaBancario.Aplicacao.Interfaces
{
    public interface IMapeamentoConta
    {
        List<ResultadoListar> ToDTO(List<Conta> contas);
        ResultadoObterPorNumero? ToDTO(Conta? contas);
    }
}

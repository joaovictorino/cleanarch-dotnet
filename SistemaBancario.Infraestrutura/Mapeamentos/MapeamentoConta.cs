using AutoMapper;
using SistemaBancario.Aplicacao.DTOs.ConsultarConta;
using SistemaBancario.Aplicacao.Interfaces;
using SistemaBancario.Dominio.Entidades;

namespace SistemaBancario.Infraestrutura.Mapeamentos
{
    public class MapeamentoConta : IMapeamentoConta
    {
        private readonly IMapper _mapper;

        public MapeamentoConta(IMapper mapper)
        {
            _mapper = mapper;
        }

        public List<ResultadoListar> ToDTO(List<Conta> contas)
        {
            return _mapper.Map<List<ResultadoListar>>(contas);
        }

        public ResultadoObterPorNumero? ToDTO(Conta? conta)
        {
            return _mapper.Map<ResultadoObterPorNumero?>(conta);
        }
    }
}
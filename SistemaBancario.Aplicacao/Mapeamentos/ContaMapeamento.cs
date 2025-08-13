using AutoMapper;
using SistemaBancario.Aplicacao.DTOs.ConsultarConta;
using SistemaBancario.Dominio.Entidades;

namespace SistemaBancario.Aplicacao.Mapeamentos
{
    public class ContaMapeamento : Profile
    {
        public ContaMapeamento()
        {
            CreateMap<Conta, ResultadoListar>()
                .ForMember(dest => dest.Numero, opt => opt.MapFrom(src => src.Numero))
                .ForMember(dest => dest.NomeCliente, opt => opt.MapFrom(src => src.NomeCliente))
                .ForMember(dest => dest.Saldo, opt => opt.MapFrom(src => src.Saldo))
                .ForMember(dest => dest.DataCriacao, opt => opt.MapFrom(src => src.DataCriacao));

            CreateMap<Conta, ResultadoObterPorNumero>()
                .ForMember(dest => dest.Numero, opt => opt.MapFrom(src => src.Numero))
                .ForMember(dest => dest.NomeCliente, opt => opt.MapFrom(src => src.NomeCliente))
                .ForMember(dest => dest.Saldo, opt => opt.MapFrom(src => src.Saldo))
                .ForMember(dest => dest.DataCriacao, opt => opt.MapFrom(src => src.DataCriacao))
                .ForMember(dest => dest.QuantidadeTransacoes, opt => opt.MapFrom(src => src.TransacoesOrigem.Count + src.TransacoesDestino.Count));
        }
    }
}
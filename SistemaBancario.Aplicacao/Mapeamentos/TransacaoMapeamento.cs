using AutoMapper;
using SistemaBancario.Aplicacao.DTOs.Transferencia;
using SistemaBancario.Dominio.Entidades;

namespace SistemaBancario.Aplicacao.Mapeamentos
{
    public class TransacaoMapeamento : Profile
    {
        public TransacaoMapeamento()
        {
            CreateMap<Transacao, ResultadoTransferenciaDTO>()
                .ForMember(dest => dest.CodigoTransacao, opt => opt.MapFrom(src => src.CodigoTransacao))
                .ForMember(dest => dest.DataHoraTransacao, opt => opt.MapFrom(src => src.DataHoraTransacao));
        }
    }
}
using AutoMapper;
using SistemaBancario.Aplicacao.DTOs.Transferencia;
using SistemaBancario.Dominio.Entidades;

namespace SistemaBancario.Infraestrutura.Mapeamentos
{
    public class PerfilTransacao : Profile
    {
        public PerfilTransacao()
        {
            CreateMap<Transacao, ResultadoTransferenciaDTO>()
                .ForMember(dest => dest.CodigoTransacao, opt => opt.MapFrom(src => src.CodigoTransacao))
                .ForMember(dest => dest.DataHoraTransacao, opt => opt.MapFrom(src => src.DataHoraTransacao));
        }
    }
}
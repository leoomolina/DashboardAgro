using DashboardAgro.Application.DTOs;
using DashboardAgro.Domain.Enums;
using MediatR;

namespace DashboardAgro.Application.Queries
{
    public class GetResumoAnualByEstadoAsyncQuery(int ano, int idRegiao, TipoLavoura tipoLavoura) : IRequest<IEnumerable<ResumoAnoDTO>>
    {
        public int Ano { get; set; } = ano;
        public int IdRegiao { get; set; } = idRegiao;
        public TipoLavoura TipoLavoura { get; set; } = tipoLavoura;
    }
}

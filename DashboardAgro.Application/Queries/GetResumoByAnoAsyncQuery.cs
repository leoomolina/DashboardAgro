using DashboardAgro.Application.DTOs;
using DashboardAgro.Domain.Enums;
using MediatR;

namespace DashboardAgro.Application.Queries
{
    public class GetResumoByAnoAsyncQuery(int ano, int idRegiao, int idUf, TipoLavoura? tipoLavoura) : IRequest<ResumoAnoDTO>
    {
        public int Ano { get; set; } = ano;
        public int IdRegiao { get; set; } = idRegiao;
        public int IdUf { get; set; } = idUf;
        public TipoLavoura? TipoLavoura { get; set; } = tipoLavoura;
    }
}

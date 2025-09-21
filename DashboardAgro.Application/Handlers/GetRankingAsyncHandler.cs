using DashboardAgro.Application.Contracts;
using DashboardAgro.Application.DTOs;
using DashboardAgro.Application.Queries;
using MediatR;

namespace DashboardAgro.Application.Handlers
{
    public class GetRankingAsyncHandler : IRequestHandler<GetRankingAsyncQuery, IEnumerable<RankingDashboardDTO>>
    {
        private readonly IDashboardRepository _repository;
        public GetRankingAsyncHandler(IDashboardRepository repository) => _repository = repository;

        public async Task<IEnumerable<RankingDashboardDTO>> Handle(GetRankingAsyncQuery request, CancellationToken cancellationToken)
        {
            var ranking = await _repository.GetRankingsDashboardAsync
                (request.Ano, request.TipoRanking, request.IdRegiao, request.IdUf, request.IdProducao, request.TipoLavoura);
            return [.. ranking.Select(r => new RankingDashboardDTO
                {
                    Id = r.Id,
                    Descricao = r.Descricao,
                    Valor = r.Valor,
                    TipoRanking = r.TipoRanking,
                    Tag = r.Tag,
                })];
        }
    }
}

using DashboardAgro.Application.Contracts;
using DashboardAgro.Application.DTOs;
using DashboardAgro.Application.Queries;
using MediatR;

namespace DashboardAgro.Application.Handlers
{
    public class GetAnalisePorRegiaoAsyncHandler : IRequestHandler<GetAnalisePorRegiaoAsyncQuery, IEnumerable<ResumoRegiaoDTO>>
    {
        private readonly IDashboardRepository _dashboardRepository;
        public GetAnalisePorRegiaoAsyncHandler(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<IEnumerable<ResumoRegiaoDTO>> Handle(GetAnalisePorRegiaoAsyncQuery request, CancellationToken cancellationToken)
        {
            var resumos = await _dashboardRepository.GetResumoRegioesAsync(request.Ano, request.IdRegiao, request.IdUf, request.IdProducao, request.TipoLavoura);
                return [.. resumos.Select(r => new ResumoRegiaoDTO
                {
                    Id = r.Id,
                    Descricao = r.Descricao,
                    AreaColhida = r.AreaColhida,
                    QuantidadeProduzida = r.QuantidadeProduzida,
                    ValorProducao = r.ValorProducao,
                    PrincipaisProducoes = r.PrincipaisProducoes
                })];
        }
    }
}

using DashboardAgro.Application.Contracts;
using DashboardAgro.Application.DTOs;
using DashboardAgro.Application.Queries;
using DashboardAgro.Domain.Enums;
using MediatR;

namespace DashboardAgro.Application.Handlers
{
    public class GetResumoAnualByEstadoAsyncHandler : IRequestHandler<GetResumoAnualByEstadoAsyncQuery, IEnumerable<ResumoAnoDTO>>
    {
        private readonly ILavouraPermanenteRepository _repositoryLavouraPermanente;
        private readonly ILavouraTemporariaRepository _repositoryLavouraTemporaria;

        public GetResumoAnualByEstadoAsyncHandler(ILavouraPermanenteRepository repositoryLavouraPermanente, ILavouraTemporariaRepository repositoryLavouraTemporaria)
        {
            _repositoryLavouraPermanente = repositoryLavouraPermanente;
            _repositoryLavouraTemporaria = repositoryLavouraTemporaria;
        }

        public async Task<IEnumerable<ResumoAnoDTO>> Handle(GetResumoAnualByEstadoAsyncQuery request, CancellationToken cancellationToken)
        {
            var lavourasPermanente = request.TipoLavoura != TipoLavoura.Temporaria
                ? await _repositoryLavouraPermanente.GetResumoAnualLavouraPermanenteAsync(request.Ano, request.IdRegiao, 0, request.IdProducao)
                : [];

            var lavourasTemporaria = request.TipoLavoura != TipoLavoura.Permanente
                ? await _repositoryLavouraTemporaria.GetResumoAnualLavouraTemporariaAsync(request.Ano, request.IdRegiao, 0, request.IdProducao)
                : [];

            var lavouras = lavourasPermanente.Concat(lavourasTemporaria);

            return lavouras
                .GroupBy(g => new { g.Ano, g.SiglaUf, g.DescricaoRegiao })
                .Select(r => new ResumoAnoDTO
                {
                    Ano = r.Key.Ano,
                    SiglaUf = r.Key.SiglaUf,
                    DescricaoRegiao = r.Key.DescricaoRegiao,
                    AreaPlantadaTotal = r.Sum(s => s.AreaPlantadaXDestinadaColheita),
                    AreaColhidaTotal = r.Sum(s => s.AreaColhida),
                    QuantidadeProduzidaTotal = r.Sum(s => s.QuantidadeProduzida),
                    ValorProducaoTotal = r.Sum(s => s.ValorProducao),

                    Lavouras = [.. r
                        .GroupBy(l => l.TipoLavoura)
                        .Select(l => new LavouraDTO
                        {
                            Descricao = l.Key == TipoLavoura.Permanente ? "Lavoura Permanente" : "Lavoura Temporária",
                            TipoLavoura = l.Key,
                            AreaColhida = l.Sum(x => x.AreaColhida),
                            QuantidadeProduzida = l.Sum(x => x.QuantidadeProduzida),
                            ValorProducao = l.Sum(x => x.ValorProducao),
                            AreaPlantadaXDestinadaColheita = l.Sum(x => x.AreaPlantadaXDestinadaColheita),
                        })]
                })
                .OrderByDescending(o => o.QuantidadeProduzidaTotal)
                .ToList();
        }

    }
}

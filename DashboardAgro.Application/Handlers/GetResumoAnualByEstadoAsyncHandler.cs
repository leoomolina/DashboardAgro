using DashboardAgro.Application.Contracts;
using DashboardAgro.Application.DTOs;
using DashboardAgro.Application.Queries;
using DashboardAgro.Domain.Entities;
using DashboardAgro.Domain.Enums;
using MediatR;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                ? await _repositoryLavouraPermanente.GetResumoAnualLavouraPermanenteAsync(request.Ano, request.IdRegiao, 0)
                : [];

            var lavourasTemporaria = request.TipoLavoura != TipoLavoura.Permanente
                ? await _repositoryLavouraTemporaria.GetResumoAnualLavouraTemporariaAsync(request.Ano, request.IdRegiao, 0)
                : [];

            List<LavouraDTO> lavourasList = [.. lavourasPermanente
                .Concat(lavourasTemporaria)
                .GroupBy(g => new { g.Ano, g.TipoLavoura })
                .Select(l => new LavouraDTO
                {
                   Descricao = l.Key.TipoLavoura == TipoLavoura.Permanente ? "Lavoura Permanente" : "Lavoura Temporária",
                    AreaColhida = l.Sum(r => r.AreaColhida),
                    QuantidadeProduzida = l.Sum(r => r.QuantidadeProduzida),
                    ValorProducao = l.Sum(r => r.ValorProducao),
                    AreaPlantadaXDestinadaColheita = l.Sum(r => r.AreaPlantadaXDestinadaColheita),
                })];

            return [.. lavourasPermanente
                .Concat(lavourasTemporaria)
                .GroupBy(g => new { g.Ano, g.SiglaUf, g.DescricaoRegiao })
                .Select(r => new ResumoAnoDTO
                {
                    Ano = r.Key.Ano,
                    SiglaUf = r.Key.SiglaUf,
                    DescricaoRegiao = r.Key.DescricaoRegiao,
                    AreaColhidaTotal = r.Sum(s => s.AreaColhida),
                    QuantidadeProduzidaTotal = r.Sum(s => s.QuantidadeProduzida),
                    ValorProducaoTotal = r.Sum(s => s.ValorProducao),
                    Lavouras = lavourasList
                })
                .OrderByDescending(o => o.QuantidadeProduzidaTotal)];

        }
    }
}

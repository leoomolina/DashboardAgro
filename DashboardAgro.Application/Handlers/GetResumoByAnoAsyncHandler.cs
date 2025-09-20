using DashboardAgro.Application.Contracts;
using DashboardAgro.Application.DTOs;
using DashboardAgro.Application.Queries;
using DashboardAgro.Domain.Entities;
using DashboardAgro.Domain.Enums;
using MediatR;

namespace DashboardAgro.Application.Handlers
{
    public class GetResumoByAnoAsyncHandler : IRequestHandler<GetResumoByAnoAsyncQuery, ResumoAnoDTO>
    {
        private readonly ILavouraPermanenteRepository _repositoryLavouraPermanente;
        private readonly ILavouraTemporariaRepository _repositoryLavouraTemporaria;

        public GetResumoByAnoAsyncHandler(ILavouraPermanenteRepository repositoryLavouraPermanente, ILavouraTemporariaRepository repositoryLavouraTemporaria)
        {
            _repositoryLavouraPermanente = repositoryLavouraPermanente;
            _repositoryLavouraTemporaria = repositoryLavouraTemporaria;
        }

        public async Task<ResumoAnoDTO> Handle(GetResumoByAnoAsyncQuery request, CancellationToken cancellationToken)
        {
            var lavourasPermanente = request.TipoLavoura != TipoLavoura.Temporaria
                ? await _repositoryLavouraPermanente.GetResumoAnualLavouraPermanenteAsync(request.Ano, request.IdRegiao, request.IdUf)
                : [];

            var lavourasTemporaria = request.TipoLavoura != TipoLavoura.Permanente
                ? await _repositoryLavouraTemporaria.GetResumoAnualLavouraTemporariaAsync(request.Ano, request.IdRegiao, request.IdUf)
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

            return new ResumoAnoDTO
            {
                Ano = request.Ano,
                AreaColhidaTotal = lavourasPermanente.Sum(s => s.AreaColhida) + lavourasTemporaria.Sum(s => s.AreaColhida),
                QuantidadeProduzidaTotal = lavourasPermanente.Sum(s => s.QuantidadeProduzida) + lavourasTemporaria.Sum(s => s.QuantidadeProduzida),
                ValorProducaoTotal = lavourasPermanente.Sum(s => s.ValorProducao) + lavourasTemporaria.Sum(s => s.ValorProducao),
                Lavouras = lavourasList
            };
        }
    }
}

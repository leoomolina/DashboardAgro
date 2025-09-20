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
            IEnumerable<ResumoDashboard> resumo;

            switch (request.TipoLavoura)
            {
                case TipoLavoura.Permanente:
                    resumo = await _repositoryLavouraPermanente
                        .GetResumoAnualLavouraPermanenteAsync(request.Ano, request.IdRegiao, 0);
                    break;
                case TipoLavoura.Temporaria:
                    resumo = await _repositoryLavouraTemporaria
                        .GetResumoAnualLavouraTemporariaAsync(request.Ano, request.IdRegiao, 0);
                    break;
                default:
                    var lavourasPermanente = await _repositoryLavouraPermanente
                        .GetResumoAnualLavouraPermanenteAsync(request.Ano, request.IdRegiao, 0);
                    var lavourasTemporaria = await _repositoryLavouraTemporaria
                        .GetResumoAnualLavouraTemporariaAsync(request.Ano, request.IdRegiao, 0);

                    resumo = lavourasPermanente.Concat(lavourasTemporaria);
                    break;
            }

            return [.. resumo
                .GroupBy(l => new { l.Ano, l.SiglaUf, l.DescricaoRegiao })
                .Select(g => new ResumoAnoDTO
                {
                    Ano = g.Key.Ano,
                    SiglaUf = g.Key.SiglaUf,
                    DescricaoRegiao = g.Key.DescricaoRegiao,
                    AreaColhida = g.Sum(x => x.AreaColhida),
                    QuantidadeProduzida = g.Sum(x => x.QuantidadeProduzida),
                    ValorProducao = g.Sum(x => x.ValorProducao)
                }).OrderByDescending(r => r.QuantidadeProduzida)];
        }
    }
}

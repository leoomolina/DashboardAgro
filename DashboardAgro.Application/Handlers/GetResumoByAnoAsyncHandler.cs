using DashboardAgro.Application.Contracts;
using DashboardAgro.Application.DTOs;
using DashboardAgro.Application.Queries;
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
            if (request.TipoLavoura == TipoLavoura.Permanente)
            {
                var lavourasPermanente = await _repositoryLavouraPermanente
                    .GetResumoAnualLavouraPermanenteAsync(request.Ano, request.IdRegiao, request.IdUf);
                return lavourasPermanente
                    .GroupBy(g => g.Ano)
                    .Select(r => new ResumoAnoDTO
                    {
                        Ano = r.Key,
                        AreaColhida = r.Sum(s => s.AreaColhida),
                        QuantidadeProduzida = r.Sum(s => s.QuantidadeProduzida),
                        ValorProducao = r.Sum(s => s.ValorProducao)
                    }).First();
            }
            else if (request.TipoLavoura == TipoLavoura.Temporaria)
            {
                var lavourasTemporaria = await _repositoryLavouraTemporaria.GetResumoAnualLavouraTemporariaAsync(request.Ano, request.IdRegiao, request.IdUf);
                return lavourasTemporaria
                    .GroupBy(g => g.Ano)
                    .Select(r => new ResumoAnoDTO
                    {
                        Ano = r.Key,
                        AreaColhida = r.Sum(s => s.AreaColhida),
                        QuantidadeProduzida = r.Sum(s => s.QuantidadeProduzida),
                        ValorProducao = r.Sum(s => s.ValorProducao)
                    }).First();
            }
            else
            {
                var lavourasPermanente = await _repositoryLavouraPermanente.GetResumoAnualLavouraPermanenteAsync(request.Ano, request.IdRegiao, request.IdUf);
                var lavourasTemporaria = await _repositoryLavouraTemporaria.GetResumoAnualLavouraTemporariaAsync(request.Ano, request.IdRegiao, request.IdUf);

                return new ResumoAnoDTO
                {
                    Ano = request.Ano,
                    AreaColhida = lavourasPermanente.Sum(s => s.AreaColhida) + lavourasTemporaria.Sum(s => s.AreaColhida),
                    QuantidadeProduzida = lavourasPermanente.Sum(s => s.QuantidadeProduzida) + lavourasTemporaria.Sum(s => s.QuantidadeProduzida),
                    ValorProducao = lavourasPermanente.Sum(s => s.ValorProducao) + lavourasTemporaria.Sum(s => s.ValorProducao)
                };
            }
        }
    }
}

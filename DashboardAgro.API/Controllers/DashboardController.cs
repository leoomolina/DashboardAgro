using DashboardAgro.Application.Queries;
using DashboardAgro.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DashboardAgro.API.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IMediator _mediator;

        public DashboardController(ILogger<DashboardController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("resumo-anual")]
        public async Task<IActionResult> GetResumoAnual(int ano, int idRegiao, int idUf, TipoLavoura? tipoLavoura, int idProducao)
        {
            if (ano == 0)
                return BadRequest("Ano não pode ser nulo.");

            var result = await _mediator.Send(new GetResumoByAnoAsyncQuery(ano, idRegiao, idUf, tipoLavoura, idProducao));
            return Ok(result);
        }

        [HttpGet("resumo-anual-por-estado")]
        public async Task<IActionResult> GetResumoAnualPorEstado(int ano, int idRegiao, TipoLavoura? tipoLavoura, int idProducao)
        {
            if (ano == 0)
                return BadRequest("Ano não pode ser nulo.");

            var result = await _mediator.Send(new GetResumoAnualByEstadoAsyncQuery(ano, idRegiao, tipoLavoura, idProducao));
            return Ok(result);
        }

        [HttpGet("listar-lavouras-permanentes")]
        public async Task<IActionResult> GetLavourasPerm()
        {
            var result = await _mediator.Send(new ListarLavourasPermanentesQuery());

            if (result == null)
                return NotFound("Nenhum dado encontrado para o ano informado.");

            return Ok(result);
        }

        [HttpGet("listar-lavouras-temporarias")]
        public async Task<IActionResult> GetLavourasTemp()
        {
            var result = await _mediator.Send(new ListarLavourasTemporariasQuery());

            if (result == null)
                return NotFound("Nenhum dado encontrado para o ano informado.");

            return Ok(result);
        }

        [HttpGet("analise-por-regiao")]
        public async Task<IActionResult> AnalisePorRegiao(int ano, int idRegiao, int idUf, int idProducao, TipoLavoura? tipoLavoura)
        {
            var result = await _mediator.Send(new GetAnalisePorRegiaoAsyncQuery(ano, idRegiao, idUf, idProducao, tipoLavoura));

            if (result == null)
                return NotFound("Nenhum dado encontrado para o ano informado.");

            return Ok(result);
        }

        [HttpGet("ranking-quantidade-produzida")]
        public async Task<IActionResult> ListarRankingPorTipo(int ano, Agrupamento tipoRanking, int idRegiao, int idUf, int idProducao, TipoLavoura? tipoLavoura)
        {
            var result = await _mediator.Send(new GetRankingAsyncQuery(ano, tipoRanking, idRegiao, idUf, tipoLavoura, idProducao));

            if (result == null)
                return NotFound("Nenhum dado encontrado para o ano informado.");

            return Ok(result);
        }
    }
}

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
        public async Task<IActionResult> GetResumoAnual(int ano, int idRegiao, int idUf, TipoLavoura? tipoLavoura)
        {
            if (ano == 0)
                return BadRequest("Ano não pode ser nulo.");

            var result = await _mediator.Send(new GetResumoByAnoAsyncQuery(ano, idRegiao, idUf, tipoLavoura));
            return Ok(result);
        }

        [HttpGet("resumo-anual-por-estado")]
        public async Task<IActionResult> GetResumoAnualPorEstado(int ano, int idRegiao, TipoLavoura? tipoLavoura)
        {
            if (ano == 0)
                return BadRequest("Ano não pode ser nulo.");

            var result = await _mediator.Send(new GetResumoAnualByEstadoAsyncQuery(ano, idRegiao, tipoLavoura));
            return Ok(result);
        }


        //[HttpGet("resumo-ano-producao")]
        //public async Task<IActionResult> GetResumoAnoByProducao(ResumoAnoByProducaoDTO.ResumoAnoByProducaoFilter filter)
        //{
        //    if (filter == null)
        //        return BadRequest("O filtro não pode ser nulo.");

        //    if (filter.Ano <= 0)
        //        return BadRequest("O campo 'Ano' é obrigatório e deve ser maior que zero.");

        //    if (filter.IdProducao <= 0)
        //        return BadRequest("O campo 'IdProducao' é obrigatório e deve ser maior que zero.");

        //    var result = await _mediator.Send(new GetResumoAnoLavouraPermanenteByProducaoQuery(filter));

        //    if (result == null || result.Count == 0)
        //        return NotFound("Nenhum dado encontrado para o ano informado.");

        //    return Ok(result);
        //}

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
    }
}

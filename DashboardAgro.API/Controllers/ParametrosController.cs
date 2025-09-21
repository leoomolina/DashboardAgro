using DashboardAgro.Application.Queries;
using DashboardAgro.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DashboardAgro.API.Controllers
{
    [Route("api/parametros")]
    [ApiController]
    public class ParametrosController : ControllerBase
    {
        private readonly ILogger<ParametrosController> _logger;
        private readonly IMediator _mediator;

        public ParametrosController(ILogger<ParametrosController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("regioes-brasil")]
        public async Task<IActionResult> GetRegiaoBrasil() => Ok(await _mediator.Send(new ListarRegioesQuery()));

        [HttpGet("unidades-federativas")]
        public async Task<IActionResult> GetUFs(int? idRegiao) => Ok(await _mediator.Send(new ListarUFsAsyncQuery(idRegiao)));

        [HttpGet("tipo-lavoura")]
        public async Task<IActionResult> GetTipoLavoura()
            => Ok(Enum.GetValues<TipoLavoura>()
                .Cast<TipoLavoura>()
                .Select(t => new { Id = (int)t, Descricao = t.ToString() })
                .ToList());

        [HttpGet("listar-producoes")]
        public async Task<IActionResult> GetProducao()
            => Ok(await _mediator.Send(new ListarProducoesAsyncQuery()));
    }
}

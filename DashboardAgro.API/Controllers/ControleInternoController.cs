using DashboardAgro.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace api_dashboard_agro.Controllers
{
    [Route("api/controle-interno")]
    [ApiController]
    public class ControleInternoController : ControllerBase
    {
        private readonly ILogger<ControleInternoController> _logger;
        private readonly IMediator _mediator;

        public ControleInternoController(ILogger<ControleInternoController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("anos-disponiveis")]
        public async Task<IActionResult> GetByAnosDisponiveis() 
            => Ok(await _mediator.Send(new ListarAnosDisponiveisAsyncQuery()));
    }
}

using DashboardAgro.Domain.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace api_dashboard_agro.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LavouraController : ControllerBase
    {

        private readonly ILogger<LavouraController> _logger;
        private readonly ILavouraRepository _repo;

        public LavouraController(ILogger<LavouraController> logger, ILavouraRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet("anos-disponiveis")]
        public async Task<IActionResult> GetByAnosDisponiveis() => Ok(await _repo.GetAnosDisponiveisAsync());
    }
}

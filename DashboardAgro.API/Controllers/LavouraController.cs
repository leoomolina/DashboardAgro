using DashboardAgro.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api_dashboard_agro.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LavouraController : ControllerBase
    {

        private readonly ILogger<LavouraController> _logger;
        private readonly RankingMunicipiosPorCulturaHandler _repo;

        public LavouraController(ILogger<LavouraController> logger, RankingMunicipiosPorCulturaHandler repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet("{ano}")]
        public async Task<IActionResult> GetByAno(int ano)
        {
            return Ok();
        }
    }
}

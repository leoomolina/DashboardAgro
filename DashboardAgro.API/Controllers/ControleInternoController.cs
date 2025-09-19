using DashboardAgro.Application.Contracts;
using DashboardAgro.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace api_dashboard_agro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControleInternoController : ControllerBase
    {
        private readonly ILogger<ControleInternoController> _logger;
        private readonly IImportarDados _repo;

        public ControleInternoController(ILogger<ControleInternoController> logger, IImportarDados repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet("anos-disponiveis")]
        public async Task<IActionResult> GetByAnosDisponiveis() => Ok(await _repo.GetAnosDisponiveisAsync());

        [HttpGet("importacoes")]
        public async Task<IActionResult> GetImportacoes(StatusImportacaoDados? status) => Ok(await _repo.GetImportacao(status));
    }
}

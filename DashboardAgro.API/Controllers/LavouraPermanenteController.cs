using DashboardAgro.Application.Contracts;
using DashboardAgro.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace api_dashboard_agro.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LavouraPermanenteController : ControllerBase
    {

        private readonly ILogger<LavouraPermanenteController> _logger;
        private readonly ILavouraPermanenteRepository _repo;

        public LavouraPermanenteController(ILogger<LavouraPermanenteController> logger, ILavouraPermanenteRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet("resumo-ano")]
        public async Task<IActionResult> GetResumoAno(int ano)
        {
            var result = await _repo.GetResumoAnualLavouraPermanenteAsync(ano);

            if (result == null || result.Count == 0)
                return NotFound("Nenhum dado encontrado para o ano informado.");

            return Ok(result);
        }

        [HttpGet("resumo-ano-producao")]
        public async Task<IActionResult> GetResumoAnoByProducao(ResumoAnoByLavouraDTO.ResumoAnoByLavouraFilter filter)
        {
            if (filter == null)
                return BadRequest("O filtro não pode ser nulo.");

            if (filter.Ano <= 0)
                return BadRequest("O campo 'Ano' é obrigatório e deve ser maior que zero.");

            if (filter.IdProducao <= 0)
                return BadRequest("O campo 'IdProducao' é obrigatório e deve ser maior que zero.");

            var result = await _repo.GetResumoAnualByLavouraAsync(filter);

            if (result == null || result.Count == 0)
                return NotFound("Nenhum dado encontrado para o ano informado.");

            return Ok(result);
        }

        [HttpGet("lavouras")]
        public async Task<IActionResult> GetLavouras(ResumoAnoByLavouraDTO.ResumoAnoByLavouraFilter filter)
        {
            var result = await _repo.GetAllAsync();

            if (result == null)
                return NotFound("Nenhum dado encontrado para o ano informado.");

            return Ok(result);
        }
    }
}

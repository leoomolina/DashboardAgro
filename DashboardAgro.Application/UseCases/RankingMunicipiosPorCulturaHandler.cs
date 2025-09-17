using DashboardAgro.Domain.Contracts;

namespace DashboardAgro.Application.Interfaces
{
    public class RankingMunicipiosPorCulturaHandler
    {
        private readonly ILavouraRepository _repo;

        public RankingMunicipiosPorCulturaHandler(ILavouraRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<(string Municipio, decimal Producao)>> Handle(int ano, int idCultura, int top)
        {
            return await _repo.GetTopMunicipiosAsync(ano, idCultura, top);
        }

    }
}

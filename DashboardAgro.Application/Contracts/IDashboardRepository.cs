using DashboardAgro.Domain.Entities;
using DashboardAgro.Domain.Enums;

namespace DashboardAgro.Application.Contracts
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<ResumoRegiao>> GetResumoRegioesAsync(int ano, int idRegiao, int idUf, int idProducao, TipoLavoura? tipoLavoura);
        Task<IEnumerable<RankingDashboard>> GetRankingsDashboardAsync (int ano, Agrupamento tipoRanking, int idRegiao, int idUf, int idProducao, TipoLavoura? tipoLavoura);
    }
}

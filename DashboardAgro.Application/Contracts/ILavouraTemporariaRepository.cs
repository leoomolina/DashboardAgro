using DashboardAgro.Domain.Entities;

namespace DashboardAgro.Application.Contracts
{
    public interface ILavouraTemporariaRepository
    {
        Task<List<ResumoLavouraAno>> GetResumoAnualLavouraTemporariaAsync(int ano, int idRegiao, int idUf);
        Task<List<ResumoLavouraAno>> GetResumoAnualByLavouraAsync(int ano, int idRegiao, int idUf, int idProducao);
        Task<IEnumerable<Lavoura>> GetAllAsync();
    }
}

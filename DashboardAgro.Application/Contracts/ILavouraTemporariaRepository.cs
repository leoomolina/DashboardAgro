using DashboardAgro.Application.DTOs;
using DashboardAgro.Domain.Entities;

namespace DashboardAgro.Application.Contracts
{
    public interface ILavouraTemporariaRepository
    {
        Task<List<ResumoDashboard>> GetResumoAnualLavouraTemporariaAsync(int ano, int idRegiao, int idUf);
        Task<List<ResumoDashboard>> GetResumoAnualByLavouraAsync(int ano, int idRegiao, int idUf, int idProducao);
        Task<IEnumerable<Lavoura>> GetAllAsync();
    }
}

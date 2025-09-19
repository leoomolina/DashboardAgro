using DashboardAgro.Application.DTOs;
using DashboardAgro.Domain.Entities;

namespace DashboardAgro.Application.Contracts
{
    public interface ILavouraPermanenteRepository
    {
        Task<IEnumerable<Lavoura>> GetByAnoAsync(int ano);
        Task<IEnumerable<Lavoura>> GetByCulturaAsync(int ano, int idProducao);
        Task<decimal> GetProdutividadeMediaAsync(int ano, int idProducao);
        Task<IEnumerable<(UnidadeFederativa UnidadeFederativa, decimal Producao)>> GetTopUFsAsync(int ano, int idProducao, int top = 10);
        Task<List<ResumoAnoDTO>> GetResumoAnualLavouraPermanenteAsync(int ano);
        Task<List<ResumoAnoByLavouraDTO>> GetResumoAnualByLavouraAsync(ResumoAnoByLavouraDTO.ResumoAnoByLavouraFilter filter);
        Task<IEnumerable<Lavoura>> GetAllAsync();
    }
}

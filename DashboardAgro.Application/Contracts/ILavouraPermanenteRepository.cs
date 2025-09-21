using DashboardAgro.Domain.Entities;

namespace DashboardAgro.Application.Contracts
{
    public interface ILavouraPermanenteRepository
    {
        Task<IEnumerable<Lavoura>> GetByAnoAsync(int ano);
        Task<IEnumerable<Lavoura>> GetByCulturaAsync(int ano, int idProducao);
        Task<decimal> GetProdutividadeMediaAsync(int ano, int idProducao);
        Task<IEnumerable<(UnidadeFederativa UnidadeFederativa, decimal Producao)>> GetTopUFsAsync(int ano, int idProducao, int top = 10);
        Task<IEnumerable<ResumoLavouraAno>> GetResumoAnualLavouraPermanenteAsync(int ano, int idRegiao, int idUf, int idProducao);
        Task<IEnumerable<ResumoLavouraAno>> GetResumoAnualByLavouraAsync(int ano, int idRegiao, int idUf, int idProducao);
        Task<IEnumerable<Lavoura>> GetAllAsync();
    }
}

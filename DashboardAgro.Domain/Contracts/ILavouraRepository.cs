using DashboardAgro.Domain.Entities;

namespace DashboardAgro.Domain.Contracts
{
    public interface ILavouraRepository
    {
        Task<IEnumerable<int>> GetAnosDisponiveisAsync();
        Task<IEnumerable<Lavoura>> GetByAnoAsync(int ano);
        Task<IEnumerable<Lavoura>> GetByCulturaAsync(int ano, int idProducao);
        Task<decimal> GetProdutividadeMediaAsync(int ano, int idProducao);
        Task<IEnumerable<(UnidadeFederativa UnidadeFederativa, decimal Producao)>> GetTopUFsAsync(int ano, int idProducao, int top = 10);
    }
}

using DashboardAgro.Domain.Entities;

namespace DashboardAgro.Domain.Contracts
{
    public interface ILavouraRepository
    {
        Task<IEnumerable<Lavoura>> GetByAnoAsync(int ano);
        Task<IEnumerable<Lavoura>> GetByCulturaAsync(int ano, int idCultura);
        Task<decimal> GetProdutividadeMediaAsync(int ano, int idCultura);
        Task<IEnumerable<(string Municipio, decimal Producao)>> GetTopMunicipiosAsync(int ano, int idCultura, int top = 10);
    }
}

using DashboardAgro.Domain.Entities;

namespace DashboardAgro.Application.Contracts
{
    public interface IParametrosRepository
    {
        Task<IEnumerable<Regiao>> GetAllRegiao();
        Task<IEnumerable<UnidadeFederativa>> GetAllUnidadesFederativas(int? idRegiao);
        Task<IEnumerable<Producao>> GetAllProducoes();
    }
}

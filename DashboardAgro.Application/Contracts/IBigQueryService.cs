using DashboardAgro.Domain.Entities;

namespace DashboardAgro.Application.Interfaces
{
    public interface IBigQueryService
    {
        IEnumerable<UnidadeFederativa> ObterUnidadesFederativas(int ano);
        IEnumerable<Lavoura> ObterLavouraPermanente(int ano);
        List<int> ObterAnosDisponiveisAsync();
    }
}

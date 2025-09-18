using DashboardAgro.Domain.Entities;
using DashboardAgro.Domain.Enums;
using DashboardAgro.Domain.ValueObjects;

namespace DashboardAgro.Application.Interfaces
{
    public interface IBigQueryService
    {
        IEnumerable<UnidadeFederativa> ObterUnidadesFederativas(int ano);
        Dictionary<LavouraKey, Lavoura> ObterLavoura(int ano, TipoLavoura tipoLavoura);
        List<int> ObterAnosDisponiveisAsync();
    }
}

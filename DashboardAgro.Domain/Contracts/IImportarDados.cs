using DashboardAgro.Domain.Entities;

namespace DashboardAgro.Domain.Contracts
{
    public interface IImportarDados
    {
        Task ImportarAnosAsync(Dictionary<int, ControleImportacao> anos);
    }
}

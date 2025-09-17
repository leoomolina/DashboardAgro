namespace DashboardAgro.Application.Interfaces
{
    public interface IBigQueryService
    {
        IEnumerable<object> ExecutarQueryAsync(string sql);

        List<int> ObterAnosDisponiveisAsync();
    }
}

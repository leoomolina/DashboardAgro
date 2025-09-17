using DashboardAgro.Application.Interfaces;
using Google.Cloud.BigQuery.V2;

namespace DashboardAgro.Infraestructure.Services
{
    public class BigQueryService : IBigQueryService
    {
        private readonly BigQueryClient _client;

        public BigQueryService(string projectId)
        {
            _client = BigQueryClient.Create(projectId);
        }

        public IEnumerable<object> ExecutarQueryAsync(string sql)
        {
            var results = _client.ExecuteQuery(sql, parameters: null);
            return results;
        }

        public List<int> ObterAnosDisponiveisAsync()
        {
            string query = @"
        SELECT DISTINCT ano
        FROM `basedosdados.br_ibge_pam.lavoura_permanente`
        ORDER BY ano DESC";

            var results = _client.ExecuteQuery(query, parameters: null);

            var listaAnos = new List<int>();
            foreach (BigQueryRow row in results)
            {
                listaAnos.Add(Convert.ToInt32(row["ano"]));
            }

            return listaAnos;
        }
    }
}

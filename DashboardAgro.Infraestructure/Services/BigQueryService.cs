using DashboardAgro.Application.Interfaces;
using DashboardAgro.Domain.Entities;
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

        public IEnumerable<Lavoura> ObterLavouraPermanente(int ano)
        {
            string sql = $@"
        SELECT
            dados.ano as ano,
            dados.id_municipio AS id_municipio,
            dados.produto as produto,
            dados.area_destinada_colheita as area_destinada_colheita,
            dados.area_colhida as area_colhida,
            dados.quantidade_produzida as quantidade_produzida,
            dados.rendimento_medio_producao as rendimento_medio_producao,
            dados.valor_producao as valor_producao
        FROM `basedosdados.br_ibge_pam.lavoura_permanente` AS dados 
        WHERE dados.ano = {ano}";

            var results = _client.ExecuteQuery(sql, parameters: null);

            foreach (var row in results)
            {
                yield return new Lavoura
                {
                    Produto = (string)row["produto"],
                    Ano = Convert.ToInt32(row["ano"]),
                    AreaColhida = row["area_colhida"] is null ? 0 : Convert.ToInt64(row["area_colhida"]),
                    AreaDestinadaColheita = row["area_destinada_colheita"] is null ? 0 : Convert.ToInt64(row["area_destinada_colheita"]),
                    QuantidadeProduzida = row["quantidade_produzida"] is null ? 0 : Convert.ToDecimal(row["quantidade_produzida"]),
                    ValorProducao = row["valor_producao"] is null ? 0 : Convert.ToDecimal(row["valor_producao"]),
                    Tipo = Domain.Enums.TipoLavoura.Permanente
                };
            }
        }

        public IEnumerable<UnidadeFederativa> ObterUnidadesFederativas(int ano)
        {
            string sql = $@"
                        SELECT DISTINCT
                            sigla_uf,
                            sigla_uf_nome,
                            regiao_uf_nome
                        FROM `basedosdados.br_ibge_pam.lavoura_permanente`
                        WHERE ano = {ano}
                        ORDER BY sigla_uf";

            var results = _client.ExecuteQuery(sql, parameters: null);

            foreach (var row in results)
            {
                yield return new UnidadeFederativa
                {
                    SiglaUF = (string)row["sigla_uf"],
                    NomeUF = (string)row["sigla_uf_nome"],
                    Regiao = new Regiao
                    {
                        Descricao = (string)row["regiao_uf_nome"]
                    }
                };
            }
        }
    }
}

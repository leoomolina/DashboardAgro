using DashboardAgro.Application.Contracts;
using DashboardAgro.Domain.Entities;
using DashboardAgro.Domain.Enums;
using DashboardAgro.Domain.ValueObjects;
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

        public Dictionary<LavouraKey, Lavoura> ObterLavoura(int ano, TipoLavoura tipoLavoura)
        {
            string tableName = tipoLavoura == TipoLavoura.Permanente ? "lavoura_permanente" : "lavoura_temporaria";
            string fieldColheita = tipoLavoura == TipoLavoura.Permanente ? "area_destinada_colheita" : "area_plantada";

            string sql = $@"
SELECT
    dados.ano as ano,
    dados.sigla_uf AS sigla_uf,
    dados.produto as produto,
    SUM(dados.{fieldColheita}) as {fieldColheita},
    SUM(dados.area_colhida) as area_colhida,
    SUM(dados.quantidade_produzida) as quantidade_produzida,
    SUM(dados.rendimento_medio_producao) as rendimento_medio_producao,
    SUM(dados.valor_producao) as valor_producao
FROM `basedosdados.br_ibge_pam.{tableName}` AS dados
WHERE dados.ano = {ano}
GROUP BY
    dados.ano,
    dados.sigla_uf,
    dados.produto
";

            var results = _client.ExecuteQuery(sql, parameters: null);

            var response = new Dictionary<LavouraKey, Lavoura>();

            foreach (var row in results)
            {
                response.Add(new LavouraKey(Ano: Convert.ToInt32(row["ano"]), Producao: (string)row["produto"], SiglaUF: (string)row["sigla_uf"]),
                    new Lavoura
                    {
                        Producao = (string)row["produto"],
                        Ano = Convert.ToInt32(row["ano"]),
                        AreaColhida = row["area_colhida"] is null ? 0 : Convert.ToInt64(row["area_colhida"]),
                        AreaDestinadaColheita = tipoLavoura == TipoLavoura.Permanente ?
                        (row[fieldColheita] is null ? 0 : Convert.ToInt64(row[fieldColheita])) : 0,
                        AreaPlantada = tipoLavoura == TipoLavoura.Temporaria ?
                        (row[fieldColheita] is null ? 0 : Convert.ToInt64(row[fieldColheita])) : 0,
                        QuantidadeProduzida = row["quantidade_produzida"] is null ? 0 : Convert.ToDecimal(row["quantidade_produzida"]),
                        ValorProducao = row["valor_producao"] is null ? 0 : Convert.ToDecimal(row["valor_producao"]),
                        Tipo = TipoLavoura.Permanente,
                        UF = new UnidadeFederativa
                        {
                            SiglaUF = (string)row["sigla_uf"]
                        }
                    });
            }

            return response;
        }

        public IEnumerable<UnidadeFederativa> ObterUnidadesFederativas(int ano)
        {
            string sql = $@"
                        SELECT DISTINCT
                            sigla AS sigla_uf,
                            nome AS sigla_uf_nome,
                            regiao AS regiao_uf_nome
                        FROM `basedosdados.br_bd_diretorios_brasil.uf`";

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

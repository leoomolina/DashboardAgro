//using DashboardAgro.Application.CaseUses;
//using DashboardAgro.Domain.Enums;
//using DashboardAgro.Infraestructure;
//using DashboardAgro.Infraestructure.Tables;
//using Google.Cloud.BigQuery.V2;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;

//class Program
//{
//    static async Task Main(string[] args)
//    {
//        var configuration = new ConfigurationBuilder()
//            .AddJsonFile("appsettings.json", optional: true)
//            .AddEnvironmentVariables()
//            .Build();

//        var projectId = configuration["BigQuery:ProjectId"];

//        var services = new ServiceCollection();
//        services.AddInfrastructureServices(configuration);
//        var provider = services.BuildServiceProvider();

//        var ufRepo = provider.GetRequiredService<IRepository<UFTable>>();
//        var controleRepo = provider.GetRequiredService<IRepository<ControleImportacaoTable>>();
//        var producaoRepo = provider.GetRequiredService<IRepository<ProducaoTable>>();
//        var lavouraTempRepo = provider.GetRequiredService<IRepository<DadosLavouraTemporariaTable>>();
//        var lavouraPermRepo = provider.GetRequiredService<IRepository<DadosLavouraPermanenteTable>>();

//        var client = BigQueryClient.Create(projectId);

//        var ufsExistentes = await ufRepo.Query().OfType<UFTable>().AsNoTracking().ToListAsync();
//        var anosExistentes = await controleRepo.Query().OfType<ControleImportacaoTable>().AsNoTracking().ToListAsync();
//        var producoesExistentes = await producaoRepo.Query().OfType<ProducaoTable>().AsNoTracking().ToListAsync();

//        var anosDict = anosExistentes.ToDictionary(m => m.Ano, m => m);
//        var producaoDict = producoesExistentes.ToDictionary(p => p.Descricao, p => p);

//        string queryAnos = @"
//SELECT DISTINCT
//    dados_permanente.ano AS ano
//FROM `basedosdados.br_ibge_pam.lavoura_permanente` AS dados_permanente
//GROUP BY dados_permanente.ano
//ORDER BY ano DESC
//";

//        var resultAnos = client.ExecuteQuery(queryAnos, parameters: null).ToList();

//        var novosControlesImportacao = new List<ControleImportacaoTable>();

//        foreach (var row in client.ExecuteQuery(queryAnos, parameters: null))
//        {
//            int ano = (int)row["ano"];

//            if (!anosDict.TryGetValue(ano, out ControleImportacaoTable? controleExistente))
//            {
//                var controleImportacao = new ControleImportacaoTable
//                {
//                    ImportedDate = DateTime.UtcNow,
//                    Ano = ano,
//                    StatusImportacao = StatusImportacaoDados.Pendente,
//                    DescricaoStatusImportacao = StatusImportacaoDados.Pendente.ToString(),
//                    QuantidadeRegistros = 0
//                };

//                novosControlesImportacao.Add(controleImportacao);
//                anosDict[ano] = controleImportacao;
//            }
//            else
//            {
//                if (controleExistente.StatusImportacao == StatusImportacaoDados.Erro)
//                {
//                    controleExistente.StatusImportacao = StatusImportacaoDados.Pendente;
//                    controleExistente.DescricaoStatusImportacao = StatusImportacaoDados.Pendente.ToString();
//                    controleExistente.DataInicio = null;
//                    controleExistente.DataFim = null;
//                    controleExistente.QuantidadeRegistros = 0;

//                    await controleRepo.UpdateAsync(controleExistente);
//                }
//            }
//        }

//        await provider.GetRequiredService<DatabaseContext>().SaveChangesAsync();

//        string queryLocalizacao = @"
//SELECT DISTINCT
//    dados.sigla_uf AS sigla_uf,
//    diretorio_sigla_uf.nome AS sigla_uf_nome,
//    diretorio_sigla_uf.regiao AS regiao_uf_nome,
//    dados.id_municipio AS id_municipio,
//    diretorio_id_municipio.nome AS id_municipio_nome
//FROM `basedosdados.br_ibge_pam.lavoura_permanente` AS dados
//LEFT JOIN (SELECT DISTINCT sigla,nome,regiao FROM `basedosdados.br_bd_diretorios_brasil.uf`) AS diretorio_sigla_uf
//    ON dados.sigla_uf = diretorio_sigla_uf.sigla
//LEFT JOIN (SELECT DISTINCT id_municipio,nome FROM `basedosdados.br_bd_diretorios_brasil.municipio`) AS diretorio_id_municipio
//    ON dados.id_municipio = diretorio_id_municipio.id_municipio
//";

//        var resultLocalizacao = client.ExecuteQuery(queryLocalizacao, parameters: null).ToList();

//        // ## Importar UFs ##

//        var novosUFs = new List<UFTable>();
//        var ufDict = ufsExistentes.ToDictionary(u => u.SiglaUF, u => u);

//        foreach (var row in client.ExecuteQuery(queryLocalizacao, parameters: null))
//        {
//            string siglaUF = (string)row["sigla_uf"];
//            string nomeUF = (string)row["sigla_uf_nome"];
//            string regiaoUF = (string)row["regiao_uf_nome"];

//            if (!ufDict.ContainsKey(siglaUF))
//            {
//                var uf = new UFTable
//                {
//                    ImportedDate = DateTime.UtcNow,
//                    NomeUF = nomeUF,
//                    SiglaUF = siglaUF,
//                };
//                novosUFs.Add(uf);
//                ufDict[siglaUF] = uf;
//            }
//        }

//        if (novosUFs.Count > 0)
//        {
//            await ufRepo.AddRangeAsync(novosUFs);
//            await provider.GetRequiredService<DatabaseContext>().SaveChangesAsync();

//            Console.WriteLine($"Importação unidades federais importadas com sucesso! Quantidade: {novosUFs.Count}");
//        }

//        ufsExistentes = await ufRepo.Query().OfType<UFTable>().AsNoTracking().ToListAsync();
//        ufDict = ufsExistentes.ToDictionary(u => u.SiglaUF, u => u);

//        // ## Importar lavouras permanentes ##
//        string queryProdutos = @"
//        SELECT DISTINCT
//            dados.produto as produto,
//        FROM `basedosdados.br_ibge_pam.lavoura_permanente` AS dados
//";

//        var novosProdutos = new List<ProducaoTable>();

//        foreach (var row in client.ExecuteQuery(queryProdutos, parameters: null))
//        {
//            string produto = (string)row["produto"];

//            var producao = producaoRepo.Query()
//                                    .OfType<ProducaoTable>()
//                                    .Any(p => p.Descricao == produto);
//            if (!producaoDict.ContainsKey(produto))
//            {
//                var prod = new ProducaoTable
//                {
//                    ImportedDate = DateTime.UtcNow,
//                    Descricao = produto,
//                    TipoLavoura = TipoLavoura.Permanente
//                };
//                novosProdutos.Add(prod);
//                producaoDict[produto] = prod;
//            }
//        }

//        if (novosProdutos.Count > 0)
//        {
//            await producaoRepo.AddRangeAsync(novosProdutos);
//            await provider.GetRequiredService<DatabaseContext>().SaveChangesAsync();

//            Console.WriteLine($"Importação produções importadas com sucesso! Quantidade: {novosProdutos.Count}");
//        }

//        string querydDadosLavouraPermanente = @"
//        SELECT
//            dados.ano as ano,
//            dados.id_municipio AS id_municipio,
//            dados.produto as produto,
//            dados.area_destinada_colheita as area_destinada_colheita,
//            dados.area_colhida as area_colhida,
//            dados.quantidade_produzida as quantidade_produzida,
//            dados.rendimento_medio_producao as rendimento_medio_producao,
//            dados.valor_producao as valor_producao
//        FROM `basedosdados.br_ibge_pam.lavoura_permanente` AS dados where dados.ano >= 2023
//";

//        var novosDados = new List<DadosLavouraPermanenteTable>();

//        foreach (var row in client.ExecuteQuery(querydDadosLavouraPermanente, parameters: null))
//        {
//            long ano = (long)row["ano"];
//            string produto = (string)row["produto"];
//            string siglaUf = (string)row["siglaUf"];

//            bool existe = await lavouraPermRepo.Query()
//                .OfType<DadosLavouraPermanenteTable>()
//                .AnyAsync(p => p.Ano == ano &&
//                               p.Uf.SiglaUF == siglaUf &&
//                               p.Producao.Descricao == produto);
//            if (!existe)
//            {
//                var registro = new DadosLavouraPermanenteTable
//                {
//                    ImportedDate = DateTime.UtcNow,
//                    Ano = ano,
//                    Producao = await producaoRepo.Query().OfType<ProducaoTable>().FirstAsync(p => p.Descricao == produto),
//                    AreaColhida = row["area_colhida"] is null ? 0 : Convert.ToInt64(row["area_colhida"]),
//                    AreaDestinadaColheita = row["area_destinada_colheita"] is null ? 0 : Convert.ToInt64(row["area_destinada_colheita"]),
//                    QuantidadeProduzida = row["quantidade_produzida"] is null ? 0 : Convert.ToDecimal(row["quantidade_produzida"]),
//                    RendimentoMedioProducao = row["rendimento_medio_producao"] is null ? 0 : Convert.ToDecimal(row["rendimento_medio_producao"]),
//                    ValorProducao = row["valor_producao"] is null ? 0 : Convert.ToDecimal(row["valor_producao"]),
//                };

//                novosDados.Add(registro);
//            }
//        }

//        if (novosDados.Count > 0)
//        {
//            await lavouraPermRepo.AddRangeAsync(novosDados);
//            await provider.GetRequiredService<DatabaseContext>().SaveChangesAsync();

//            Console.WriteLine($"Importação  dados lavoura permanente importados com sucesso! Quantidade: {novosDados.Count}");
//        }

//        Console.WriteLine("Importação finalizada com sucesso!");
//    }
//}

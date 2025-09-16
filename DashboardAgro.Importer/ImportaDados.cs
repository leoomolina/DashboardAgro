//using DashboardAgro.Application.CaseUses;
//using DashboardAgro.Domain.Contracts;
//using DashboardAgro.Domain.Enums;
//using DashboardAgro.Infraestructure;
//using DashboardAgro.Infraestructure.Tables;
//using Google.Cloud.BigQuery.V2;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;

//public class ImportaDados(BigQueryClient bigQueryClient, ServiceProvider serviceProvider)
//{
//    private readonly BigQueryClient _bigQueryClient = bigQueryClient;

//    private readonly ServiceProvider _serviceProvider = serviceProvider;

//    public async Task ImportarTodosDados()
//    {
//        IRepository<MunicipioTable> municipioRepo = await ImportarMunicipiosUFs();
//        IRepository<ProducaoTable> producaoRepo = await ImportarProdutos();

//        await AtualizarDadosLavouraPermanente(municipioRepo, producaoRepo);
//    }

//    public async Task AtualizarDadosLavouraPermanente(IRepository<MunicipioTable> municipioRepo, IRepository<ProducaoTable> producaoRepo)
//    {
//        var lavouraTempRepo = serviceProvider.GetRequiredService<IRepository<DadosLavouraTemporariaTable>>();
//        var lavouraPermRepo = serviceProvider.GetRequiredService<IRepository<DadosLavouraPermanenteTable>>();

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
//        FROM `basedosdados.br_ibge_pam.lavoura_permanente` AS dados WHERE dados.ano >= 2015
//";

//        var novosDados = new List<DadosLavouraPermanenteTable>();

//        foreach (var row in _bigQueryClient.ExecuteQuery(querydDadosLavouraPermanente, parameters: null))
//        {
//            long ano = (long)row["ano"];
//            string produto = (string)row["produto"];
//            string idMunicipio = (string)row["id_municipio"];

//            bool existe = await lavouraPermRepo.Query()
//                .OfType<DadosLavouraPermanenteTable>()
//                .AnyAsync(p => p.Ano == ano &&
//                               p.Municipio.IdMunicipioIBGE == idMunicipio &&
//                               p.Producao.Descricao == produto);
//            if (!existe)
//            {
//                var registro = new DadosLavouraPermanenteTable
//                {
//                    ImportedDate = DateTime.UtcNow,
//                    Ano = ano,
//                    Municipio = await municipioRepo.Query().OfType<MunicipioTable>().FirstAsync(m => m.IdMunicipioIBGE == idMunicipio),
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
//            await serviceProvider.GetRequiredService<DatabaseContext>().SaveChangesAsync();
//        }
//    }

//    public async Task<IRepository<ProducaoTable>> ImportarProdutos()
//    {
//        var producaoRepo = serviceProvider.GetRequiredService<IRepository<ProducaoTable>>();

//        var producoesExistentes = await producaoRepo.Query().OfType<ProducaoTable>().AsNoTracking().ToListAsync();

//        var producaoDict = producoesExistentes.ToDictionary(p => p.Descricao, p => p);

//        string queryProdutos = @"
//        SELECT DISTINCT
//            dados.produto as produto,
//        FROM `basedosdados.br_ibge_pam.lavoura_permanente` AS dados
//";

//        var novosProdutos = new List<ProducaoTable>();

//        foreach (var row in bigQueryClient.ExecuteQuery(queryProdutos, parameters: null))
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
//            await serviceProvider.GetRequiredService<DatabaseContext>().SaveChangesAsync();
//        }

//        return producaoRepo;
//    }

//    private async Task<IRepository<MunicipioTable>> ImportarMunicipiosUFs()
//    {
//        var ufRepo = serviceProvider.GetRequiredService<IRepository<UFTable>>();
//        var municipioRepo = serviceProvider.GetRequiredService<IRepository<MunicipioTable>>();

//        var ufsExistentes = await ufRepo.Query().OfType<UFTable>().AsNoTracking().ToListAsync();
//        var municipiosExistentes = await municipioRepo.Query().OfType<MunicipioTable>().AsNoTracking().ToListAsync();

//        var ufDict = ufsExistentes.ToDictionary(u => u.SiglaUF, u => u);
//        var municipioDict = municipiosExistentes.ToDictionary(m => m.IdMunicipioIBGE, m => m);

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

//        var resultLocalizacao = bigQueryClient.ExecuteQuery(queryLocalizacao, parameters: null).ToList();

//        // ## Importar UFs ##

//        var novosUFs = new List<UFTable>();

//        foreach (var row in bigQueryClient.ExecuteQuery(queryLocalizacao, parameters: null))
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
//                    NomeRegiao = regiaoUF
//                };
//                novosUFs.Add(uf);
//                ufDict[siglaUF] = uf;
//            }
//        }

//        if (novosUFs.Count > 0)
//        {
//            await ufRepo.AddRangeAsync(novosUFs);
//            await serviceProvider.GetRequiredService<DatabaseContext>().SaveChangesAsync();
//        }

//        ufsExistentes = await ufRepo.Query().OfType<UFTable>().AsNoTracking().ToListAsync();
//        ufDict = ufsExistentes.ToDictionary(u => u.SiglaUF, u => u);

//        // ## Importar municípios ##
//        var novosMunicipios = new List<MunicipioTable>();

//        foreach (var row in resultLocalizacao)
//        {
//            string idMunicipio = (string)row["id_municipio"];
//            string nomeMunicipio = (string)row["id_municipio_nome"];
//            string siglaUF = (string)row["sigla_uf"];

//            UFTable uf = ufDict[siglaUF];

//            if (!municipioDict.ContainsKey(idMunicipio))
//            {
//                var municipio = new MunicipioTable
//                {
//                    ImportedDate = DateTime.UtcNow,
//                    NomeMunicipio = nomeMunicipio,
//                    IdMunicipioIBGE = idMunicipio,
//                    IdUF = uf.Id
//                };

//                novosMunicipios.Add(municipio);
//                municipioDict[idMunicipio] = municipio;
//            }
//        }

//        if (novosMunicipios.Count > 0)
//        {
//            await municipioRepo.AddRangeAsync(novosMunicipios);
//            await serviceProvider.GetRequiredService<DatabaseContext>().SaveChangesAsync();
//        }

//        municipiosExistentes = await municipioRepo.Query().OfType<MunicipioTable>().AsNoTracking().ToListAsync();
//        municipioDict = municipiosExistentes.ToDictionary(m => m.IdMunicipioIBGE, m => m);
//        return municipioRepo;
//    }
//}
using DashboardAgro.Application.Contracts;
using DashboardAgro.Domain.Entities;
using DashboardAgro.Domain.Enums;

namespace DashboardAgro.Application.Handlers.Importacao
{
    public class ImportarDadosBigQueryHandler
    {
        private readonly IBigQueryService _bigQuery;
        private readonly IImportarDados _importarDadosBigQuery;

        public ImportarDadosBigQueryHandler(
            IBigQueryService bigQuery,
            IImportarDados importarDadosBigQuery)
        {
            _bigQuery = bigQuery;
            _importarDadosBigQuery = importarDadosBigQuery;
        }

        public async Task ExecutarCargaInicial()
        {
            var anosDisponiveis = _bigQuery.ObterAnosDisponiveisAsync();
            var novosDadosImportar = new Dictionary<int, ControleImportacao>();

            foreach (var ano in anosDisponiveis)
            {
                var controleImportacao = new ControleImportacao
                {
                    Ano = ano,
                    StatusImportacao = StatusImportacaoDados.Pendente,
                    DescricaoStatusImportacao = StatusImportacaoDados.Pendente.ToString(),
                    QuantidadeRegistros = 0,
                    ImportedDate = DateTime.UtcNow
                };

                novosDadosImportar.Add(ano, controleImportacao);
            }

            await ImportarCarga(novosDadosImportar);

            Console.WriteLine("Carga inicial concluída!");
        }

        public async Task ExecutarCargaIncremental(int anoInicial)
        {
            int anoAtual = DateTime.UtcNow.Year;
            var requisicoesImportacao = new Dictionary<int, ControleImportacao>();

            Console.WriteLine($"Importando dados de {anoInicial} até {anoAtual}");

            for (int ano = anoInicial; ano <= anoAtual; ano++)
            {
                if (!requisicoesImportacao.TryGetValue(ano, out ControleImportacao? value))
                {
                    requisicoesImportacao.Add(ano, new ControleImportacao
                    {
                        Ano = ano,
                        StatusImportacao = StatusImportacaoDados.Pendente,
                        DescricaoStatusImportacao = StatusImportacaoDados.Pendente.ToString(),
                        QuantidadeRegistros = 0,
                        ImportedDate = DateTime.UtcNow
                    });
                }
                else
                {
                    await _importarDadosBigQuery.AtualizarRequisicaoImportacaoAsync(value, StatusImportacaoDados.Pendente);
                }
            }

            await ImportarCarga(requisicoesImportacao);
            Console.WriteLine("Requisição de importacao de cargas finalizada.");
        }

        private async Task ImportarCarga(Dictionary<int, ControleImportacao> anos)
        {
            await _importarDadosBigQuery.ImportarAnosAsync(anos);
            await ExecutarImportacoesPendentesAsync();
        }

        private async Task ExecutarImportacoesPendentesAsync()
        {
            List<ControleImportacao> importacoesPendentes = _importarDadosBigQuery.GetImportacoesPendentes();

            if (importacoesPendentes.Count > 0)
            {

                foreach (ControleImportacao importacao in importacoesPendentes)
                {
                    try
                    {
                        await _importarDadosBigQuery.AtualizarRequisicaoImportacaoAsync(importacao, StatusImportacaoDados.EmProcessamento);

                        var unidadesFederativasBigQuery = _bigQuery.ObterUnidadesFederativas(importacao.Ano);
                        var unidadesFederativas = new Dictionary<string, UnidadeFederativa>();

                        foreach (var item in unidadesFederativasBigQuery)
                        {
                            unidadesFederativas.Add(item.SiglaUF, item);
                        }

                        Console.WriteLine($"Iniciou importação de Regiões! Ano: {importacao.Ano}");
                        importacao.QuantidadeRegistros = await _importarDadosBigQuery.ImportarRegioesUFsAsync(unidadesFederativas);
                        Console.WriteLine($"Regiões importadas com sucesso! Ano: {importacao.Ano} - Qtd: {importacao.QuantidadeRegistros}");

                        Console.WriteLine($"Iniciou importação de Unidades Federais! Ano: {importacao.Ano}");
                        int qtdUnidadesImportadas = await _importarDadosBigQuery.ImportarUnidadesFederativasAsync(unidadesFederativas);
                        Console.WriteLine($"Unidades Federais importadas com sucesso! Ano: {importacao.Ano} - Qtd: {qtdUnidadesImportadas}");

                        importacao.QuantidadeRegistros += qtdUnidadesImportadas;

                        // ## IMPORTAÇÃO DAS LAVOURAS E PRODUÇÕES PERMANENTES ##
                        Console.WriteLine($"Iniciou importação de Produções Permanentes! Ano: {importacao.Ano}");
                        var lavourasPermBigQuery = _bigQuery.ObterLavoura(importacao.Ano, TipoLavoura.Permanente);

                        int qtdProducoesImportadas = await _importarDadosBigQuery.ImportarProducoesAsync(lavourasPermBigQuery);
                        importacao.QuantidadeRegistros += qtdProducoesImportadas;
                        Console.WriteLine($"Produções Permanentes importadas com sucesso! Ano: {importacao.Ano} - Qtd: {qtdProducoesImportadas}");

                        Console.WriteLine($"Iniciou importação de Lavouras Permanentes! Ano: {importacao.Ano}");
                        int qtdLavourasPermanentes = await _importarDadosBigQuery.ImportarDadosLavouraPermanenteAsync(lavourasPermBigQuery);
                        importacao.QuantidadeRegistros += qtdLavourasPermanentes;
                        Console.WriteLine($"Lavouras Permanentes importadas com sucesso! Ano: {importacao.Ano} - Qtd: {qtdLavourasPermanentes}");

                        // ## IMPORTAÇÃO DAS LAVOURAS E PRODUÇÕES TEMPORÁRIAS ##
                        Console.WriteLine($"Iniciou importação de Produções Temporárias! Ano: {importacao.Ano}");
                        var lavourasTempBigQuery = _bigQuery.ObterLavoura(importacao.Ano, TipoLavoura.Temporaria);

                        int qtdProducoesTemporarias = await _importarDadosBigQuery.ImportarProducoesAsync(lavourasTempBigQuery);
                        importacao.QuantidadeRegistros += qtdProducoesTemporarias;
                        Console.WriteLine($"Produções Temporárias importadas com sucesso! Ano: {importacao.Ano} - Qtd: {qtdProducoesTemporarias}");

                        Console.WriteLine($"Iniciou importação de Lavouras Temporárias! Ano: {importacao.Ano}");
                        int qtdLavourasTemporarias = await _importarDadosBigQuery.ImportarDadosLavouraTemporariaAsync(lavourasTempBigQuery);
                        importacao.QuantidadeRegistros += qtdLavourasTemporarias;
                        Console.WriteLine($"Lavouras Permanentes importadas com sucesso! Ano: {importacao.Ano} - Qtd: {qtdLavourasTemporarias}");

                        importacao.QuantidadeRegistros += qtdLavourasTemporarias;

                        await _importarDadosBigQuery.AtualizarRequisicaoImportacaoAsync(importacao, StatusImportacaoDados.Concluido);
                    }
                    catch (Exception e)
                    {
                        importacao.DescricaoStatusImportacao = e.Message;

                        await _importarDadosBigQuery.AtualizarRequisicaoImportacaoAsync(importacao, StatusImportacaoDados.Erro);
                        throw;
                    }
                }


            }
        }
    }
}

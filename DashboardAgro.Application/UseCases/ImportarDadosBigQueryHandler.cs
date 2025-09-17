using DashboardAgro.Application.Interfaces;
using DashboardAgro.Domain.Contracts;
using DashboardAgro.Domain.Entities;
using DashboardAgro.Domain.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DashboardAgro.Application.UseCases
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

            await _importarDadosBigQuery.ImportarAnosAsync(novosDadosImportar);

            Console.WriteLine("Carga inicial concluída!");
        }

        public async Task ExecutarCargaIncremental()
        {
            var anoAtual = DateTime.UtcNow.Year;

            var anos = new Dictionary<int, ControleImportacao>
            {
                {
                    anoAtual, new ControleImportacao
                    {
                        Ano = anoAtual,
                        StatusImportacao = StatusImportacaoDados.Pendente,
                        DescricaoStatusImportacao = StatusImportacaoDados.Pendente.ToString(),
                        QuantidadeRegistros = 0,
                        ImportedDate = DateTime.UtcNow
                    }
                },
                {
                    anoAtual - 1, new ControleImportacao
                    {
                       Ano = anoAtual - 1,
                       StatusImportacao = StatusImportacaoDados.Pendente,
                       DescricaoStatusImportacao = StatusImportacaoDados.Pendente.ToString(),
                       QuantidadeRegistros = 0,
                       ImportedDate = DateTime.UtcNow
                    }
                }
            };

            await _importarDadosBigQuery.ImportarAnosAsync(anos);
            Console.WriteLine("Carga incremental concluída!");
        }
    }
}

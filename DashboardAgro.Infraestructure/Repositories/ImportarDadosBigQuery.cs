using DashboardAgro.Domain.Contracts;
using DashboardAgro.Domain.Entities;
using DashboardAgro.Domain.Enums;
using DashboardAgro.Infraestructure.Tables;
using Microsoft.EntityFrameworkCore;
namespace DashboardAgro.Infraestructure.Repositories
{
    public class ImportarDadosBigQuery : IImportarDados
    {
        protected readonly DatabaseContext _context;
        public ImportarDadosBigQuery(DatabaseContext context)
        {
            _context = context;
        }
        public async Task ImportarAnosAsync(Dictionary<int, ControleImportacao> anos)
        {
            ArgumentNullException.ThrowIfNull(anos);
            
            foreach (var ano in anos.Keys)
            {
                await ImportarAnoAsync(ano);
            }

            await Task.CompletedTask;
        }

        private async Task ImportarAnoAsync(int ano)
        {
            var controleExistente = await _context.ControleImportacaoTable
                .FirstOrDefaultAsync(c => c.Ano == ano);

            if (controleExistente == null)
            {
                var novoControle = new ControleImportacaoTable
                {
                    Ano = ano,
                    ImportedDate = DateTime.UtcNow,
                    StatusImportacao = StatusImportacaoDados.Pendente,
                    DescricaoStatusImportacao = StatusImportacaoDados.Pendente.ToString(),
                    QuantidadeRegistros = 0
                };

                _context.ControleImportacaoTable.Add(novoControle);
            }
            else
            {
                if (controleExistente.StatusImportacao == StatusImportacaoDados.Erro)
                {
                    controleExistente.StatusImportacao = StatusImportacaoDados.Pendente;
                    controleExistente.DescricaoStatusImportacao = StatusImportacaoDados.Pendente.ToString();
                    controleExistente.DataInicio = null;
                    controleExistente.DataFim = null;
                    controleExistente.QuantidadeRegistros = 0;

                    _context.ControleImportacaoTable.Update(controleExistente);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}

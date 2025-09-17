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

        public List<ControleImportacao> GetImportacoesPendentes() => [.. _context.ControleImportacaoTable.Where(c => c.StatusImportacao == StatusImportacaoDados.Pendente)];

        public async Task ImportarAnosAsync(Dictionary<int, ControleImportacao> anos)
        {
            ArgumentNullException.ThrowIfNull(anos);

            foreach (var ano in anos.Keys)
            {
                await ImportarAnoAsync(ano);
            }

            await Task.CompletedTask;
        }

        public async Task<int> ImportarUnidadesFederativasAsync(Dictionary<string, UnidadeFederativa> unidadesFederativas)
        {
            int qtdImportada = 0;

            List<RegiaoTable> regioes = unidadesFederativas
                .GroupBy(r => r.Value.Regiao)
                .Select(uf => new RegiaoTable
                {
                    Descricao = uf.Key.Descricao,
                    ImportedDate = DateTime.UtcNow,
                })
                .ToList();

            foreach (var regiao in regioes)
            {
                var regiaoExistente = _context.Regiao
                    .FirstOrDefault(c => c.Descricao == regiao.Descricao);

                if (regiaoExistente == null)
                {
                    _context.Regiao.Add(regiao);
                    qtdImportada++;
                }
            }

            foreach (var uf in unidadesFederativas)
            {
                var unidadeExistente = _context.UnidadeFederativa
                    .FirstOrDefault(c => c.SiglaUF == uf.Key);

                if (unidadeExistente == null)
                {
                    var novaUnidade = new UFTable
                    {
                        NomeUF = uf.Value.NomeUF,
                        SiglaUF = uf.Value.SiglaUF,
                        ImportedDate = DateTime.UtcNow,
                    };

                    _context.UnidadeFederativa.Add(novaUnidade);
                    qtdImportada++;
                }
                else
                {
                    unidadeExistente.NomeUF = uf.Value.NomeUF;
                    _context.UnidadeFederativa.Update(unidadeExistente);
                }
            }

            await _context.SaveChangesAsync();

            return qtdImportada;
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

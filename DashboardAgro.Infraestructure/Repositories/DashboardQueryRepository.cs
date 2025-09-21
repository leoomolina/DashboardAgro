using DashboardAgro.Application.Contracts;
using DashboardAgro.Domain.Entities;
using DashboardAgro.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DashboardAgro.Infraestructure.Repositories
{
    public class DashboardQueryRepository : IDashboardRepository
    {
        private readonly DatabaseContext _context;
        public DashboardQueryRepository(DatabaseContext context) => _context = context;

        public async Task<IEnumerable<RankingDashboard>> GetRankingsDashboardAsync(int ano, Agrupamento tipoRanking, int idRegiao, int idUf, int idProducao, TipoLavoura? tipoLavoura)
        {
            IQueryable<RankingDashboard> query = null!;

            switch (tipoRanking)
            {
                case Agrupamento.UnidadeFederativa:
                    var queryUfTemp = _context.DadosLavouraTemporaria
                        .Where(e => e.Ano == ano
                                    && (idRegiao == 0 || e.Uf.IdRegiao == idRegiao)
                                    && (idUf == 0 || e.IdUf == idUf)
                                    && (idProducao == 0 || e.IdProducao == idProducao))
                        .Select(e => new
                        {
                            e.Uf.NomeUF,
                            DescricaoRegiao = e.Uf.Regiao.Descricao,
                            e.QuantidadeProduzida
                        });

                    var queryUfPerm = _context.DadosLavouraPermanente
                        .Where(e => e.Ano == ano
                                    && (idRegiao == 0 || e.Uf.IdRegiao == idRegiao)
                                    && (idUf == 0 || e.IdUf == idUf)
                                    && (idProducao == 0 || e.IdProducao == idProducao))
                        .Select(e => new
                        {
                            e.Uf.NomeUF,
                            DescricaoRegiao = e.Uf.Regiao.Descricao,
                            e.QuantidadeProduzida
                        });

                    query = queryUfTemp
                        .Concat(queryUfPerm)
                        .GroupBy(x => new { x.NomeUF, x.DescricaoRegiao })
                        .Select(g => new RankingDashboard
                        {
                            Descricao = g.Key.NomeUF,
                            Valor = g.Sum(x => x.QuantidadeProduzida),
                            TipoRanking = tipoRanking,
                            Tag = g.Key.DescricaoRegiao
                        })
                        .OrderByDescending(r => r.Valor)
                        .Take(5);
                    break;
                case Agrupamento.Regiao:
                    var queryRegiaoTemp = _context.DadosLavouraTemporaria
                        .Where(e => e.Ano == ano && e.Producao.TipoLavoura == TipoLavoura.Temporaria
                                    && (idRegiao == 0 || e.Uf.IdRegiao == idRegiao)
                                    && (idUf == 0 || e.IdUf == idUf)
                                    && (idProducao == 0 || e.IdProducao == idProducao))
                        .Select(e => new
                        {
                            e.Uf.IdRegiao,
                            e.Uf.Regiao.Descricao,
                            e.QuantidadeProduzida
                        });

                    var queryRegiaoPerm = _context.DadosLavouraPermanente
                        .Where(e => e.Ano == ano && e.Producao.TipoLavoura == TipoLavoura.Permanente
                                    && (idRegiao == 0 || e.Uf.IdRegiao == idRegiao)
                                    && (idUf == 0 || e.IdUf == idUf)
                                    && (idProducao == 0 || e.IdProducao == idProducao))
                        .Select(e => new
                        {
                            e.Uf.IdRegiao,
                            e.Uf.Regiao.Descricao,
                            e.QuantidadeProduzida
                        });

                    query = queryRegiaoTemp
                        .Concat(queryRegiaoPerm)
                        .GroupBy(x => new { x.IdRegiao, x.Descricao })
                        .Select(g => new RankingDashboard
                        {
                            Descricao = g.Key.Descricao,
                            Valor = g.Sum(x => x.QuantidadeProduzida),
                            TipoRanking = tipoRanking
                        })
                        .OrderByDescending(r => r.Valor)
                        .Take(5);
                    break;
                case Agrupamento.ProducaoPermanente:
                    query = _context.DadosLavouraPermanente
                        .Where(e => e.Ano == ano && e.Producao.TipoLavoura == TipoLavoura.Permanente
                                    && (idRegiao == 0 || e.Uf.IdRegiao == idRegiao)
                                    && (idUf == 0 || e.IdUf == idUf)
                                    && (idProducao == 0 || e.IdProducao == idProducao))
                        .GroupBy(r => r.Producao)
                        .Select(e => new RankingDashboard
                        {
                            Descricao = e.Key.Descricao,
                            Valor = e.Sum(r => r.QuantidadeProduzida),
                            TipoRanking = tipoRanking
                        })
                        .OrderByDescending(e => e.Valor)
                        .Take(5);
                    break;
                case Agrupamento.ProducaoTemporaria:
                    query = _context.DadosLavouraTemporaria
                        .Where(e => e.Ano == ano && e.Producao.TipoLavoura == TipoLavoura.Temporaria
                                    && (idRegiao == 0 || e.Uf.IdRegiao == idRegiao)
                                    && (idUf == 0 || e.IdUf == idUf)
                                    && (idProducao == 0 || e.IdProducao == idProducao))
                        .GroupBy(r => r.Producao)
                        .Select(e => new RankingDashboard
                        {
                            Descricao = e.Key.Descricao,
                            Valor = e.Sum(r => r.QuantidadeProduzida),
                            TipoRanking = tipoRanking
                        })
                        .OrderByDescending(e => e.Valor)
                        .Take(5);
                    break;
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ResumoRegiao>> GetResumoRegioesAsync(
    int ano, int idRegiao, int idUf, int idProducao, TipoLavoura? tipoLavoura)
        {
            var queryPerm = _context.DadosLavouraPermanente
                .Where(r => r.Ano == ano && r.Producao.TipoLavoura == TipoLavoura.Permanente
                            && (idRegiao == 0 || r.Uf.IdRegiao == idRegiao)
                            && (idUf == 0 || r.IdUf == idUf)
                            && (idProducao == 0 || r.IdProducao == idProducao));

            var queryTemp = _context.DadosLavouraTemporaria
                .Where(r => r.Ano == ano && r.Producao.TipoLavoura == TipoLavoura.Temporaria
                            && (idRegiao == 0 || r.Uf.IdRegiao == idRegiao)
                            && (idUf == 0 || r.IdUf == idUf)
                            && (idProducao == 0 || r.IdProducao == idProducao));

            IQueryable<ResumoRegiao>? resumoQuery = null;

            resumoQuery = tipoLavoura switch
            {
                TipoLavoura.Permanente => queryPerm
                    .GroupBy(d => new { d.Uf.Regiao.Id, d.Uf.Regiao.Descricao })
                    .Select(g => new ResumoRegiao
                    {
                        Id = g.Key.Id,
                        Descricao = g.Key.Descricao,
                        AreaColhida = g.Sum(x => x.AreaColhida),
                        QuantidadeProduzida = g.Sum(x => x.QuantidadeProduzida),
                        ValorProducao = g.Sum(x => x.ValorProducao),
                        PrincipaisProducoes = string.Join(", ",
                            g.Select(x => x.Producao.Descricao)
                             .Distinct()
                             .Take(3))
                    }),

                TipoLavoura.Temporaria => queryTemp
                    .GroupBy(d => new { d.Uf.Regiao.Id, d.Uf.Regiao.Descricao })
                    .Select(g => new ResumoRegiao
                    {
                        Id = g.Key.Id,
                        Descricao = g.Key.Descricao,
                        AreaColhida = g.Sum(x => x.AreaColhida),
                        QuantidadeProduzida = g.Sum(x => x.QuantidadeProduzida),
                        ValorProducao = g.Sum(x => x.ValorProducao),
                        PrincipaisProducoes = string.Join(", ",
                            g.Select(x => x.Producao.Descricao)
                             .Distinct()
                             .Take(3))
                    }),

                _ => queryPerm
                    .Select(d => new ResumoRegiao
                    {
                        Id = d.Uf.Regiao.Id,
                        Descricao = d.Uf.Regiao.Descricao,
                        AreaColhida = d.AreaColhida,
                        QuantidadeProduzida = d.QuantidadeProduzida,
                        ValorProducao = d.ValorProducao,
                        PrincipaisProducoes = d.Producao.Descricao
                    })
                    .Concat(
                        queryTemp.Select(d => new ResumoRegiao
                        {
                            Id = d.Uf.Regiao.Id,
                            Descricao = d.Uf.Regiao.Descricao,
                            AreaColhida = d.AreaColhida,
                            QuantidadeProduzida = d.QuantidadeProduzida,
                            ValorProducao = d.ValorProducao,
                            PrincipaisProducoes = d.Producao.Descricao
                        })
                    )
                    .GroupBy(r => new { r.Id, r.Descricao })
                    .Select(g => new ResumoRegiao
                    {
                        Id = g.Key.Id,
                        Descricao = g.Key.Descricao,
                        AreaColhida = g.Sum(x => x.AreaColhida),
                        QuantidadeProduzida = g.Sum(x => x.QuantidadeProduzida),
                        ValorProducao = g.Sum(x => x.ValorProducao),
                        PrincipaisProducoes = ""
                    })
            };

            return await resumoQuery.OrderBy(r => r.Descricao).ToListAsync();
        }


    }
}

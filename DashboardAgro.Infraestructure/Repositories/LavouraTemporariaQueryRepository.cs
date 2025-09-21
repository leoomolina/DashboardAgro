using DashboardAgro.Application.Contracts;
using DashboardAgro.Domain.Entities;
using DashboardAgro.Domain.Enums;
using DashboardAgro.Infraestructure.Tables;
using Microsoft.EntityFrameworkCore;

namespace DashboardAgro.Infraestructure.Repositories
{
    public class LavouraTemporariaQueryRepository : ILavouraTemporariaRepository
    {
        private readonly DatabaseContext _context;

        public LavouraTemporariaQueryRepository(DatabaseContext context)
        {
            _context = context;
        }

        public Task AddAsync(Lavoura lavoura)
        {
            DadosLavouraTemporariaTable record = new()
            {
                Ano = lavoura.Ano,
                AreaColhida = lavoura.AreaColhida,
                QuantidadeProduzida = lavoura.QuantidadeProduzida,
                ValorProducao = lavoura.ValorProducao,
            };

            _context.DadosLavouraTemporaria.Add(record);

            return _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Lavoura>> GetAllAsync()
        {
            return await _context.Producao
            .Where(p => p.TipoLavoura == Domain.Enums.TipoLavoura.Temporaria)
            .Select(p => new Lavoura
            {
                IdProducao = p.Id,
                Producao = p.Descricao,
                Tipo = p.TipoLavoura
            })
            .ToListAsync();
        }

        public async Task<IEnumerable<Lavoura>> GetByAnoAsync(int ano)
        {
            return await _context.DadosLavouraPermanente
            .Where(p => p.Ano == ano && p.Producao.TipoLavoura == Domain.Enums.TipoLavoura.Temporaria)
            .Select(p => new Lavoura
            {
                Ano = p.Ano,
                UF = new UnidadeFederativa
                {
                    Id = p.Uf.Id,
                    SiglaUF = p.Uf.SiglaUF,
                    NomeUF = p.Uf.NomeUF,
                    Regiao = new Regiao
                    {
                        Id = p.Uf.Regiao.Id,
                        Descricao = p.Uf.Regiao.Descricao
                    }
                },
                Producao = p.Producao.Descricao,
                AreaColhida = p.AreaColhida,
                QuantidadeProduzida = p.QuantidadeProduzida,
                ValorProducao = p.ValorProducao,
            })
            .ToListAsync();
        }

        public async Task<List<ResumoLavouraAno>> GetResumoAnualLavouraTemporariaAsync(int ano, int idRegiao, int idUf, int idProducao)
        {
            var query = _context.DadosLavouraTemporaria
                .Where(d => d.Ano == ano);

            if (idUf > 0)
                query = query.Where(d => d.IdUf == idUf);

            if (idRegiao > 0)
                query = query.Where(d => d.Uf.IdRegiao == idRegiao);

            if (idProducao > 0)
                query = query.Where(d => d.IdProducao == idProducao);

            return await query.GroupBy(d => new { d.IdUf })
                .Select(g => new ResumoLavouraAno
                {
                    SiglaUf = g.First().Uf.SiglaUF,
                    TipoLavoura = TipoLavoura.Temporaria,
                    Ano = ano,
                    AreaColhida = g.Sum(x => x.AreaColhida),
                    QuantidadeProduzida = g.Sum(x => x.QuantidadeProduzida),
                    AreaPlantadaXDestinadaColheita = g.Sum(x => x.AreaPlantada),
                    ValorProducao = g.Sum(x => x.ValorProducao),
                    DescricaoRegiao = g.First().Uf.Regiao.Descricao,
                })
                .ToListAsync();
        }

        public async Task<List<ResumoLavouraAno>> GetResumoAnualByLavouraAsync(int ano, int idRegiao, int idUf, int idProducao)
        {
            var query = _context.DadosLavouraTemporaria
                .Where(d => d.Ano == ano && d.IdProducao == idProducao);

            if (idUf > 0)
                query = query.Where(d => d.IdUf == idUf);

            if (idRegiao > 0)
                query = query.Where(d => d.Uf.IdRegiao == idRegiao);

            if (idProducao > 0)
                query = query.Where(d => d.IdProducao == idProducao);

            return await query.GroupBy(d => new { d.IdUf })
                .Select(g => new ResumoLavouraAno
                {
                    SiglaUf = g.First().Uf.SiglaUF,
                    TipoLavoura = TipoLavoura.Temporaria,
                    Ano = ano,
                    AreaPlantadaXDestinadaColheita = g.Sum(x => x.AreaPlantada),
                    AreaColhida = g.Sum(x => x.AreaColhida),
                    QuantidadeProduzida = g.Sum(x => x.QuantidadeProduzida),
                    ValorProducao = g.Sum(x => x.ValorProducao),
                    DescricaoRegiao = g.First().Uf.Regiao.Descricao,
                })
                .ToListAsync();
        }

        public Task<IEnumerable<(UnidadeFederativa UnidadeFederativa, decimal Producao)>> GetTopUFsAsync(int ano, int idProducao, int top = 10)
        {
            throw new NotImplementedException();
        }
    }
}

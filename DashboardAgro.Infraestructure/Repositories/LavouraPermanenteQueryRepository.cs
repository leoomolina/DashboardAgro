using DashboardAgro.Application.Contracts;
using DashboardAgro.Application.DTOs;
using DashboardAgro.Domain.Entities;
using DashboardAgro.Infraestructure.Tables;
using Microsoft.EntityFrameworkCore;

namespace DashboardAgro.Infraestructure.Repositories
{
    public class LavouraPermanenteQueryRepository : ILavouraPermanenteRepository
    {
        private readonly DatabaseContext _context;

        public LavouraPermanenteQueryRepository(DatabaseContext context)
        {
            _context = context;
        }

        public Task AddAsync(Lavoura lavoura)
        {
            DadosLavouraPermanenteTable record = new()
            {
                Ano = lavoura.Ano,
                AreaColhida = lavoura.AreaColhida,
                QuantidadeProduzida = lavoura.QuantidadeProduzida,
                ValorProducao = lavoura.ValorProducao,
            };

            _context.DadosLavouraPermanente.Add(record);

            return _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Lavoura>> GetAllAsync()
        {
            return await _context.Producao
            .Where(p => p.TipoLavoura == Domain.Enums.TipoLavoura.Permanente)
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
            .Where(p => p.Ano == ano && p.Producao.TipoLavoura == Domain.Enums.TipoLavoura.Permanente)
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

        public async Task<List<ResumoAnoDTO>> GetResumoAnualLavouraPermanenteAsync(int ano) => await _context.DadosLavouraPermanente
                .Where(d => d.Ano == ano)
                .GroupBy(d => new { d.IdUf })
                .Select(g => new ResumoAnoDTO
                {
                    SiglaUf = g.First().Uf.SiglaUF,
                    Ano = ano,
                    AreaColhida = g.Sum(x => x.AreaColhida),
                    QuantidadeProduzida = g.Sum(x => x.QuantidadeProduzida),
                    ValorProducao = g.Sum(x => x.ValorProducao),
                    RendimentoMedioProducao = g.Average(x => x.RendimentoMedioProducao),
                    DescricaoRegiao = g.First().Uf.Regiao.Descricao,
                })
                .ToListAsync();

        public async Task<List<ResumoAnoByLavouraDTO>> GetResumoAnualByLavouraAsync(ResumoAnoByLavouraDTO.ResumoAnoByLavouraFilter filter) => await _context.DadosLavouraPermanente
                .Where(d => d.Ano == filter.Ano && d.IdProducao == filter.IdProducao)
                .GroupBy(d => new { d.IdUf })
                .Select(g => new ResumoAnoByLavouraDTO
                {
                    SiglaUf = g.First().Uf.SiglaUF,
                    Ano = filter.Ano,
                    AreaColhida = g.Sum(x => x.AreaColhida),
                    QuantidadeProduzida = g.Sum(x => x.QuantidadeProduzida),
                    ValorProducao = g.Sum(x => x.ValorProducao),
                    RendimentoMedioProducao = g.Average(x => x.RendimentoMedioProducao),
                    DescricaoRegiao = g.First().Uf.Regiao.Descricao,
                })
                .ToListAsync();



        public Task<IEnumerable<Lavoura>> GetByCulturaAsync(int ano, int idCultura)
        {
            throw new NotImplementedException();
        }

        public Task<Lavoura?> GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetLavouraPermanenteByAnoAsync(int ano)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetProdutividadeMediaAsync(int ano, int idCultura)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<(UnidadeFederativa UnidadeFederativa, decimal Producao)>> GetTopUFsAsync(int ano, int idProducao, int top = 10)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Lavoura entity)
        {
            throw new NotImplementedException();
        }
    }
}

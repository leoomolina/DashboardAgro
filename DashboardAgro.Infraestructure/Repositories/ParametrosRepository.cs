using DashboardAgro.Application.Contracts;
using DashboardAgro.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DashboardAgro.Infraestructure.Repositories
{
    public class ParametrosRepository : IParametrosRepository
    {
        private readonly DatabaseContext _context;
        public ParametrosRepository(DatabaseContext context) => _context = context;

        public async Task<IEnumerable<Regiao>> GetAllRegiao()
        {
            return await _context.Regiao.Select(r => new Regiao
            {
                Id = r.Id,
                Descricao = r.Descricao
            }).ToListAsync();
        }

        public async Task<IEnumerable<UnidadeFederativa>> GetAllUnidadesFederativas(int? idRegiao)
        {
            if (idRegiao.HasValue)
            {
                return await _context.UnidadeFederativa
                    .Where(uf => uf.Regiao.Id == idRegiao.Value)
                    .Select(r => new UnidadeFederativa
                    {
                        Id = r.Id,
                        SiglaUF = r.SiglaUF,
                        NomeUF = r.NomeUF,
                        Regiao = new Regiao { Id = r.Regiao.Id, Descricao = r.Regiao.Descricao }
                    }).ToListAsync();
            }
            else
            {
                return await _context.UnidadeFederativa.Select(r => new UnidadeFederativa
                {
                    Id = r.Id,
                    SiglaUF = r.SiglaUF,
                    NomeUF = r.NomeUF,
                    Regiao = new Regiao { Id = r.Regiao.Id, Descricao = r.Regiao.Descricao }
                }).ToListAsync();
            }
        }
    }
}

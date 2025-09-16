using DashboardAgro.Domain.Contracts;
using DashboardAgro.Domain.Entities;
using DashboardAgro.Infraestructure.Tables;
using Microsoft.EntityFrameworkCore;

namespace DashboardAgro.Infraestructure.Repositories
{
    public class DadosLavouraRepository : ILavouraRepository
    {
        private readonly DatabaseContext _context;

        public DadosLavouraRepository(DatabaseContext context)
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

        public Task DeleteAsync(Lavoura entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Lavoura>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Lavoura>> GetByAnoAsync(int ano)
        {
            return await _context.DadosLavouraPermanente
            .Where(p => p.Ano == ano && p.Producao.TipoLavoura == Domain.Enums.TipoLavoura.Permanente)
            .Select(p => new Lavoura
            {
                Ano = p.Ano,
                SiglaUF = p.Uf.SiglaUF,
                Produto = p.Producao.Descricao,
                AreaColhida = p.AreaColhida,
                QuantidadeProduzida = p.QuantidadeProduzida,
                ValorProducao = p.ValorProducao,
            })
            .ToListAsync();
        }

        public Task<IEnumerable<Lavoura>> GetByCulturaAsync(int ano, int idCultura)
        {
            throw new NotImplementedException();
        }

        public Task<Lavoura?> GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetProdutividadeMediaAsync(int ano, int idCultura)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<(string Municipio, decimal Producao)>> GetTopMunicipiosAsync(int ano, int idCultura, int top = 10)
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

        Task<IEnumerable<Lavoura>> ILavouraRepository.GetByAnoAsync(int ano)
        {
            throw new NotImplementedException();
        }
    }
}

using DashboardAgro.Domain.Contracts;
using DashboardAgro.Domain.Entities;
using DashboardAgro.Domain.Enums;
using DashboardAgro.Domain.ValueObjects;
using DashboardAgro.Infraestructure.Tables;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DashboardAgro.Infraestructure.Repositories
{
    public class ImportarDadosBigQuery : IImportarDados
    {
        protected readonly DatabaseContext _context;
        public ImportarDadosBigQuery(DatabaseContext context)
        {
            _context = context;
        }

        public async Task AtualizarRequisicaoImportacaoAsync(ControleImportacao controleImportacao, StatusImportacaoDados novoStatus)
        {
            ArgumentException.ThrowIfNullOrEmpty(controleImportacao?.Ano.ToString());

            ControleImportacao? controleExistente = await _context.ControleImportacaoTable
                .FirstOrDefaultAsync(c => c.Ano == controleImportacao.Ano)
                ?? throw new KeyNotFoundException($"Nenhum controle de importação encontrado para o ano {controleImportacao.Ano}.");

            switch (novoStatus)
            {
                case StatusImportacaoDados.Pendente:
                    controleExistente.StatusImportacao = StatusImportacaoDados.Pendente;
                    controleExistente.DescricaoStatusImportacao = "";
                    controleExistente.QuantidadeRegistros = 0;
                    controleExistente.DataInicio = null;
                    controleExistente.DataFim = null;
                    break;
                case StatusImportacaoDados.EmProcessamento:
                    controleExistente.StatusImportacao = StatusImportacaoDados.EmProcessamento;
                    controleExistente.DescricaoStatusImportacao = "";
                    controleExistente.QuantidadeRegistros = 0;
                    controleExistente.DataInicio = DateTime.UtcNow;
                    controleExistente.DataFim = null;
                    break;
                case StatusImportacaoDados.Concluido:
                    controleExistente.StatusImportacao = StatusImportacaoDados.Concluido;
                    controleExistente.DescricaoStatusImportacao = "";
                    controleExistente.QuantidadeRegistros = controleExistente.QuantidadeRegistros;
                    controleExistente.DataFim = DateTime.UtcNow;
                    break;
                case StatusImportacaoDados.Erro:
                    controleExistente.StatusImportacao = StatusImportacaoDados.Erro;
                    controleExistente.DescricaoStatusImportacao = controleExistente.DescricaoStatusImportacao;
                    controleExistente.QuantidadeRegistros = 0;
                    controleExistente.DataFim = DateTime.UtcNow;
                    break;
                default:
                    return;
            }

            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public List<ControleImportacao> GetImportacoesPendentes() => [.. _context.ControleImportacaoTable.Where(c => c.StatusImportacao == StatusImportacaoDados.Pendente)];

        public async Task ImportarAnosAsync(Dictionary<int, ControleImportacao> anos)
        {
            ArgumentNullException.ThrowIfNull(anos);

            foreach (var ano in anos.Keys)
            {
                await EnviarRequisicaoImportacaoAsync(ano);
            }

            await Task.CompletedTask;
        }

        public Task<int> ImportarDadosLavouraPermanenteAsync(Dictionary<LavouraKey, Lavoura> lavouras)
        {
            return ImportarDadosLavouraAsync(
                lavouras,
                    (ano, l, idProducao, idUf) => new DadosLavouraPermanenteTable
                    {
                        Ano = ano,
                        IdProducao = idProducao,
                        IdUf = idUf,
                        AreaColhida = l.AreaColhida,
                        AreaDestinadaColheita = l.AreaDestinadaColheita,
                        QuantidadeProduzida = l.QuantidadeProduzida,
                        ValorProducao = l.ValorProducao,
                        ImportedDate = DateTime.UtcNow,
                        RendimentoMedioProducao = l.Produtividade
                    },
                    _context.DadosLavouraPermanente
                );
        }

        public Task<int> ImportarDadosLavouraTemporariaAsync(Dictionary<LavouraKey, Lavoura> lavouras)
        {
            return ImportarDadosLavouraAsync(
                lavouras,
                (ano, l, idProducao, idUf) => new DadosLavouraTemporariaTable
                {
                    Ano = ano,
                    IdProducao = idProducao,
                    IdUf = idUf,
                    AreaColhida = l.AreaColhida,
                    AreaPlantada = l.AreaPlantada,
                    QuantidadeProduzida = l.QuantidadeProduzida,
                    ValorProducao = l.ValorProducao,
                    ImportedDate = DateTime.UtcNow,
                    RendimentoMedioProducao = l.Produtividade
                },
                _context.DadosLavouraTemporaria
            );
        }

        public async Task<int> ImportarProducoesAsync(Dictionary<LavouraKey, Lavoura> lavouras)
        {
            List<ProducaoTable> producoes = [.. lavouras
                .GroupBy(r => r.Value.Producao)
                .Select(p => new ProducaoTable
                {
                    Descricao = p.Key,
                    TipoLavoura = p.First().Value.Tipo
                })];

            var descricoes = producoes.Select(p => p.Descricao).ToList();

            var existentes = await _context.Producao
                                        .Where(c => descricoes.Contains(c.Descricao))
                                        .Select(c => c.Descricao)
                                        .ToListAsync();

            var novas = producoes
                .Where(p => !existentes.Contains(p.Descricao))
                .ToList();

            if (novas.Count > 0)
            {
                _context.Producao.AddRange(novas);
                await _context.SaveChangesAsync();
            }

            return novas.Count;
        }

        public async Task<int> ImportarRegioesUFsAsync(Dictionary<string, UnidadeFederativa> unidadesFederativas)
        {
            List<RegiaoTable> regioes = [.. unidadesFederativas
                .GroupBy(r => r.Value.Regiao.Descricao)
                .Select(uf => new RegiaoTable
                {
                    Descricao = uf.Key
                })];

            var descricoes = regioes.Select(p => p.Descricao).ToList();

            var existentes = await _context.Regiao
                                        .Where(c => descricoes.Contains(c.Descricao))
                                        .Select(c => c.Descricao)
                                        .ToListAsync();

            var novas = regioes
                .Where(p => !existentes.Contains(p.Descricao))
                .ToList();

            if (novas.Count > 0)
            {
                _context.Regiao.AddRange(novas);
                await _context.SaveChangesAsync();
            }

            return novas.Count;
        }

        public async Task<int> ImportarUnidadesFederativasAsync(Dictionary<string, UnidadeFederativa> unidadesFederativas)
        {
            var siglas = unidadesFederativas.Select(p => p.Key).ToList();

            var existentes = await _context.UnidadeFederativa
                                        .Where(c => siglas.Contains(c.SiglaUF))
                                        .Select(c => c.SiglaUF)
                                        .ToListAsync();

            var novas = unidadesFederativas
                .Where(p => !existentes.Contains(p.Key))
                .ToList();

            if (novas.Count > 0)
            {
                _context.UnidadeFederativa.AddRange([.. novas
                .Select(uf => new UFTable
                {
                    NomeUF = uf.Value.NomeUF,
                    SiglaUF = uf.Value.SiglaUF,
                    IdRegiao = _context.Regiao.First(r => r.Descricao == uf.Value.Regiao.Descricao).Id,
                    ImportedDate = DateTime.UtcNow
                })]);

                await _context.SaveChangesAsync();
            }

            return novas.Count;
        }

        private async Task EnviarRequisicaoImportacaoAsync(int ano)
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
                await _context.SaveChangesAsync();
            }
            else
            {
                AtualizarRequisicaoImportacaoAsync(controleExistente, StatusImportacaoDados.Pendente).Wait();
            }
        }

        private async Task<int> ImportarDadosLavouraAsync<T>(
            Dictionary<LavouraKey, Lavoura> lavouras,
            Func<int, Lavoura, int, int, T> factory,
            DbSet<T> dbSet) where T : class, ILavouraEntity
        {
            if (lavouras.All(l => l.Value == null))
                return 0;

            var producoesDescricao = lavouras.Values.Select(l => l.Producao).Distinct().ToList();
            var producoesExistentes = await _context.Producao
                .Where(p => producoesDescricao.Contains(p.Descricao))
                .ToDictionaryAsync(p => p.Descricao, p => p.Id);

            var siglasUF = lavouras.Values.Select(l => l.UF.SiglaUF).Distinct().ToList();
            var ufsExistentes = await _context.UnidadeFederativa
                .Where(uf => siglasUF.Contains(uf.SiglaUF))
                .ToDictionaryAsync(uf => uf.SiglaUF, uf => uf.Id);

            var lavourasValidas = lavouras
                .Where(l => ufsExistentes.ContainsKey(l.Value.UF.SiglaUF))
                .ToDictionary(k => k.Key, v => v.Value);

            var entidades = lavourasValidas
                .Select(p => factory(
                    p.Key.Ano,
                    p.Value,
                    producoesExistentes[p.Value.Producao],
                    ufsExistentes[p.Value.UF.SiglaUF]))
                .ToList();

            var anos = entidades.Select(e => e.Ano).Distinct().ToList();
            var idsProducoes = entidades.Select(e => e.IdProducao).Distinct().ToList();
            var idsUf = entidades.Select(e => e.IdUf).Distinct().ToList();

            var existentes = await dbSet
                .Where(d => anos.Contains(d.Ano)
                         && idsProducoes.Contains(d.IdProducao)
                         && idsUf.Contains(d.IdUf))
                .Select(d => new { d.Ano, d.IdProducao, d.IdUf })
                .ToListAsync();

            

            var novos = entidades
                .Where(c => !existentes.Any(e => e.Ano == c.Ano
                                              && e.IdProducao == c.IdProducao
                                              && e.IdUf == c.IdUf))
                .ToList();

            if (novos.Count != 0)
            {
                dbSet.AddRange(novos);
                await _context.SaveChangesAsync();
            }

            return novos.Count;
        }
    }
}

using DashboardAgro.Domain.Entities;
using DashboardAgro.Domain.Enums;
using DashboardAgro.Domain.ValueObjects;

namespace DashboardAgro.Application.Contracts
{
    public interface IImportarDados
    {
        Task AtualizarRequisicaoImportacaoAsync(ControleImportacao controleImportacao, StatusImportacaoDados novoStatus);
        Task ImportarAnosAsync(Dictionary<int, ControleImportacao> anos);
        List<ControleImportacao> GetImportacoesPendentes();
        Task<int> ImportarUnidadesFederativasAsync(Dictionary<string, UnidadeFederativa> unidadesFederativas);
        Task<int> ImportarRegioesUFsAsync(Dictionary<string, UnidadeFederativa> unidadesFederativas);
        Task<int> ImportarDadosLavouraTemporariaAsync(Dictionary<LavouraKey, Lavoura> lavouras);
        Task<int> ImportarDadosLavouraPermanenteAsync(Dictionary<LavouraKey, Lavoura> lavouras);
        Task<int> ImportarProducoesAsync(Dictionary<LavouraKey, Lavoura> lavouras);
        Task<IEnumerable<int>> GetAnosDisponiveisAsync();
        Task<IEnumerable<ControleImportacao>> GetImportacao(StatusImportacaoDados? status);
    }
}

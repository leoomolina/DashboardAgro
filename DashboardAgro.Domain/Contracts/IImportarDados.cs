using DashboardAgro.Domain.Entities;
using DashboardAgro.Domain.Enums;
using DashboardAgro.Domain.ValueObjects;

namespace DashboardAgro.Domain.Contracts
{
    public interface IImportarDados
    {
        Task AtualizarRequisicaoImportacaoAsync(ControleImportacao controleImportacao, StatusImportacaoDados novoStatus);
        Task ImportarAnosAsync(Dictionary<int, ControleImportacao> anos);
        Task<int> ImportarUnidadesFederativasAsync(Dictionary<string, UnidadeFederativa> unidadesFederativas);
        Task<int> ImportarRegioesUFsAsync(Dictionary<string, UnidadeFederativa> unidadesFederativas);
        Task<int> ImportarDadosLavouraTemporariaAsync(Dictionary<LavouraKey, Lavoura> lavouras);
        Task<int> ImportarDadosLavouraPermanenteAsync(Dictionary<LavouraKey, Lavoura> lavouras);
        Task<int> ImportarProducoesAsync(Dictionary<LavouraKey, Lavoura> lavouras);
        List<ControleImportacao> GetImportacoesPendentes();
    }
}

namespace DashboardAgro.Domain.Contracts
{
    public interface ILavouraEntity
    {
        int Ano { get; }
        int IdProducao { get; }
        int IdUf { get; }
        string SiglaUF { get; }
        string ProducaoDescricao { get; }
    }
}

namespace DashboardAgro.Domain.Entities
{
    public class ResumoDashboard
    {
        public string DescricaoRegiao { get; set; }
        public string SiglaUf { get; set; }
        public long Ano { get; set; }
        public decimal AreaColhida { get; set; }
        public decimal QuantidadeProduzida { get; set; }
        public decimal ValorProducao { get; set; }
    }
}

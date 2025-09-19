namespace DashboardAgro.Application.DTOs
{
    public class ResumoAnoDTO
    {
        public string DescricaoRegiao { get; set; }
        public string SiglaUf { get; set; }
        public long Ano { get; set; }
        public decimal AreaColhida { get; set; }
        public decimal QuantidadeProduzida { get; set; }
        public decimal ValorProducao { get; set; }
        public decimal RendimentoMedioProducao { get; set; }
    }
}

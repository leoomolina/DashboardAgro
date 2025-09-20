namespace DashboardAgro.Application.DTOs
{
    public class ResumoAnoDTO
    {
        public string DescricaoRegiao { get; set; }
        public string SiglaUf { get; set; }
        public long Ano { get; set; }
        public decimal AreaColhidaTotal { get; set; }
        public decimal QuantidadeProduzidaTotal { get; set; }
        public decimal ValorProducaoTotal { get; set; }
        public List<LavouraDTO> Lavouras { get; set; }
    }
}

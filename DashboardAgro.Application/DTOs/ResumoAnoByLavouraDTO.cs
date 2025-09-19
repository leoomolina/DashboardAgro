namespace DashboardAgro.Application.DTOs
{
    public class ResumoAnoByLavouraDTO
    {
        public string DescricaoRegiao { get; set; }
        public string SiglaUf { get; set; }
        public string Producao { get; set; }
        public int Ano { get; set; }
        public decimal AreaColhida { get; set; }
        public decimal QuantidadeProduzida { get; set; }
        public decimal ValorProducao { get; set; }
        public decimal RendimentoMedioProducao { get; set; }

        public class ResumoAnoByLavouraFilter
        {
            public int Ano { get; set; }
            public int IdProducao { get; set; }
            public int IdUf { get; set; }
            public int IdRegiao { get; set; }
        }
    }
}

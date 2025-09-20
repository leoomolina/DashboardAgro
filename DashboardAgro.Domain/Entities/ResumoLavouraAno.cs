using DashboardAgro.Domain.Enums;

namespace DashboardAgro.Domain.Entities
{
    public class ResumoLavouraAno
    {
        public long Ano { get; set; }
        public string DescricaoRegiao { get; set; }
        public string SiglaUf { get; set; }
        public decimal AreaColhida { get; set; }
        public decimal AreaPlantadaXDestinadaColheita { get; set; }
        public decimal QuantidadeProduzida { get; set; }
        public decimal ValorProducao { get; set; }
        public decimal Produtividade => AreaColhida == 0 ? 0 : (QuantidadeProduzida / AreaColhida) * 100;
        public TipoLavoura TipoLavoura { get; set; }
    }
}

using DashboardAgro.Domain.Enums;

namespace DashboardAgro.Domain.Entities
{
    public class Lavoura
    {
        public long Ano { get; set; }
        public int IdRegiao { get; set; }
        public string SiglaUF { get; set; }
        public string NomeUF { get; set; }
        public string DescricaoRegiao { get; set; }
        public string Produto { get; set; }
        public decimal QuantidadeProduzida { get; set; }
        public long AreaColhida { get; set; }
        public long AreaDestinadaColheita { get; set; }
        public long AreaPlantada { get; set; }
        public decimal ValorProducao { get; set; }
        public decimal Produtividade => AreaColhida == 0 ? 0 : QuantidadeProduzida / AreaColhida;
        public TipoLavoura Tipo { get; set; }
    }
}

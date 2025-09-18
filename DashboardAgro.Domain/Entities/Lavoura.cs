using DashboardAgro.Domain.Contracts;
using DashboardAgro.Domain.Enums;

namespace DashboardAgro.Domain.Entities
{
    public class Lavoura : ILavouraEntity
    {
        public int Ano { get; set; }
        public int IdProducao { get; set; }
        public string Producao { get; set; }
        public decimal QuantidadeProduzida { get; set; }
        public long AreaColhida { get; set; }
        public long AreaDestinadaColheita { get; set; }
        public long AreaPlantada { get; set; }
        public decimal ValorProducao { get; set; }
        public decimal RendimentoMedioProducao { get; set; }
        public decimal Produtividade => AreaColhida == 0 ? 0 : QuantidadeProduzida / AreaColhida;
        public UnidadeFederativa UF { get; set; }
        public TipoLavoura Tipo { get; set; }
        public string ProducaoDescricao => Producao;
        public int IdUf => UF.Id;
        public string SiglaUF => UF.SiglaUF;
    }
}

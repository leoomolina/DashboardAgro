using System.ComponentModel.DataAnnotations.Schema;

namespace DashboardAgro.Infraestructure.Tables
{
    public abstract class LavouraBase : TableBase
    {
        public int Ano { get; set; }
        public long AreaColhida { get; set; }
        public decimal QuantidadeProduzida { get; set; }
        public decimal RendimentoMedioProducao { get; set; }
        public decimal ValorProducao { get; set; }
        public int IdProducao { get; set; }
        public int IdUf { get; set; }

        [ForeignKey("IdProducao")]
        public ProducaoTable Producao { get; set; }

        [ForeignKey("IdEstado")]
        public UFTable Uf { get; set; }
    }
}

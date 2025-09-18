using DashboardAgro.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace DashboardAgro.Infraestructure.Tables
{
    public abstract class LavouraBase : TableBase, ILavouraEntity
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

        [ForeignKey("IdUf")]
        public UFTable Uf { get; set; }

        string ILavouraEntity.SiglaUF => Uf != null ? Uf.SiglaUF : "";

        string ILavouraEntity.ProducaoDescricao => Producao != null ? Producao.Descricao : "";
    }
}

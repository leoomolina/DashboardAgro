using System.ComponentModel.DataAnnotations.Schema;

namespace DashboardAgro.Infraestructure.Tables
{
    public class UFTable : TableBase
    {
        public string SiglaUF { get; set; }
        public string NomeUF { get; set; }
        public int IdRegiao { get; set; }

        [ForeignKey("IdRegiao")]
        public RegiaoTable Regiao { get; set; }
    }
}

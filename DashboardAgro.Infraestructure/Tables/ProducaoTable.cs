using DashboardAgro.Domain.Enums;

namespace DashboardAgro.Infraestructure.Tables
{
    public class ProducaoTable : TableBase
    {
        public string Descricao { get; set; }
        public TipoLavoura TipoLavoura { get; set; }

    }
}

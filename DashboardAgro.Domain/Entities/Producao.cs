using DashboardAgro.Domain.Enums;

namespace DashboardAgro.Domain.Entities
{
    public class Producao
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public TipoLavoura TipoLavoura { get; set; }
    }
}

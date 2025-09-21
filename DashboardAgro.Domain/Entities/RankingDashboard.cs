using DashboardAgro.Domain.Enums;

namespace DashboardAgro.Domain.Entities
{
    public class RankingDashboard
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Tag { get; set; } = "";
        public decimal Valor { get; set; }
        public Agrupamento TipoRanking { get; set; }
    }
}

using DashboardAgro.Domain.Enums;

namespace DashboardAgro.Application.DTOs
{
    public class ProducaoDTO
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public TipoLavoura TipoLavoura { get; set; }
    }
}

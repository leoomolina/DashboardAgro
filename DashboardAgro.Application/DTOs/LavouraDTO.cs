namespace DashboardAgro.Application.DTOs
{
    public class LavouraDTO
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public decimal AreaColhida { get; set; }
        public decimal AreaPlantadaXDestinadaColheita { get; set; }
        public decimal QuantidadeProduzida { get; set; }
        public decimal ValorProducao { get; set; }
        public decimal Produtividade => AreaColhida == 0 ? 0 : QuantidadeProduzida / AreaColhida;
    }
}

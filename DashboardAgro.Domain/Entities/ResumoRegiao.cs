namespace DashboardAgro.Domain.Entities
{
    public class ResumoRegiao
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string PrincipaisProducoes { get; set; }
        public decimal AreaColhida { get; set; }
        public decimal QuantidadeProduzida { get; set; }
        public decimal ValorProducao { get; set; }
        public decimal Produtividade => AreaColhida == 0 ? 0 : QuantidadeProduzida / AreaColhida;
    }
}

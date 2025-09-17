using DashboardAgro.Domain.Enums;

namespace DashboardAgro.Domain.Entities
{
    public class ControleImportacao
    {
        public int Id { get; set; }
        public int Ano { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public StatusImportacaoDados StatusImportacao { get; set; }
        public DateTime ImportedDate { get; set; }
        public int QuantidadeRegistros { get; set; }
        public string? DescricaoStatusImportacao { get; set; }
    }
}

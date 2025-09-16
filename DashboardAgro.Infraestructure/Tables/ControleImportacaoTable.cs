using DashboardAgro.Domain.Enums;

namespace DashboardAgro.Infraestructure.Tables
{
    public class ControleImportacaoTable : TableBase
    {
        public int Ano { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public StatusImportacaoDados StatusImportacao { get; set; }
        public int QuantidadeRegistros { get; set; }
        public string? DescricaoStatusImportacao { get; set; }
    }
}

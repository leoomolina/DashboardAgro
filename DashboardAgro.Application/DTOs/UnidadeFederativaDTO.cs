namespace DashboardAgro.Application.DTOs
{
    public class UnidadeFederativaDTO
    {
        public int Id { get; set; }
        public string SiglaUF { get; set; }
        public string NomeUF { get; set; }
        public RegiaoDTO Regiao { get; set; }
    }
}

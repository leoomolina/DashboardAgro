namespace DashboardAgro.Infraestructure.Tables
{
    public abstract class TableBase
    {
        public int Id { get; set; }
        public DateTime ImportedDate { get; set; }
    }
}

using DashboardAgro.Infraestructure.Tables;
using Microsoft.EntityFrameworkCore;

namespace DashboardAgro.Infraestructure
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) :
            base(options)
        {
        }

        public DbSet<ControleImportacaoTable> ControleImportacaoTable { get; set; }
        public DbSet<DadosLavouraPermanenteTable> DadosLavouraPermanente { get; set; }
        public DbSet<DadosLavouraTemporariaTable> DadosLavouraTemporaria { get; set; }
        public DbSet<RegiaoTable> Municipio { get; set; }
        public DbSet<ProducaoTable> Producao { get; set; }
        public DbSet<UFTable> UnidadeFederativa { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ControleImportacaoTable>(table =>
            {
                table.ToTable("ControleImportacao");
                table.HasKey(e => e.Id );
            });

            modelBuilder.Entity<DadosLavouraPermanenteTable>(table =>
            {
                table.ToTable("DadosLavouraPermanente");
                table.HasKey(e => e.Id);
            });

            modelBuilder.Entity<DadosLavouraTemporariaTable>(table =>
            {
                table.ToTable("DadosLavouraTemporaria");
                table.HasKey(e => e.Id);
            });

            modelBuilder.Entity<ProducaoTable>(table =>
            {
                table.ToTable("Producao");
                table.HasKey(e => e.Id);
            });

            modelBuilder.Entity<UFTable>(table =>
            {
                table.ToTable("UnidadeFederativa");
                table.HasKey(e => e.Id);
            });
        }
    }
}

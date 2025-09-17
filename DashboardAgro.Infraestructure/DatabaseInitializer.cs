using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DashboardAgro.Infraestructure
{
    public static class DatabaseInitializer
    {
        public static void MigrateDatabase(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            try
            {
                db.Database.Migrate();
                Console.WriteLine("Banco de dados atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar o banco: {ex.Message}");
            }
        }
    }
}

using DashboardAgro.Application.UseCases;
using DashboardAgro.Infraestructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var services = new ServiceCollection();
        services.AddInfrastructureServices(configuration);

        var provider = services.BuildServiceProvider();

        DatabaseInitializer.MigrateDatabase(provider);

        var importador = provider.GetRequiredService<ImportarDadosBigQueryHandler>();

        if (args.Contains("--full"))
            await importador.ExecutarCargaInicial();
        else
            await importador.ExecutarCargaIncremental();
    }
}

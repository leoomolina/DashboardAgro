using DashboardAgro.Application.Handlers.Importacao;
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
        else if (args.Contains("--anoInicial"))
        {
            int index = Array.IndexOf(args, "--anoInicial");
            if (index >= 0 && index < args.Length - 1 && int.TryParse(args[index + 1], out int ano))
            {
                await importador.ExecutarCargaIncremental(ano);
            }
        }
        else
        {
            await importador.ExecutarCargaIncremental(2015);
        }

    }
}

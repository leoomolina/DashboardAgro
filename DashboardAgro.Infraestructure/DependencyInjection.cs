using DashboardAgro.Application.Contracts;
using DashboardAgro.Application.UseCases.Importacao;
using DashboardAgro.Domain.Contracts;
using DashboardAgro.Infraestructure.Repositories;
using DashboardAgro.Infraestructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DashboardAgro.Infraestructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // registra o DbContext com a connection string
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Default")));

            // registra os repositórios
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IBigQueryService>(sp =>
            {
                var projectId = configuration["BigQuery:ProjectId"];
                return new BigQueryService(projectId);
            });

            services.AddScoped<IImportarDados, ImportaDados>();
            services.AddScoped<ImportarDadosBigQueryHandler>();
            services.AddScoped<ILavouraPermanenteRepository, LavouraPermanenteQueryRepository>();

            return services;
        }
    }
}

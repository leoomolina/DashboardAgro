using DashboardAgro.Application.Handlers;
using DashboardAgro.Infraestructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev",
        policy => policy
            .WithOrigins("http://127.0.0.1:60740") // ou "http://localhost:4200" se usar ng serve
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Adiciona Controllers e OpenAPI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GetResumoByAnoAsyncHandler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetResumoAnualByEstadoAsyncHandler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(ListarLavourasTemporariasHandler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(ListarRegioesHandler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(ListarAnosDisponiveisAsyncHandler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(ListarProducoesAsyncHandler).Assembly);
});

// Faz o Kestrel escutar todas as interfaces
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});

builder.Services.AddInfrastructureServices(builder.Configuration);


var app = builder.Build();

DatabaseInitializer.MigrateDatabase(app.Services);

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("./swagger/v1/swagger.json", "API Agro V1");
    c.RoutePrefix = string.Empty;
});
app.UseCors("AllowAngularDev");

// NÃO usa HTTPS para testes no Docker
// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
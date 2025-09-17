using DashboardAgro.Infraestructure;

var builder = WebApplication.CreateBuilder(args);

// Adiciona Controllers e OpenAPI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// NÃO usa HTTPS para testes no Docker
// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
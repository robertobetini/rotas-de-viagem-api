using Application.Services;
using Domain.Repositories.Interfaces;
using Domain.Services.Interfaces;
using Infrastructure.Repositories.Strategies;

var builder = WebApplication.CreateBuilder(args);

Configure(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static void Configure(IServiceCollection services)
{
    services.AddControllers();

    services
        .AddEndpointsApiExplorer()
        .AddSwaggerGen()
        .AddSingleton<IRoutesService, RoutesService>()
        .AddSingleton<IRoutesRepository, MemoryRoutesRepository>();
}

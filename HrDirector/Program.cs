using HrDirector.DataBase;
using HrDirector.Repository;
using HrDirector.Service;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<HackathonContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), h =>
        {
            h.Username(builder.Configuration["MessageBroker:Username"]);
            h.Password(builder.Configuration["MessageBroker:Password"]);
        });
        configurator.ConfigureEndpoints(context);
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddScoped<StartupNotifierService>();
builder.Services.AddScoped<HrDirectorService>();

builder.Services.AddScoped<SatisfactionRepository>();
builder.Services.AddScoped<HackathonRepository>();
builder.Services.AddScoped<TeamRepository>();
builder.Services.AddScoped<WishlistRepository>();

builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

await app.UseStartupNotifier();

app.UseSwagger();
app.UseSwaggerUI();
app.ApplyMigrations();
app.UseRouting();

app.MapControllers();
app.MapGet("/", () => "Hello, World!");
app.Run();
using HrDirector.DataBase;
using HrDirector.MassTransit;
using HrDirector.Repository;
using HrDirector.Service;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddScoped<StartupNotifierService>();
builder.Services.AddScoped<IHrDirectorService, HrDirectorService>();

builder.Services.AddScoped<SatisfactionRepository>();
builder.Services.AddScoped<HackathonRepository>();
builder.Services.AddScoped<TeamRepository>();
builder.Services.AddScoped<WishlistRepository>();
builder.Services.AddSingleton<DataStore>();

builder.Configuration.AddEnvironmentVariables();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<HackathonContext>(options => options.UseNpgsql(connectionString).LogTo(Console.WriteLine, LogLevel.None));
builder.Logging.ClearProviders(); 

builder.Logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<WishlistConsumer>();

    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), h =>
        {
            h.Username(builder.Configuration["MessageBroker:Username"]);
            h.Password(builder.Configuration["MessageBroker:Password"]);
        });
        configurator.ReceiveEndpoint("get_wishlist_in_director_queue",
            e => { e.ConfigureConsumer<WishlistConsumer>(context); });

        configurator.ConfigureEndpoints(context);
    });
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.ApplyMigrations();
app.UseRouting();

app.MapControllers();

await app.UseStartupNotifier(app.Services);

app.Run();
using HrManager.MassTransit;
using HrManager.Service;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<ITeamBuildingStrategy, MarriageStrategy>();
builder.Services.AddSingleton<ITeamBuilder, TeamBuilder>();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<WishlistConsumer>();

    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), h =>
        {
            h.Username(builder.Configuration["MessageBroker:Username"]!);
            h.Password(builder.Configuration["MessageBroker:Password"]!);
        });
        configurator.ReceiveEndpoint("get_wishlist_queue",
            e => { e.ConfigureConsumer<WishlistConsumer>(context); });

        configurator.ConfigureEndpoints(context);
    });
});
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.MapControllers();

app.Run();
using Hackathon.DataBase;
using Hackathon.Repository;
using Hackathon.Service;
using Hackathon.Service.Strategy;
using Microsoft.EntityFrameworkCore;

namespace Hackathon;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<HackathonContext>(options => options.UseNpgsql(connectionString));

        builder.Services.AddControllers();
        builder.Services.AddHttpClient();

        builder.Services.AddHostedService<HackathonWorker>();
        builder.Services.AddTransient<Service.Hackathon>();
        builder.Services.AddTransient<ITeamBuildingStrategy, MarriageStrategy>();
        builder.Services.AddTransient<HrManager>();
        builder.Services.AddTransient<HrDirector>();

        builder.Services.AddScoped<EmployeeRepository>();
        builder.Services.AddScoped<HackathonRepository>();
        builder.Services.AddScoped<WishlistRepository>();
        builder.Services.AddScoped<SatisfactionRepository>();
        builder.Services.AddScoped<TeamRepository>();

        var app = builder.Build();
        
        
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        
        app.Run();
    }
}
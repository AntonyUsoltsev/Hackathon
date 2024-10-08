using Hackathon.DataBase;
using Hackathon.Repository;
using Hackathon.Service;
using Hackathon.Service.Strategy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hackathon;

internal class Program
{
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                string? connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<HackathonContext>(options => options.UseNpgsql(connectionString));
                services.AddHostedService<HackathonWorker>();
                services.AddTransient<Service.Hackathon>();
                services.AddTransient<ITeamBuildingStrategy, MarriageStrategy>();
                services.AddTransient<HrManager>();
                services.AddTransient<HrDirector>();
                
                services.AddTransient<EmployeeRepository>();
                services.AddTransient<HackathonRepository>();
                services.AddTransient<WishlistRepository>();
                services.AddTransient<SatisfactionRepository>();
                services.AddTransient<TeamRepository>();
            })
            .Build();

        host.Run();
    }
}
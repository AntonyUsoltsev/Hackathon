using Hackathon.Service;
using Hackathon.Service.Strategy;
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
                services.AddHostedService<HackathonWorker>();
                services.AddTransient<Service.Hackathon>();
                services.AddTransient<ITeamBuildingStrategy, MarriageStrategy>();
                services.AddTransient<HrManager>();
                services.AddTransient<HrDirector>();
            })
            .Build();

        host.Run();
    }
}
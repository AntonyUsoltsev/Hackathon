using Hackathon.Model;
using Microsoft.Extensions.Hosting;

namespace Hackathon.Service;

public class HackathonWorker : IHostedService
{
    private readonly Hackathon _hackathon;
    private static int _repeatNumber = 1000;

    public HackathonWorker(Hackathon hackathon)
    {
        _hackathon = hackathon;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        List<Employee> teamLeads = CsvReader.ReadCsv("Resources/Teamleads20.csv");
        List<Employee> juniors = CsvReader.ReadCsv("Resources/Juniors20.csv");
        double maxAccuracy = double.MinValue;
        double totalAccuracy = 0;

        for (int i = 0; i < _repeatNumber; i++)
        {
            List<Wishlist> teamLeadsWishlists = WishlistService.BuildWishlist(teamLeads, juniors);
            List<Wishlist> juniorsWishlists = WishlistService.BuildWishlist(juniors, teamLeads);

            double accuracy = _hackathon.Conduct(teamLeads, juniors, teamLeadsWishlists, juniorsWishlists);

            Console.WriteLine($"Accuracy: {accuracy} on iteration {i}");
            maxAccuracy = Math.Max(maxAccuracy, accuracy);
            totalAccuracy += accuracy;
        }

        Console.WriteLine($"Max accuracy: {maxAccuracy}");
        Console.WriteLine($"Average accuracy: {totalAccuracy / _repeatNumber}");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
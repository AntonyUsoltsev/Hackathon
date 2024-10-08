using Hackathon.Model;
using Hackathon.Repository;
using Microsoft.Extensions.Hosting;

namespace Hackathon.Service;

public class HackathonWorker(Hackathon hackathon, EmployeeRepository employeeRepository) : IHostedService
{
    private static int _repeatNumber = 1000;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        List<Employee> teamLeads = employeeRepository.GetEmployeesByRole("teamLead").ToList();
        List<Employee> juniors = employeeRepository.GetEmployeesByRole("junior").ToList();
        double maxAccuracy = double.MinValue;
        double totalAccuracy = 0;

        for (int i = 0; i < _repeatNumber; i++)
        {
            List<Wishlist> teamLeadsWishlists = WishlistService.BuildWishlist(teamLeads, juniors);
            List<Wishlist> juniorsWishlists = WishlistService.BuildWishlist(juniors, teamLeads);

            double accuracy = hackathon.Conduct(teamLeads, juniors, teamLeadsWishlists, juniorsWishlists);

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
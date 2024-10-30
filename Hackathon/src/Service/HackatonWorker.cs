using Hackathon.Model;
using Hackathon.Repository;
using Microsoft.Extensions.Hosting;

namespace Hackathon.Service;

public class HackathonWorker(
    Hackathon hackathon,
    EmployeeRepository employeeRepository,
    HackathonRepository hackathonRepository,
    WishlistRepository wishlistRepository) : IHostedService
{
    private static int _repeatNumber = 1;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        List<Employee> teamLeads = employeeRepository.GetEmployeesByRole("teamLead").ToList();
        List<Employee> juniors = employeeRepository.GetEmployeesByRole("junior").ToList();
        double maxAccuracy = double.MinValue;
        double totalAccuracy = 0;

        for (int i = 0; i < _repeatNumber; i++)
        {
            var hackathonDto = hackathonRepository.CreateEmptyHackathon();

            List<Wishlist> teamLeadsWishlists = WishlistService.BuildWishlist(teamLeads, juniors);
            List<Wishlist> juniorsWishlists = WishlistService.BuildWishlist(juniors, teamLeads);
            juniorsWishlists.ForEach(wl => wishlistRepository.SaveWishlist(wl, hackathonDto.Id));
            teamLeadsWishlists.ForEach(wl => wishlistRepository.SaveWishlist(wl, hackathonDto.Id));

            double accuracy = hackathon.Conduct(teamLeads, juniors, teamLeadsWishlists, juniorsWishlists, hackathonDto.Id);

            hackathonRepository.UpdateHackathonResult(hackathonDto.Id, accuracy);

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
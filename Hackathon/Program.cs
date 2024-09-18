using Hackathon.Model;
using Hackathon.Strategy;

namespace Hackathon
{
    internal class Program
    {
        public static int repeatNumber = 1000;

        public static void Main(string[] args)
        {
            List<Employee> teamleads = CsvReader.ReadCsv("Resources/Teamleads20.csv");
            List<Employee> juniors = CsvReader.ReadCsv("Resources/Juniors20.csv");
            double maxAccuracy = double.MinValue;
            double totalAccuracy = 0;
            ITeamBuildingStrategy teamBuildingStrategy = new MarriageStrategy();
            for (int i = 0; i < repeatNumber; i++)
            {
                List<Wishlist> teamleadsWishlists = WishlistService.BuildWishlist(teamleads, juniors);
                List<Wishlist> juniorsWishlists = WishlistService.BuildWishlist(juniors, teamleads);

                IEnumerable<Team> teams = teamBuildingStrategy.BuildTeams(teamleads, juniors, teamleadsWishlists, juniorsWishlists);

                double accuracy = HarmonicMeanCount.CountSatisfaction(teams, teamleadsWishlists, juniorsWishlists);
                Console.WriteLine($"Accuracy: {accuracy} on iter {i}");
                maxAccuracy = Math.Max(maxAccuracy, accuracy);
                totalAccuracy += accuracy;
            }

            Console.WriteLine($"Max accuracy: {maxAccuracy}");
            Console.WriteLine($"Average accuracy: {totalAccuracy / repeatNumber}");
        }

        public static void PrintWishlist(List<Wishlist> wishlists)
        {
            foreach (Wishlist wishlist in wishlists)
            {
                Console.Write($"ParticipantId: {wishlist.EmployeeId}. Chosen: ");
                foreach (var chosen in wishlist.DesiredEmployees)
                {
                    Console.Write($"{chosen} ");
                }

                Console.WriteLine();
            }
        }

        public static void PrintTeams(List<Team> teams)
        {
            foreach (Team team in teams)
            {
                Console.WriteLine($"Teamlead: {team.TeamLead.Id} - Junior {team.Junior.Id}: ");
            }
        }
    }
}
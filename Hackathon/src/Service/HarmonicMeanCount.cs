using Hackathon.Model;

namespace Hackathon
{
    public class HarmonicMeanCount
    {
        public static double CountSatisfaction(
            IEnumerable<Team> teams,
            IEnumerable<Wishlist> teamLeadsWishlists,
            IEnumerable<Wishlist> juniorsWishlists)
        {
            int participantCount = teams.Count();
            List<int> allSatisfactions = new List<int>();
            foreach (var team in teams)
            {
                var teamLeadWishlist = teamLeadsWishlists.First(tl => tl.EmployeeId == team.TeamLead.Id);
                var teamLeadSat = participantCount - Array.IndexOf(teamLeadWishlist.DesiredEmployees, team.Junior.Id);
                allSatisfactions.Add(teamLeadSat);
                var juniorWishList = juniorsWishlists.First(jl => jl.EmployeeId == team.Junior.Id);
                var juniorSat = participantCount - Array.IndexOf(juniorWishList.DesiredEmployees, team.TeamLead.Id);
                allSatisfactions.Add(juniorSat);
            }

            return CountHarmonicMean(allSatisfactions);
        }

        private static double CountHarmonicMean(List<int> numbers)
        {
            double inverseSum = 0;
            Console.WriteLine(String.Join(", ", numbers));
            foreach (var number in numbers)
            {
                inverseSum += 1d / number;
            }

            return numbers.Count / inverseSum;
        }
    }
}
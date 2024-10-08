using Hackathon.Model;
using Hackathon.Repository;

namespace Hackathon.Service;

public class HrDirector(SatisfactionRepository satisfactionRepository)
{
    public double CalculateHarmony(IEnumerable<Team> teams,
        IEnumerable<Wishlist> teamLeadsWishlists,
        IEnumerable<Wishlist> juniorsWishlists,
        int hackathonId)
    {
        int participantCount = teams.Count();
        var allSatisfactions = new List<int>();
        foreach (var team in teams)
        {
            var teamLeadWishlist = teamLeadsWishlists.First(tl => tl.EmployeeId == team.TeamLead.Id);
            int teamLeadSat = participantCount - Array.IndexOf(teamLeadWishlist.DesiredEmployees, team.Junior.Id);
            allSatisfactions.Add(teamLeadSat);
            satisfactionRepository.SaveSatisfaction(hackathonId, team.TeamLead.Id, teamLeadSat);

            var juniorWishList = juniorsWishlists.First(jl => jl.EmployeeId == team.Junior.Id);
            int juniorSat = participantCount - Array.IndexOf(juniorWishList.DesiredEmployees, team.TeamLead.Id);
            allSatisfactions.Add(juniorSat);
            satisfactionRepository.SaveSatisfaction(hackathonId, team.Junior.Id, juniorSat);
        }

        return HarmonicMeanCount.CountHarmonicMean(allSatisfactions);
    }
}
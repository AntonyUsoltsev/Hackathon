using Hackathon.Model;
using Hackathon.Strategy;

namespace Hackathon.Service.Strategy;

public class GreedyStrategy : ITeamBuildingStrategy
{
    public IEnumerable<Team> BuildTeams(
        IEnumerable<Employee> teamLeads,
        IEnumerable<Employee> juniors,
        IEnumerable<Wishlist> teamLeadsWishlists,
        IEnumerable<Wishlist> juniorsWishlists)
    {
        var teams = new List<Team>();

        foreach (var teamLeadsWishlist in teamLeadsWishlists)
        {
            var teamLead = teamLeads.First(tl => tl.Id == teamLeadsWishlist.EmployeeId);
            foreach (var desiredEmployee in teamLeadsWishlist.DesiredEmployees)
            {
                if (!teams.Any(t => t.Junior.Id == desiredEmployee))
                {
                    var junior = juniors.First(j => j.Id == desiredEmployee);
                    teams.Add(new Team(teamLead, junior));
                    break;
                }
            }
        }

        return teams;
    }
}
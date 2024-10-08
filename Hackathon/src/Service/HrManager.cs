using Hackathon.Model;
using Hackathon.Service.Strategy;

namespace Hackathon.Service;

public class HrManager(ITeamBuildingStrategy strategy)
{
    public virtual IEnumerable<Team> FormTeams(
        IEnumerable<Employee> teamLeads,
        IEnumerable<Employee> juniors,
        IEnumerable<Wishlist> teamLeadsWishlists,
        IEnumerable<Wishlist> juniorsWishlists)
    {
        return strategy.BuildTeams(teamLeads, juniors, teamLeadsWishlists, juniorsWishlists);
    }
}
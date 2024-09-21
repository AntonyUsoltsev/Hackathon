using Hackathon.Model;
using Hackathon.Service.Strategy;

namespace Hackathon.Service;

public class HrManager
{
    private readonly ITeamBuildingStrategy _strategy;

    public HrManager(ITeamBuildingStrategy strategy)
    {
        _strategy = strategy;
    }

    public IEnumerable<Team> FormTeams(IEnumerable<Employee> teamLeads,
        IEnumerable<Employee> juniors,
        IEnumerable<Wishlist> teamLeadsWishlists,
        IEnumerable<Wishlist> juniorsWishlists)
    {
        return _strategy.BuildTeams(teamLeads, juniors, teamLeadsWishlists, juniorsWishlists);
    }
}
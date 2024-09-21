using Hackathon.Model;

namespace Hackathon.Service.Strategy;

public interface ITeamBuildingStrategy
{
    IEnumerable<Team> BuildTeams(IEnumerable<Employee> teamLeads, IEnumerable<Employee> juniors,
        IEnumerable<Wishlist> teamLeadsWishlists, IEnumerable<Wishlist> juniorsWishlists);
}
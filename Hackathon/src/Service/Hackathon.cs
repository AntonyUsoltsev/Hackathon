using Hackathon.Model;
using Hackathon.Repository;

namespace Hackathon.Service;

public class Hackathon(
    HrManager hrManager,
    HrDirector hrDirector,
    TeamRepository teamRepository)
{
    public double Conduct(IEnumerable<Employee> teamLeads,
        IEnumerable<Employee> juniors,
        IEnumerable<Wishlist> teamLeadsWishlists,
        IEnumerable<Wishlist> juniorsWishlists,
        int hackathonId)
    {
        List<Team> teams = hrManager.FormTeams(teamLeads, juniors, teamLeadsWishlists, juniorsWishlists).ToList();
        teams.ForEach(team => teamRepository.SaveTeam(team, hackathonId));

        double result_harmony = hrDirector.CalculateHarmony(teams, teamLeadsWishlists, juniorsWishlists, hackathonId);

        return result_harmony;
    }
}
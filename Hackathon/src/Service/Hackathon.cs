using Hackathon.Model;

namespace Hackathon.Service;

public class Hackathon
{
    private readonly HrManager _hrManager;
    private readonly HrDirector _hrDirector;

    public Hackathon(HrManager hrManager, HrDirector hrDirector)
    {
        _hrManager = hrManager;
        _hrDirector = hrDirector;
    }

    public double Conduct(IEnumerable<Employee> teamLeads,
        IEnumerable<Employee> juniors,
        IEnumerable<Wishlist> teamLeadsWishlists,
        IEnumerable<Wishlist> juniorsWishlists)
    {
        IEnumerable<Team> teams = _hrManager.FormTeams(teamLeads, juniors, teamLeadsWishlists, juniorsWishlists);
        return _hrDirector.CalculateHarmony(teams, teamLeadsWishlists, juniorsWishlists);
    }
}
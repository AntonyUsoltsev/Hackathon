using Hackathon.Model;

namespace Hackathon.Service;

public class HrDirector
{
    public double CalculateHarmony(IEnumerable<Team> teams,
        IEnumerable<Wishlist> teamLeadsWishlists,
        IEnumerable<Wishlist> juniorsWishlists)
    {
        return HarmonicMeanCount.CountSatisfaction(teams, teamLeadsWishlists, juniorsWishlists);
    }
}
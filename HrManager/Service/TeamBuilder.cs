using Hackathon.Model;
using HrManager.Util;

namespace HrManager.Service;

public class TeamBuilder(ITeamBuildingStrategy strategy) :ITeamBuilder
{
    private readonly IEnumerable<Employee> _teamLeads =
        (IEnumerable<Employee>?)CsvReader.ReadCsv("Resources/Teamleads5.csv") ??
        throw new InvalidOperationException();

    private readonly IEnumerable<Employee> _juniors =
        (IEnumerable<Employee>?)CsvReader.ReadCsv("Resources/Juniors5.csv") ??
        throw new InvalidOperationException();

    private readonly List<Wishlist> _teamLeadsWishlists = [];
    private readonly List<Wishlist> _juniorsWishlists = [];
    private IEnumerable<Team> FormedTeams { get; set; } = new List<Team>();
    
    public void SaveTeamLeadWishlist(Wishlist wishlist)
    {
        _teamLeadsWishlists.Add(wishlist);
        TryFormTeams();
    }

    public void SaveJuniorWishlist(Wishlist wishlist)
    {
        _juniorsWishlists.Add(wishlist);
        TryFormTeams();
    }

    private void TryFormTeams()
    {
        if (_teamLeadsWishlists.Count == _teamLeads.Count() &&
            _juniorsWishlists.Count == _juniors.Count())
        {
            FormedTeams = strategy.BuildTeams(_teamLeads, _juniors, _teamLeadsWishlists, _juniorsWishlists);
            // send teams to director
        }
    }

    private void SendTeamsToDirector()
    {
    }
}
using EmployeeService.Model;
using HrDirector.Model;

namespace HrDirector.Service;

public class DataStore
{
    private Dictionary<int, List<Wishlist>> _teamLeadsWishlists = new();
    private Dictionary<int, List<Wishlist>> _juniorsWishlists = new();
    private Dictionary<int, IEnumerable<Team>> _teams = new();

    public void AddTeamLeadWishlist(int hackathonId, Wishlist wishlist)
    {
        if (!_teamLeadsWishlists.ContainsKey(hackathonId))
        {
            _teamLeadsWishlists[hackathonId] = [];
        }
        // Console.WriteLine($"Saving team leads wishlist in hackathon {hackathonId}, wishlist {wishlist}");
        _teamLeadsWishlists[hackathonId].Add(wishlist);
    }

    public void AddJuniorWishlist(int hackathonId, Wishlist wishlist)
    {
        if (!_juniorsWishlists.ContainsKey(hackathonId))
        {
            _juniorsWishlists[hackathonId] = [];
        }
        // Console.WriteLine($"Saving juniors wishlist in hackathon {hackathonId}, wishlist {wishlist}");
        _juniorsWishlists[hackathonId].Add(wishlist);
    }

    public void SetTeams(int hackathonId, IEnumerable<Team> teams)
    {
        // Console.WriteLine($"Saving teams in hackathon {hackathonId}, teams {teams}");
        _teams[hackathonId] = teams;
    }

    public List<Wishlist> GetTeamLeadWishlists(int hackathonId)
    {
        if (_teamLeadsWishlists.TryGetValue(hackathonId, out var wishlists))
        {
            return wishlists;
        }

        return [];
    }

    public List<Wishlist> GetJuniorWishlists(int hackathonId)
    {
        if (_juniorsWishlists.TryGetValue(hackathonId, out var wishlists))
        {
            return wishlists;
        }

        return [];
    }

    public IEnumerable<Team> GetTeams(int hackathonId)
    {
        if (_teams.TryGetValue(hackathonId, out var teams))
        {
            return teams;
        }

        return Enumerable.Empty<Team>();
    }
}
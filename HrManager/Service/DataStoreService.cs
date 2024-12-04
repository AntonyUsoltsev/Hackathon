using EmployeeService.Model;

namespace HrManager.Service;

public class DataStore
{
    private Dictionary<int, List<Wishlist>> _teamLeadsWishlists = new();
    private Dictionary<int, List<Wishlist>> _juniorsWishlists = new();

    public void AddTeamLeadWishlist(int hackathonId, Wishlist wishlist)
    {
        if (!_teamLeadsWishlists.ContainsKey(hackathonId))
        {
            _teamLeadsWishlists[hackathonId] = [];
        }

        _teamLeadsWishlists[hackathonId].Add(wishlist);
    }

    public void AddJuniorWishlist(int hackathonId, Wishlist wishlist)
    {
        if (!_juniorsWishlists.ContainsKey(hackathonId))
        {
            _juniorsWishlists[hackathonId] = [];
        }

        _juniorsWishlists[hackathonId].Add(wishlist);
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
}
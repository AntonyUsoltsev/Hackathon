using HrDirector.DataBase.Dto;
using HrDirector.Model;

namespace HrDirector.Service;

public interface IHrDirectorService
{
    public double CalculateHarmony(IEnumerable<Team> teams,
        IEnumerable<Wishlist> teamLeadsWishlists,
        IEnumerable<Wishlist> juniorsWishlists,
        int hackathonId);

    HackathonEntity CreateEmptyHackathon();
    void SaveTeams(IEnumerable<Team> formedTeams , int hackathonId);

    void SaveWishlists(List<Wishlist> teamLeadsWishlists, List<Wishlist> juniorsWishlists, int hackathonId);
    void UpdateHackathonResult(double satisfaction, int hackathonId);
}
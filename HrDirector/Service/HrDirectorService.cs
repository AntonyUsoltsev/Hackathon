using HrDirector.DataBase.Dto;
using HrDirector.Model;
using HrDirector.Repository;

namespace HrDirector.Service;

public class HrDirectorService(
    SatisfactionRepository satisfactionRepository,
    HackathonRepository hackathonRepository,
    TeamRepository teamRepository,
    WishlistRepository wishlistRepository) : IHrDirectorService
{
    public double CalculateHarmony(IEnumerable<Team> teams,
        IEnumerable<Wishlist> teamLeadsWishlists,
        IEnumerable<Wishlist> juniorsWishlists,
        int hackathonId)
    {
        int participantCount = teams.Count();
        var allSatisfactions = new List<int>();
        foreach (var team in teams)
        {
            var teamLeadWishlist = teamLeadsWishlists.First(tl => tl.EmployeeId == team.TeamLead.Id);
            int teamLeadSat = participantCount - Array.IndexOf(teamLeadWishlist.DesiredEmployees, team.Junior.Id);
            allSatisfactions.Add(teamLeadSat);
            satisfactionRepository.SaveSatisfaction(hackathonId, team.TeamLead.Id, teamLeadSat);

            var juniorWishList = juniorsWishlists.First(jl => jl.EmployeeId == team.Junior.Id);
            int juniorSat = participantCount - Array.IndexOf(juniorWishList.DesiredEmployees, team.TeamLead.Id);
            allSatisfactions.Add(juniorSat);
            satisfactionRepository.SaveSatisfaction(hackathonId, team.Junior.Id, juniorSat);
        }

        return HarmonicMeanCount.CountHarmonicMean(allSatisfactions);
    }

    public HackathonEntity CreateEmptyHackathon()
    {
        return hackathonRepository.CreateEmptyHackathon();
    }

    public void SaveTeams(IEnumerable<Team> formedTeams, int hackathonId)
    {
        formedTeams.ToList().ForEach(team => teamRepository.SaveTeam(team, hackathonId));
    }

    public void SaveWishlists(List<Wishlist> teamLeadsWishlists, List<Wishlist> juniorsWishlists, int hackathonId)
    {
        juniorsWishlists.ForEach(wl => wishlistRepository.SaveWishlist(wl, hackathonId));
        teamLeadsWishlists.ForEach(wl => wishlistRepository.SaveWishlist(wl, hackathonId));
    }

    public void UpdateHackathonResult(double satisfaction, int hackathonId)
    {
        hackathonRepository.UpdateHackathonResult(hackathonId, satisfaction);
    }
}
using EmployeeService.Model;
using HrDirector.DataBase.Dto;
using HrDirector.Model;
using HrDirector.Repository;

namespace HrDirector.Service;

public class HrDirectorService(
    SatisfactionRepository satisfactionRepository,
    HackathonRepository hackathonRepository,
    TeamRepository teamRepository,
    WishlistRepository wishlistRepository)
{
    private List<Wishlist> _teamLeadsWishlists = [];
    private List<Wishlist> _juniorsWishlists = [];
    private IEnumerable<Team> _teams = [];
    private readonly object _lock = new object();


    public void SaveTeamleadWishlist(DTO dto)
    {
        lock (_lock)
        {
            _teamLeadsWishlists.Add(dto.Wishlist);
            TryCalculateHarmony(dto.hackathonId);
        }
    }

    public void SaveJuniorWishlist(DTO dto)
    {
        lock (_lock)
        {
            _juniorsWishlists.Add(dto.Wishlist);
            TryCalculateHarmony(dto.hackathonId);
        }
    }

    public void SaveTeams(IEnumerable<Team> teams, int hackathonId)
    {
        lock (_lock)
        {
            _teams = teams;
            TryCalculateHarmony(hackathonId);
        }
    }

    private void TryCalculateHarmony(int hackathonId)
    {
        Console.WriteLine($"{_teams.Count()},{_teamLeadsWishlists.Count}, {_juniorsWishlists.Count} ");
        if (_teams.Count() != 0)
        {
            SaveTeamsInDb(_teams, hackathonId);
            SaveWishlistsInDb(_teamLeadsWishlists, _juniorsWishlists, hackathonId);
            Console.WriteLine("Start calculating harmony");
            double satisfaction = CalculateHarmony(_teams, _teamLeadsWishlists, _juniorsWishlists, hackathonId);
            UpdateHackathonResult(satisfaction, hackathonId);
            Console.WriteLine(satisfaction);
        }
    }

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

    public void SaveTeamsInDb(IEnumerable<Team> formedTeams, int hackathonId)
    {
        formedTeams.ToList().ForEach(team => teamRepository.SaveTeam(team, hackathonId));
    }

    public void SaveWishlistsInDb(List<Wishlist> teamLeadsWishlists, List<Wishlist> juniorsWishlists, int hackathonId)
    {
        juniorsWishlists.ForEach(wl => wishlistRepository.SaveWishlist(wl, hackathonId));
        teamLeadsWishlists.ForEach(wl => wishlistRepository.SaveWishlist(wl, hackathonId));
    }

    public void UpdateHackathonResult(double satisfaction, int hackathonId)
    {
        hackathonRepository.UpdateHackathonResult(hackathonId, satisfaction);
    }
}
using EmployeeService.Model;
using HrDirector.DataBase.Dto;
using HrDirector.Model;
using HrDirector.Repository;
using HrManager.Model;

namespace HrDirector.Service;

public class HrDirectorService(
    SatisfactionRepository satisfactionRepository,
    HackathonRepository hackathonRepository,
    TeamRepository teamRepository,
    WishlistRepository wishlistRepository,
    DataStore dataStore) : IHrDirectorService
{
    private readonly object _lock = new object();
    
    public void SaveTeamLeadWishlist(DTO dto)
    {
        lock (_lock)
        {
            dataStore.AddTeamLeadWishlist(dto.hackathonId, dto.Wishlist);
            TryCalculateHarmony(dto.hackathonId);
        }
    }

    public void SaveJuniorWishlist(DTO dto)
    {
        lock (_lock)
        {
            dataStore.AddJuniorWishlist(dto.hackathonId, dto.Wishlist);
            TryCalculateHarmony(dto.hackathonId);
        }
    }

    public void SaveTeams(IEnumerable<Team> teams, int hackathonId)
    {
        lock (_lock)
        {
            dataStore.SetTeams(hackathonId, teams);
            TryCalculateHarmony(hackathonId);
        }
    }

    private void TryCalculateHarmony(int hackathonId)
    {
        List<Wishlist> teamLeadsWishlists = dataStore.GetTeamLeadWishlists(hackathonId);
        List<Wishlist> juniorWishlists = dataStore.GetJuniorWishlists(hackathonId);
        IEnumerable<Team> teams = dataStore.GetTeams(hackathonId);
        // Console.WriteLine($"Current team leads: {teamLeadsWishlists.Count}, current juniors: {juniorWishlists.Count}, currentTeams: {teams.Count()}");
        if (teams.Count() != 0 &&
            juniorWishlists.Count == teams.Count() &&
            teamLeadsWishlists.Count == teams.Count())
        {
            SaveTeamsInDb(teams, hackathonId);
            SaveWishlistsInDb(teamLeadsWishlists, juniorWishlists, hackathonId);
            // Console.WriteLine("Start calculating harmony");
            double satisfaction = CalculateHarmony(teams, teamLeadsWishlists, juniorWishlists, hackathonId);
            UpdateHackathonResult(satisfaction, hackathonId);
            Console.WriteLine($"Satisfaction on hackathon id {hackathonId} is: {satisfaction}");
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
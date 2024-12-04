using EmployeeService.Model;
using HrManager.Model;
using HrManager.Util;

namespace HrManager.Service;

public class TeamBuilder(ITeamBuildingStrategy strategy, IHttpClientFactory clientFactory, DataStore dataStore)
    : ITeamBuilder
{
    private readonly IEnumerable<Employee> _teamLeads =
        (IEnumerable<Employee>?)CsvReader.ReadCsv("Resources/Teamleads5.csv") ??
        throw new InvalidOperationException();

    private readonly IEnumerable<Employee> _juniors =
        (IEnumerable<Employee>?)CsvReader.ReadCsv("Resources/Juniors5.csv") ??
        throw new InvalidOperationException();

    private readonly string _hrDirectorUrl = Environment.GetEnvironmentVariable("HR_DIRECTOR_URL") ??
                                             throw new InvalidOperationException("Invalid hr director url.");

    private readonly object _lock = new object();


    public void SaveTeamLeadWishlist(DTO dto)
    {
        lock (_lock)
        {
            dataStore.AddTeamLeadWishlist(dto.hackathonId, dto.Wishlist);
            TryFormTeams(dto.hackathonId);
        }
    }

    public void SaveJuniorWishlist(DTO dto)
    {
        lock (_lock)
        {
            dataStore.AddJuniorWishlist(dto.hackathonId, dto.Wishlist);
            TryFormTeams(dto.hackathonId);
        }
    }

    private void TryFormTeams(int hackathonId)
    {
        List<Wishlist> teamLeadsWishlists = dataStore.GetTeamLeadWishlists(hackathonId);
        List<Wishlist> juniorsWishlists = dataStore.GetJuniorWishlists(hackathonId);
        Console.WriteLine(
            $"Current team leads: {teamLeadsWishlists.Count}, current juniors: {juniorsWishlists.Count}, allTeamLeads: {_teamLeads.Count()}, allJuniors: {_juniors.Count()}");
        if (teamLeadsWishlists.Count == _teamLeads.Count() &&
            juniorsWishlists.Count == _juniors.Count())
        {
            Console.WriteLine("Start building teams");
            IEnumerable<Team> formedTeams =
                strategy.BuildTeams(_teamLeads, _juniors, teamLeadsWishlists, juniorsWishlists);
            SendTeamsToDirector(formedTeams, hackathonId);
        }
    }

    private async void SendTeamsToDirector(IEnumerable<Team> formedTeams, int hackathonId)
    {
        using var client = clientFactory.CreateClient();
        var allDataDto = new TeamsMessage(formedTeams, hackathonId);

        Console.WriteLine($"Send data to HR director:{JsonContent.Create(allDataDto).Value}");

        var response = await client.PostAsJsonAsync(_hrDirectorUrl, allDataDto);

        Console.WriteLine(response.IsSuccessStatusCode
            ? "Wishlist and teams sent successfully to HR Director."
            : $"Failed to send wishlist. Status Code: {response.StatusCode}, {response.ReasonPhrase}");
    }
}
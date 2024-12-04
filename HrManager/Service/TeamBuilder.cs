using Contract;
using HrManager.Util;

namespace HrManager.Service;

public class TeamBuilder(ITeamBuildingStrategy strategy, IHttpClientFactory clientFactory) : ITeamBuilder
{
    private readonly IEnumerable<Employee> _teamLeads =
        (IEnumerable<Employee>?)CsvReader.ReadCsv("Resources/Teamleads5.csv") ??
        throw new InvalidOperationException();

    private readonly IEnumerable<Employee> _juniors =
        (IEnumerable<Employee>?)CsvReader.ReadCsv("Resources/Juniors5.csv") ??
        throw new InvalidOperationException();

    private readonly string _hrDirectorUrl = Environment.GetEnvironmentVariable("HR_DIRECTOR_URL") ??
                                             throw new InvalidOperationException("Invalid hr director url.");

    private readonly List<Wishlist> _teamLeadsWishlists = [];
    private readonly List<Wishlist> _juniorsWishlists = [];
    private readonly object _lock = new object();


    public void SaveTeamLeadWishlist(WishlistMessage dto)
    {
        lock (_lock)
        {
            _teamLeadsWishlists.Add(dto.Wishlist);
            TryFormTeams(dto.hackathonId);
        }
    }

    public void SaveJuniorWishlist(WishlistMessage dto)
    {
        lock (_lock)
        {
            _juniorsWishlists.Add(dto.Wishlist);
            TryFormTeams(dto.hackathonId);
        }
    }

    private void TryFormTeams(int hackathonId)
    {
        Console.WriteLine(
            $"Current team leads: {_teamLeadsWishlists.Count}, current juniors: {_juniorsWishlists.Count}, allTeamLeads: {_teamLeads.Count()}, allJuniors: {_juniors.Count()}");
        if (_teamLeadsWishlists.Count == _teamLeads.Count() &&
            _juniorsWishlists.Count == _juniors.Count())
        {
            Console.WriteLine("Start building teams");
            IEnumerable<Team> formedTeams =
                strategy.BuildTeams(_teamLeads, _juniors, _teamLeadsWishlists, _juniorsWishlists);
            SendTeamsToDirector(formedTeams, hackathonId);
        }
    }

    private async void SendTeamsToDirector(IEnumerable<Team> formedTeams, int hackathonId)
    {
        using var client = clientFactory.CreateClient();
        var teamsMessage = new TeamsMessage(formedTeams, hackathonId);

        Console.WriteLine($"Send data to HR director:{JsonContent.Create(teamsMessage).Value}");

        var response = await client.PostAsJsonAsync(_hrDirectorUrl, teamsMessage);

        Console.WriteLine(response.IsSuccessStatusCode
            ? "Wishlist and teams sent successfully to HR Director."
            : $"Failed to send wishlist. Status Code: {response.StatusCode}, {response.ReasonPhrase}");
    }
}
using HrManager.Model;
using HrManager.Util;

namespace HrManager.Service;

public class TeamBuilder(ITeamBuildingStrategy strategy, IHttpClientFactory clientFactory) :ITeamBuilder
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
            Console.WriteLine("Start building teams");
            IEnumerable<Team> formedTeams = strategy.BuildTeams(_teamLeads, _juniors, _teamLeadsWishlists, _juniorsWishlists);
            SendTeamsToDirector(formedTeams);
        }
    }

    private async void SendTeamsToDirector(IEnumerable<Team> formedTeams)
    {
        using var client = clientFactory.CreateClient();
        var allDataDto = new AllDataDto(_teamLeadsWishlists, _juniorsWishlists, formedTeams);
        
        Console.WriteLine($"Send data to HR director:{JsonContent.Create(allDataDto).Value}");
        
        var response = await client.PostAsJsonAsync(_hrDirectorUrl, allDataDto);

        Console.WriteLine(response.IsSuccessStatusCode
            ? "Wishlist and teams sent successfully to HR Director."
            : $"Failed to send wishlist. Status Code: {response.StatusCode}");
    }
}
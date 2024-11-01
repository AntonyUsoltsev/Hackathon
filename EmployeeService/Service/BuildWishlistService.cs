using EmployeeService.Model;
using EmployeeService.Util;

namespace EmployeeService.Service;

public class BuildWishlistService : IHostedService
{
    private readonly Role _employeeType;
    private readonly int _employeeId;

    private readonly List<Employee> _juniors;
    private readonly List<Employee> _teamLeads;

    private readonly IHttpClientFactory _clientFactory;
    private readonly string _hrManagerUrl;

    public BuildWishlistService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _employeeType = Enum.TryParse(Environment.GetEnvironmentVariable("EMPLOYEE_TYPE"), true, out Role role)
            ? role
            : throw new InvalidOperationException("Invalid participant type.");

        _employeeId = int.TryParse(Environment.GetEnvironmentVariable("EMPLOYEE_ID"), out int id)
            ? id
            : throw new InvalidOperationException("Invalid participant id.");

        _teamLeads = CsvReader.ReadCsv("Resources/Teamleads5.csv");
        _juniors = CsvReader.ReadCsv("Resources/Juniors5.csv");

        string hrManagerTemplateUrl = Environment.GetEnvironmentVariable("HR_MANAGER_URL") ??
                                      throw new InvalidOperationException("Invalid hr manager url.");
        _hrManagerUrl = hrManagerTemplateUrl.Replace("{EMPLOYEE_TYPE}", _employeeType.ToString().ToLower());


        Console.WriteLine($"Employee type: {_employeeType}");
        Console.WriteLine($"Employee id: {_employeeId}");
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var buildWishlist = _employeeType switch
            {
                Role.Junior => BuildWishlist(_juniors, _teamLeads),
                Role.TeamLead => BuildWishlist(_teamLeads, _juniors),
                _ => throw new InvalidOperationException("Invalid participant type.")
            };

            await SendWishlistToHrManager(buildWishlist, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error building or sending wishlist: {ex.Message}");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("BuildWishlistService is stopping.");
        return Task.CompletedTask;
    }

    private Wishlist BuildWishlist(List<Employee> selectors, List<Employee> chooseables)
    {
        var currentEmployee = selectors.FirstOrDefault(e => e.Id == _employeeId) ??
                              throw new InvalidOperationException("Employee with the specified ID not found.");

        return WishListBuilder.BuildWishlist(currentEmployee, chooseables);
    }

    private async Task SendWishlistToHrManager(Wishlist wishlist, CancellationToken cancellationToken)
    {
        using var client = _clientFactory.CreateClient();

        var response = await client.PostAsJsonAsync(_hrManagerUrl, wishlist, cancellationToken);

        Console.WriteLine(response.IsSuccessStatusCode
            ? "Wishlist sent successfully to HR Manager."
            : $"Failed to send wishlist. Status Code: {response.StatusCode}");
    }
}
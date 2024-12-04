using Contract;
using EmployeeService.Util;
using MassTransit;

namespace EmployeeService.Service;

public class BuildWishlistService
{
    private readonly Role _employeeType;
    private readonly int _employeeId;

    private readonly List<Employee> _teamLeads = CsvReader.ReadCsv("Resources/Teamleads5.csv") ??
                                                 throw new InvalidOperationException();

    private readonly List<Employee> _juniors = CsvReader.ReadCsv("Resources/Juniors5.csv") ??
                                               throw new InvalidOperationException();

    private readonly IPublishEndpoint _publishEndpoint;

    public BuildWishlistService(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
        _employeeType = Enum.TryParse(Environment.GetEnvironmentVariable("EMPLOYEE_TYPE"), true, out Role role)
            ? role
            : throw new InvalidOperationException("Invalid participant type.");

        _employeeId = int.TryParse(Environment.GetEnvironmentVariable("EMPLOYEE_ID"), out int id)
            ? id
            : throw new InvalidOperationException("Invalid participant id.");

        string hrManagerTemplateUrl = Environment.GetEnvironmentVariable("HR_MANAGER_URL") ??
                                      throw new InvalidOperationException("Invalid hr manager url.");

        Console.WriteLine($"Employee type: {_employeeType}");
        Console.WriteLine($"Employee id: {_employeeId}");
    }

    public async void StartHackathon(int hackathonId)
    {
        try
        {
            var buildWishlist = _employeeType switch
            {
                Role.Junior => BuildWishlist(_juniors, _teamLeads),
                Role.TeamLead => BuildWishlist(_teamLeads, _juniors),
                _ => throw new InvalidOperationException("Invalid participant type.")
            };

            await SendWishlistToHrManager(buildWishlist, hackathonId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error building or sending wishlist: {ex.Message}");
        }
    }


    private Wishlist BuildWishlist(List<Employee> selectors, List<Employee> chooseables)
    {
        var currentEmployee = selectors.FirstOrDefault(e => e.Id == _employeeId) ??
                              throw new InvalidOperationException("Employee with the specified ID not found.");

        return WishListBuilder.BuildWishlist(currentEmployee, chooseables);
    }

    private async Task SendWishlistToHrManager(Wishlist wishlist, int hackathonId)
    {
        WishlistMessage dto = new WishlistMessage(wishlist, _employeeType, hackathonId);
        Console.WriteLine($"Sending wishlist to HR manager by RabbitMQ: {JsonContent.Create(dto).Value}");

        try
        {
            await _publishEndpoint.Publish(dto, CancellationToken.None);
            Console.WriteLine("Wishlist successfully sent to HR manager by RabbitMQ.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send wishlist to HR manager by RabbitMQ: {ex.Message}");
        }
    }
}
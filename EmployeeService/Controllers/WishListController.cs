using EmployeeService.Model;
using EmployeeService.Util;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeService.Controllers;

[Route("api/wishlists")]
[ApiController]
public class PreferencesController : ControllerBase
{
    private readonly Role _employeeType;
    private readonly int _employeeId;

    private readonly List<Employee> _juniors;
    private readonly List<Employee> _teamLeads;

    public PreferencesController()
    {
        _employeeType = Enum.TryParse(Environment.GetEnvironmentVariable("EMPLOYEE_TYPE"), true, out Role role)
            ? role
            : Role.Unknown;

        _employeeId = int.TryParse(Environment.GetEnvironmentVariable("EMPLOYEE_ID") ?? "0", out int id)
            ? id
            : 0;
        _teamLeads = CsvReader.ReadCsv("Resources/Teamleads5.csv");
        _juniors = CsvReader.ReadCsv("Resources/Juniors5.csv");

        Console.WriteLine($"Employee type: {_employeeType}");
        Console.WriteLine($"Employee id: {_employeeId}");
    }

    [HttpGet]
    public IActionResult GetPreferences()
    {
        Console.WriteLine("GetPreferences method called");

        try
        {
            return _employeeType switch
            {
                Role.Junior => Ok(BuildWishlist(_juniors, _teamLeads)),
                Role.TeamLead => Ok(BuildWishlist(_teamLeads, _juniors)),
                _ => BadRequest("Invalid participant type.")
            };
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    private Wishlist BuildWishlist(List<Employee> selectors, List<Employee> chooseables)
    {
        var currentEmployee = selectors.FirstOrDefault(e => e.Id == _employeeId) ??
                              throw new InvalidOperationException("Employee with the specified ID not found.");

        return WishListBuilder.BuildWishlist(currentEmployee, chooseables);
    }
}
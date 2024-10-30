using EmployeeService.Model;
using EmployeeService.Util;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeService.Controllers;

[Route("api/wishlists")]
[ApiController]
public class PreferencesController : ControllerBase
{
    private readonly string _employeeType;
    private readonly int _employeeId;

    private readonly string _juniorsCsvPath;
    private readonly string _teamLeadsCsvPath;

    public PreferencesController()
    {
        _employeeType = Environment.GetEnvironmentVariable("EMPLOYEE_TYPE") ?? "junior";
        _employeeId = int.TryParse(Environment.GetEnvironmentVariable("EMPLOYEE_ID") ?? "0", out _employeeId)
            ? _employeeId
            : 0;
        _juniorsCsvPath = "./Resources/Juniors5.csv";
        _teamLeadsCsvPath = "./Resources/Teamleads5.csv";
        
        Console.WriteLine($"Employee type: {_employeeType}");
        Console.WriteLine($"Employee id: {_employeeId}");
    }

    [HttpGet]
    public IActionResult GetPreferences()
    {
        Console.WriteLine("GetPreferences method called");
        List<Employee> teamLeads = CsvReader.ReadCsv(_teamLeadsCsvPath);
        List<Employee> juniors = CsvReader.ReadCsv(_juniorsCsvPath);
        teamLeads.ForEach(Console.WriteLine);
        juniors.ForEach(Console.WriteLine);
        try
        {
            // make enum Role
            return _employeeType.ToLower() switch
            {
                "junior" => Ok(BuildWishlist(juniors, teamLeads)),
                "teamLead" => Ok(BuildWishlist(teamLeads, juniors)),
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
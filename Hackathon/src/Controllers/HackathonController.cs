using Hackathon.DataBase.Dto;
using Hackathon.Model;
using Hackathon.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Controllers;

[ApiController]
[Route("api/hackathon")]
public class HackathonController(
    HackathonRepository hackathonRepository,
    TeamRepository teamRepository,
    EmployeeRepository employeeRepository,
    WishlistRepository wishlistRepository,
    SatisfactionRepository satisfactionRepository)
    : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllHackathons()
    {
        IEnumerable<HackathonDto> hackathons = hackathonRepository.GetAllHackathons();
        return Ok(hackathons);
    }

    [HttpGet("{id}")]
    public IActionResult GetHackathonById(int id)
    {
        var hackathon = hackathonRepository.GetHackathonById(id);
        if (hackathon == null)
        {
            return NotFound($"Hackathon with ID {id} not found.");
        }

        List<Team> teams = teamRepository.GetTeamByHackathonId(id)
            .ToList();

        List<Employee> employees = employeeRepository
            .GetAllEmployees()
            .ToList();

        List<Satisfaction> satisfactions = satisfactionRepository.GetSatisfactionsByHackathonId(id).ToList();
        List<Wishlist> wishlists = wishlistRepository.GetWishlistsByHackathonId(id).ToList();

        var result = new HackathonInfoDto
        {
            Hackathon = hackathon,
            Teams = teams,
            Employees = employees,
            Wishlists = wishlists,
            Satisfaction = satisfactions
        };

        return Ok(result);
    }

    [HttpGet("average-harmony")]
    public IActionResult GetAverageHarmony()
    {
        double averageHarmony = hackathonRepository.AverageResultHarmony();
        return Ok(new { averageHarmony });
    }

    public class HackathonInfoDto
    {
        public HackathonDto Hackathon { get; set; }
        public List<Team> Teams { get; set; }
        public List<Employee> Employees { get; set; }
        public List<Wishlist> Wishlists { get; set; }
        public List<Satisfaction> Satisfaction { get; set; }
    }
}
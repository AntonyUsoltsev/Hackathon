using Contract;
using HrDirector.Service;
using Microsoft.AspNetCore.Mvc;

namespace HrDirector.Controllers;

[Route("api/harmony")]
[ApiController]
public class HarmonicController(HrDirectorService hrDirectorService) : ControllerBase
{
    [HttpPost]
    public IActionResult CalculateHarmony([FromBody] TeamsMessage teamsMessage)
    {
        Console.WriteLine("CalculateHarmony method called");
        
        // hrDirectorService.SaveTeams(teamsMessage.formedTeams, teamsMessage.hackathonId);
        // hrDirectorService.SaveWishlists(teamsMessage.teamLeadsWishlists, teamsMessage.juniorsWishlists, teamsMessage.hackathonId);

        double satisfaction = hrDirectorService.CalculateHarmony(teamsMessage.formedTeams, teamsMessage.teamLeadsWishlists,
            teamsMessage.juniorsWishlists, teamsMessage.hackathonId);
        // hrDirectorService.UpdateHackathonResult(satisfaction, teamsMessage.hackathonId);

        Console.WriteLine($"Satisfaction: {satisfaction}");
        return Ok();
    }
}
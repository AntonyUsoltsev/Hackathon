using HrDirector.MassTransit;
using HrDirector.Model;
using HrDirector.Service;
using HrManager.Model;
using Microsoft.AspNetCore.Mvc;

namespace HrDirector.Controllers;

[Route("api/harmony")]
[ApiController]
public class HarmonicController(HrDirectorService hrDirectorService) : ControllerBase
{
    [HttpPost]
    public IActionResult CalculateHarmony([FromBody] TeamsMessage teamsMessage)
    {
        Console.WriteLine($"CalculateHarmony method called. {teamsMessage}");
        hrDirectorService.SaveTeams(teamsMessage.formedTeams, teamsMessage.hackathonId);
        return Ok();
    }
}
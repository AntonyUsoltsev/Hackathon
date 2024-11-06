using HrDirector.Model;
using HrDirector.Service;
using Microsoft.AspNetCore.Mvc;

namespace HrDirector.Controllers;

[Route("api/harmony")]
[ApiController]
public class HarmonicController(IHrDirectorService iDirectorService) : ControllerBase
{
    [HttpPost]
    public IActionResult CalculateHarmony([FromBody] AllDataDto allDataDto)
    {
        Console.WriteLine("CalculateHarmony method called");

        var hackathonDto = iDirectorService.CreateEmptyHackathon();
        
        iDirectorService.SaveTeams(allDataDto.formedTeams, hackathonDto.Id);
        iDirectorService.SaveWishlists(allDataDto.teamLeadsWishlists, allDataDto.juniorsWishlists, hackathonDto.Id);

        double satisfaction = iDirectorService.CalculateHarmony(allDataDto.formedTeams, allDataDto.teamLeadsWishlists,
            allDataDto.juniorsWishlists, hackathonDto.Id);
        iDirectorService.UpdateHackathonResult(satisfaction, hackathonDto.Id);

        Console.WriteLine($"Satisafction: {satisfaction}");
        return Ok();
    }
}
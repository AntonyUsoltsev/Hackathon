using Contract;
using HrManager.Service;
using Microsoft.AspNetCore.Mvc;

namespace HrManager.Controllers;

[Route("api/teams")]
[ApiController]
public class TeamsController(ITeamBuilder iTeamBuilder) : ControllerBase
{
    [HttpPost("junior-wishlist")]
    public IActionResult SubmitJuniorPreferences([FromBody] WishlistMessage dto)
    {
        iTeamBuilder.SaveJuniorWishlist(dto);
        Console.WriteLine($"Junior wishlist received: {dto}");
        return Ok($"Junior wishlist received: {dto}");
    }

    [HttpPost("teamlead-wishlist")]
    public IActionResult SubmitTeamLeadPreferences([FromBody] WishlistMessage dto)
    {
        iTeamBuilder.SaveTeamLeadWishlist(dto);
        Console.WriteLine($"Teamlead wishlist received: {dto}");
        return Ok($"Teamlead wishlist received: {dto}");
    }
}
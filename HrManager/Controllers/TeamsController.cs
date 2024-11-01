using Hackathon.Model;
using HrManager.Service;
using Microsoft.AspNetCore.Mvc;

namespace HrManager.Controllers;

[Route("api/teams")]
[ApiController]
public class TeamsController(ITeamBuilder iTeamBuilder) : ControllerBase
{
    [HttpPost("junior-wishlist")]
    public IActionResult SubmitJuniorPreferences([FromBody] Wishlist wishlist)
    {
        iTeamBuilder.SaveJuniorWishlist(wishlist);
        Console.WriteLine($"Junior wishlist received: {wishlist}");
        return Ok($"Junior wishlist received: {wishlist}");
    }

    [HttpPost("teamlead-wishlist")]
    public IActionResult SubmitTeamLeadPreferences([FromBody] Wishlist wishlist)
    {
        iTeamBuilder.SaveTeamLeadWishlist(wishlist);
        Console.WriteLine($"Teamlead wishlist received: {wishlist}");
        return Ok($"Teamlead wishlist received: {wishlist}");
    }
}
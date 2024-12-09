using EmployeeService.Model;
using HrDirector.Service;
using MassTransit;

namespace HrDirector.MassTransit;

public class WishlistConsumer(IHrDirectorService iHrDirectorService) : IConsumer<DTO>
{
    public async Task Consume(ConsumeContext<DTO> context)
    {
        var dto = context.Message;
        // Console.WriteLine($"Received wishlist from RabbitMQ: {dto}");

        switch (dto.Role)
        {
            case Role.Junior:
                iHrDirectorService.SaveJuniorWishlist(dto);
                // Console.WriteLine("Processed junior wishlist.");
                break;
            case Role.TeamLead:
                iHrDirectorService.SaveTeamLeadWishlist(dto);
                // Console.WriteLine("Processed team lead wishlist.");
                break;
            default:
                Console.WriteLine("Unknown role type, ignoring message.");
                break;
        }

        await Task.CompletedTask;
    }
}
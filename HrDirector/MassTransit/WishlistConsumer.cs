using EmployeeService.Model;
using HrDirector.Service;
using MassTransit;

namespace HrManager.MassTransit;

public class WishlistConsumer(HrDirectorService hrDirectorService) : IConsumer<DTO>
{
    public async Task Consume(ConsumeContext<DTO> context)
    {
        var dto = context.Message;
        Console.WriteLine($"Received wishlist from RabbitMQ: {dto}");

        switch (dto.Role)
        {
            case Role.Junior:
                hrDirectorService.SaveJuniorWishlist(dto);
                Console.WriteLine("Processed junior wishlist.");
                break;
            case Role.TeamLead:
                hrDirectorService.SaveTeamleadWishlist(dto);
                Console.WriteLine("Processed team lead wishlist.");
                break;
            default:
                Console.WriteLine("Unknown role type, ignoring message.");
                break;
        }

        await Task.CompletedTask;
    }
}
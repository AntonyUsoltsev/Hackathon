using EmployeeService.Model;
using HrManager.Service;
using MassTransit;

namespace HrManager.MassTransit;

public class WishlistConsumer(ITeamBuilder teamBuilder) : IConsumer<DTO>
{
    public async Task Consume(ConsumeContext<DTO> context)
    {
        var dto = context.Message;
        Console.WriteLine($"Received wishlist from RabbitMQ: {dto}");

        switch (dto.Role)
        {
            case Role.Junior:
                teamBuilder.SaveJuniorWishlist(dto);
                Console.WriteLine("Processed junior wishlist.");
                break;
            case Role.TeamLead:
                teamBuilder.SaveTeamLeadWishlist(dto);
                Console.WriteLine("Processed team lead wishlist.");
                break;
            default:
                Console.WriteLine("Unknown role type, ignoring message.");
                break;
        }

        await Task.CompletedTask;
    }
}
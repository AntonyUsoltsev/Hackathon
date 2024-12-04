using Contract;
using HrManager.Service;
using MassTransit;

namespace HrManager.MassTransit;

public class WishlistConsumer(ITeamBuilder teamBuilder) : IConsumer<WishlistMessage>
{
    public async Task Consume(ConsumeContext<WishlistMessage> context)
    {
        var message = context.Message;
        Console.WriteLine($"Received wishlist from RabbitMQ: {message}");

        switch (message.Role)
        {
            case Role.Junior:
                teamBuilder.SaveJuniorWishlist(message);
                Console.WriteLine("Processed junior wishlist.");
                break;
            case Role.TeamLead:
                teamBuilder.SaveTeamLeadWishlist(message);
                Console.WriteLine("Processed team lead wishlist.");
                break;
            default:
                Console.WriteLine("Unknown role type, ignoring message.");
                break;
        }

        await Task.CompletedTask;
    }
}
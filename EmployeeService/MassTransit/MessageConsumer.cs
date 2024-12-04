using EmployeeService.Service;
using HrDirector.MassTransit;
using MassTransit;

namespace EmployeeService.MassTransit;

public class MessageConsumer(BuildWishlistService buildWishlistService) : IConsumer<StartHackathonMessage>
{
    public Task Consume(ConsumeContext<StartHackathonMessage> context)
    {
        Console.WriteLine($"{nameof(MessageConsumer)} : {context.Message}");
        buildWishlistService.StartHackathon(context.Message.HackathonId);
        return Task.CompletedTask;
    }
}
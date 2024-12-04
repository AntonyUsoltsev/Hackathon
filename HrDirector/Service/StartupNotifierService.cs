using HrDirector.MassTransit;
using MassTransit;

namespace HrDirector.Service;

public class StartupNotifierService(IHrDirectorService hrDirectorService, IPublishEndpoint publishEndpoint)
    
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var hackathonDto = hrDirectorService.CreateEmptyHackathon();
        Console.WriteLine($"Starting hackathon with id = {hackathonDto.Id}");
        
        var startupEvent = new StartHackathonMessage
        {
            HackathonId = hackathonDto.Id,
            Message = "Starting hackathon"
        };

        await publishEndpoint.Publish(startupEvent, cancellationToken);
        Console.WriteLine("Hackathon started message was send");
    }

}
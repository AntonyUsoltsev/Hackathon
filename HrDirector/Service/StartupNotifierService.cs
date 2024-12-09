using HrDirector.MassTransit;
using MassTransit;

namespace HrDirector.Service;

public class StartupNotifierService
{
    private readonly IHrDirectorService _hrDirectorService;
    private readonly IPublishEndpoint _publishEndpoint;

    public StartupNotifierService(IHrDirectorService hrDirectorService, IPublishEndpoint publishEndpoint)
    {
        _hrDirectorService = hrDirectorService;
        _publishEndpoint = publishEndpoint;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(3000, cancellationToken);
        for (int i = 0; i <= 10; i++)
        {
            var hackathonDto = _hrDirectorService.CreateEmptyHackathon();
            Console.WriteLine($"Starting hackathon with id = {hackathonDto.Id}");

            var startupEvent = new StartHackathonMessage
            {
                HackathonId = hackathonDto.Id,
                Message = "Starting hackathon"
            };

            await _publishEndpoint.Publish(startupEvent, cancellationToken);
            Console.WriteLine($"Hackathon {hackathonDto.Id} started message was sent");

            await Task.Delay(1000, cancellationToken);
        }
    }
}

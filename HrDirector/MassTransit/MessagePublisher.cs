using Contract;
using MassTransit;

namespace HrDirector.MassTransit;

public class MessagePublisher(IBus bus)
{
    protected void SendMessage(int hackathonId)
    {
        bus.Publish(new StartHackathonMessage
        {
            Message = "Starting hackathon",
            HackathonId = hackathonId
        });
    }
}
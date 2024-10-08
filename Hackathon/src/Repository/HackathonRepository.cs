using Hackathon.DataBase;
using Hackathon.DataBase.Dto;

namespace Hackathon.Repository;

public class HackathonRepository(HackathonContext context)
{
    public IEnumerable<HackathonDto> GetAllHackathons()
    {
        return context.Hackathons.ToList();
    }

    public HackathonDto GetHackathonById(int id)
    {
        return context.Hackathons.SingleOrDefault(h => h.Id == id);
    }

    public HackathonDto CreateEmptyHackathon()
    {
        var hackathonDto = new HackathonDto(0);
        context.Hackathons.Add(hackathonDto);
        context.SaveChanges();
        return hackathonDto;
    }

    public HackathonDto UpdateHackathonResult(int id, double resultHarmony)
    {
        var hackathonDto = GetHackathonById(id);
        if (hackathonDto == null)
        {
            throw new Exception($"Hackathon with ID {id} not found.");
        }

        hackathonDto.ResultHarmony = resultHarmony;
        context.SaveChanges();
        return hackathonDto;
    }

    public HackathonDto SaveNewHackathon(int resultHarmony)
    {
        var hackathonDto = new HackathonDto(resultHarmony);
        context.Hackathons.Add(hackathonDto);
        context.SaveChanges();
        return hackathonDto;
    }
}
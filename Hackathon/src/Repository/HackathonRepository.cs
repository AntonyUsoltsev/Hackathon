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
    
    public HackathonDto SaveNewHackathon(int resultHarmony)
    {
        var hackathonDto = new HackathonDto(resultHarmony);
        context.Hackathons.Add(hackathonDto);
        context.SaveChanges();
        return hackathonDto;
    } 
}

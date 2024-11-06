using HrDirector.DataBase;
using HrDirector.DataBase.Dto;

namespace HrDirector.Repository;

public class HackathonRepository(HackathonContext context)
{
    public IEnumerable<HackathonEntity> GetAllHackathons()
    {
        return context.Hackathons.ToList();
    }

    public HackathonEntity GetHackathonById(int id)
    {
        return context.Hackathons.SingleOrDefault(h => h.Id == id);
    }

    public HackathonEntity CreateEmptyHackathon()
    {
        var hackathonDto = new HackathonEntity(0);
        context.Hackathons.Add(hackathonDto);
        context.SaveChanges();
        return hackathonDto;
    }

    public HackathonEntity UpdateHackathonResult(int id, double resultHarmony)
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

    public HackathonEntity SaveNewHackathon(double resultHarmony)
    {
        var hackathonDto = new HackathonEntity(resultHarmony);
        context.Hackathons.Add(hackathonDto);
        context.SaveChanges();
        return hackathonDto;
    }

    public double AverageResultHarmony()
    {
        double averageHarmony = context.Hackathons.Average(h => h.ResultHarmony);
        Console.WriteLine($"Average Result Harmony: {averageHarmony}");
        return averageHarmony;
    }
}
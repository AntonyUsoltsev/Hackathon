using Hackathon.DataBase;
using Hackathon.DataBase.Dto;

namespace Hackathon.Repository;

public class SatisfactionRepository(HackathonContext context)
{
    public IEnumerable<SatisfactionDto> GetSatisfactionsByHackathonId(int hackathonId)
    {
        return context.Satisfactions
            .Where(s => s.HackathonId == hackathonId)
            .ToList();
    }

    public void SaveSatisfaction(int hackathonId, int employeeId, int satisfactionRank)
    {
        var satisfactionDto = new SatisfactionDto(hackathonId, employeeId, satisfactionRank);
        context.Satisfactions.Add(satisfactionDto);
        context.SaveChanges();
    }
}
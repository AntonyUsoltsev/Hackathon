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
    
    public void SaveSatisfaction(SatisfactionDto satisfaction)
    {
        context.Satisfactions.Add(satisfaction);
        context.SaveChanges();
    }
}
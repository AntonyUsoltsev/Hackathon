using Hackathon.DataBase;
using Hackathon.DataBase.Dto;
using Hackathon.DtoMapper;
using Hackathon.Model;

namespace Hackathon.Repository;

public class SatisfactionRepository(HackathonContext context)
{
    public IEnumerable<Satisfaction> GetSatisfactionsByHackathonId(int hackathonId)
    {
        List<SatisfactionDto> satisfactionsDtos = context.Satisfactions
            .Where(s => s.HackathonId == hackathonId)
            .ToList();

        var satisfactions = new List<Satisfaction>();

        foreach (var satisfactionDto in satisfactionsDtos)
        {
            var employeeDto = context.Employees.SingleOrDefault(e => e.Id == satisfactionDto.EmployeeId);

            if (employeeDto != null)
            {
                var team = Mapper.MapSatisfaction(employeeDto, satisfactionDto.SatisfactionRank);
                satisfactions.Add(team);
            }
        }

        return satisfactions;
    }

    public void SaveSatisfaction(int hackathonId, int employeeId, int satisfactionRank)
    {
        var satisfactionDto = new SatisfactionDto(hackathonId, employeeId, satisfactionRank);
        context.Satisfactions.Add(satisfactionDto);
        context.SaveChanges();
    }
}
using HrDirector.DataBase;
using HrDirector.DataBase.Dto;
using HrDirector.DtoMapper;
using HrDirector.Model;

namespace HrDirector.Repository;

public class SatisfactionRepository(HackathonContext context)
{
    public IEnumerable<Satisfaction> GetSatisfactionsByHackathonId(int hackathonId)
    {
        List<SatisfactionEntity> satisfactionsDtos = context.Satisfactions
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
        var satisfactionDto = new SatisfactionEntity(hackathonId, employeeId, satisfactionRank);
        context.Satisfactions.Add(satisfactionDto);
        context.SaveChanges();
    }
}
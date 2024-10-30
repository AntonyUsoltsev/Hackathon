using Hackathon.DataBase.Dto;
using Hackathon.Model;

namespace Hackathon.DtoMapper;

public class Mapper
{
    public static Employee MapEmployee(EmployeeDto employeeDto)
    {
        return new Employee(employeeDto.Id, employeeDto.Name);
    }

    public static Team MapTeam(EmployeeDto teamLeadDto, EmployeeDto juniorDto)
    {
        return new Team(MapEmployee(teamLeadDto), MapEmployee(juniorDto));
    }

    public static Satisfaction MapSatisfaction(EmployeeDto employeeDto, int satisfactionRank)
    {
        return new Satisfaction(MapEmployee(employeeDto), satisfactionRank);
    }
}
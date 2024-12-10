using EmployeeService.Model;
using HrDirector.DataBase.Dto;
using HrDirector.Model;
using HrManager.Model;

namespace HrDirector.DtoMapper;

public class Mapper
{
    public static Employee MapEmployee(EmployeeEntity employeeDto)
    {
        return new Employee(employeeDto.Id, employeeDto.Name);
    }

    public static Team MapTeam(EmployeeEntity teamLeadDto, EmployeeEntity juniorDto)
    {
        return new Team(MapEmployee(teamLeadDto), MapEmployee(juniorDto));
    }

    public static Satisfaction MapSatisfaction(EmployeeEntity employeeDto, int satisfactionRank)
    {
        return new Satisfaction(MapEmployee(employeeDto), satisfactionRank);
    }
}
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
    
    public static Wishlist MapWishlist(EmployeeDto employeeDto, int[] desiredEmployees)
    {
        return new Wishlist(employeeDto.Id, desiredEmployees);
    }
}
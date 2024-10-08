using Hackathon.DataBase;
using Hackathon.DataBase.Dto;
using Hackathon.DtoMapper;
using Hackathon.Model;

namespace Hackathon.Repository;

public class EmployeeRepository(HackathonContext context)
{
    public IEnumerable<Employee> GetEmployeesByRole(string role)
    {
        List<EmployeeDto> employeeDtos = context.Employees
            .Where(e => e.Role == role)
            .ToList();
        return employeeDtos.Select(Mapper.MapEmployee).ToList();
    }
}
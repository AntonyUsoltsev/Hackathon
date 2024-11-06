using HrDirector.DataBase;
using HrDirector.DataBase.Dto;
using HrDirector.DtoMapper;
using Employee = HrDirector.Model.Employee;

namespace HrDirector.Repository;

public class EmployeeRepository(HackathonContext context)
{
    public IEnumerable<Employee> GetAllEmployees()
    {
        List<EmployeeDto> employeeDtos = context.Employees.ToList();
        return employeeDtos.Select(Mapper.MapEmployee).ToList();
    }

    public IEnumerable<Employee> GetEmployeesByRole(string role)
    {
        List<EmployeeDto> employeeDtos = context.Employees
            .Where(e => e.Role == role)
            .ToList();
        return employeeDtos.Select(Mapper.MapEmployee).ToList();
    }
}
using Contract;

namespace EmployeeService.Util;

public class CsvReader
{
    public static List<Employee> ReadCsv(string filePath)
    {
        var result = new List<Employee>();

        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            string[] parts = line.Split(';');
            if (int.TryParse(parts[0], out int id))
            {
                result.Add(new Employee(id, parts[1]));
            }
        }

        return result;
    }
}
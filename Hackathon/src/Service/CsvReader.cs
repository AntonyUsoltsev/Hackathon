using Hackathon.Model;

namespace Hackathon
{
    public class CsvReader
    {
        public static List<Employee> ReadCsv(string filePath)
        {
            List<Employee> result = new List<Employee>();

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split(';');
                if (int.TryParse(parts[0], out int id))
                {
                    result.Add(new Employee(id, parts[1]));
                }
            }

            return result;
        }
    }
}
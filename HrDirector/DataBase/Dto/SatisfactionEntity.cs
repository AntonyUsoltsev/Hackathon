namespace HrDirector.DataBase.Dto;

public class SatisfactionEntity(int hackathonId, int employeeId, int satisfactionRank)
{
    public int Id { get; set; }
    public int HackathonId { get; set; } = hackathonId;
    public HackathonEntity Hackathon { get; set; }
    public int EmployeeId { get; set; } = employeeId;
    public EmployeeEntity Employee { get; set; }
    public int SatisfactionRank { get; set; } = satisfactionRank;
}
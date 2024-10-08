namespace Hackathon.DataBase.Dto;

public class SatisfactionDto(int employeeId, int hackathonId, int satisfactionRank)
{
    public int Id { get; init; }
    public int EmployeeId { get; init; } = employeeId;
    public int HackathonId { get; init; } = hackathonId;
    public int SatisfactionRank { get; init; } = satisfactionRank;
}
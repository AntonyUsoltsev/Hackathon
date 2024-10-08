namespace Hackathon.DataBase.Dto;

public class SatisfactionDto(int hackathonId, int employeeId, int satisfactionRank)
{
    public int Id { get; init; }
    public int HackathonId { get; init; } = hackathonId;
    public int EmployeeId { get; init; } = employeeId;
    public int SatisfactionRank { get; init; } = satisfactionRank;
}
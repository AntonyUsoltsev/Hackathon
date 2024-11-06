namespace HrDirector.DataBase.Dto;

public class TeamDto(int teamLeadId, int juniorId, int hackathonId)
{
    public int Id { get; init; }
    public int TeamLeadId { get; init; } = teamLeadId;
    public int JuniorId { get; init; } = juniorId;
    public int HackathonId { get; init; } = hackathonId;
}
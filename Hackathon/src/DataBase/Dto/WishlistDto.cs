namespace Hackathon.DataBase.Dto;

public class WishlistDto(int employeeId, int chosenEmployeeId, int rank, int hackathonId)
{
    public int Id { get; init; }
    public int EmployeeId { get; init; } = employeeId;
    public int ChosenEmployeeId { get; init; } = chosenEmployeeId;
    
    public int Rank { get; init; } = rank;
    public int HackathonId { get; init; } = hackathonId;
}
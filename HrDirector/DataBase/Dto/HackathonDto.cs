namespace HrDirector.DataBase.Dto;

public class HackathonDto(double resultHarmony)
{
    public int Id { get; init; }
    public double ResultHarmony { get; set; } = resultHarmony;
}
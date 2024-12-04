namespace Contract;

public record StartHackathonMessage
{
    public int HackathonId { get; init; } = -1;
    public string Message { get; init; } = string.Empty;
}
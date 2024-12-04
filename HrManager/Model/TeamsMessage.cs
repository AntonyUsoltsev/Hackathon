namespace HrManager.Model;

public record TeamsMessage(
    IEnumerable<Team> formedTeams,
    int hackathonId);
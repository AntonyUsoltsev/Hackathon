namespace Contract;

public record TeamsMessage(
    IEnumerable<Team> formedTeams,
    int hackathonId);
using HrDirector.DataBase;
using HrDirector.DataBase.Dto;
using HrDirector.DtoMapper;
using HrDirector.Model;

namespace HrDirector.Repository;

public class TeamRepository(HackathonContext context)
{
    public IEnumerable<Team> GetTeamByHackathonId(int hackathonId)
    {
        List<TeamEntity> teamDtos = context.Teams
            .Where(t => t.HackathonId == hackathonId)
            .ToList();

        var teams = new List<Team>();

        foreach (var teamDto in teamDtos)
        {
            var teamLeadDto = context.Employees.SingleOrDefault(e => e.Id == teamDto.TeamLeadId);
            var juniorDto = context.Employees.SingleOrDefault(e => e.Id == teamDto.JuniorId);

            if (teamLeadDto != null && juniorDto != null)
            {
                var team = Mapper.MapTeam(teamLeadDto, juniorDto);
                teams.Add(team);
            }
        }

        return teams;
    }

    public TeamEntity SaveTeam(Team team, int hackathonId)
    {
        var teamDto = new TeamEntity(team.TeamLead.Id, team.Junior.Id, hackathonId);
        context.Teams.Add(teamDto);
        context.SaveChanges();
        return teamDto;
    }
}
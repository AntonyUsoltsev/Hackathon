using Hackathon.DataBase;
using Hackathon.DataBase.Dto;
using Hackathon.DtoMapper;
using Hackathon.Model;

namespace Hackathon.Repository;

public class TeamRepository(HackathonContext context)
{
    public IEnumerable<Team> GetTeamByHackathonId(int hackathonId)
    {
        List<TeamDto> teamDtos = context.Teams
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

    public TeamDto SaveTeam(Team team, int hackathonId)
    {
        var teamDto = new TeamDto(team.TeamLead.Id, team.Junior.Id, hackathonId);
        context.Teams.Add(teamDto);
        context.SaveChanges();
        return teamDto;
    }
}
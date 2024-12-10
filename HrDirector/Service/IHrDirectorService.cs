using EmployeeService.Model;
using HrDirector.DataBase.Dto;
using HrDirector.Model;
using HrManager.Model;

namespace HrDirector.Service;

public interface IHrDirectorService
{
    public void SaveTeamLeadWishlist(DTO dto);
    public void SaveJuniorWishlist(DTO dto);
    public void SaveTeams(IEnumerable<Team> teams, int hackathonId);
    public HackathonEntity CreateEmptyHackathon();
}
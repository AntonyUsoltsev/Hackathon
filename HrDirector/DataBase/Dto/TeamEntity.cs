using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrDirector.DataBase.Dto;

public class TeamEntity(int teamLeadId, int juniorId, int hackathonId)
{
    public int Id { get; set; }
    public int HackathonId { get; set; } = hackathonId;
    public HackathonEntity Hackathon { get; set; }
    public int JuniorId { get; set; } = juniorId;
    public EmployeeEntity Junior { get; set; }
    public int TeamLeadId { get; set; } = teamLeadId;
    public EmployeeEntity TeamLead { get; set; }
}
using EmployeeService.Model;

namespace HrManager.Service;

public interface ITeamBuilder
{
    public void SaveJuniorWishlist(DTO dto);
    public void SaveTeamLeadWishlist(DTO dto);
}
namespace HrDirector.DataBase.Dto;

public class EmployeeEntity
{
    public EmployeeEntity(string Role, string Name)
    {
        this.Role = Role;
        this.Name = Name;
    }
    public EmployeeEntity(int Id, string Role, string Name)
    {
        this.Id = Id;
        this.Role = Role;
        this.Name = Name;
    }
    public int Id { get; set; }
    public string Role { get; set; }
    public string Name { get; set; }
    public ICollection<TeamEntity> TeamsAsJunior { get; set; }
    public ICollection<TeamEntity> TeamsAsTeamLead { get; set; }
    public ICollection<WishlistEntity> Wishlists { get; set; }
    public ICollection<WishlistEntity> ChosenWishlists { get; set; }
    public ICollection<SatisfactionEntity> Satisfactions { get; set; }
}
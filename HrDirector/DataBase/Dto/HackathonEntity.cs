namespace HrDirector.DataBase.Dto;

public class HackathonEntity(double resultHarmony)
{
    public int Id { get; set; }
    public double ResultHarmony { get; set; } = resultHarmony;
    public ICollection<TeamEntity> Teams { get; set; }
    public ICollection<WishlistEntity> Wishlists { get; set; }
    public ICollection<SatisfactionEntity> Satisfactions { get; set; }
}
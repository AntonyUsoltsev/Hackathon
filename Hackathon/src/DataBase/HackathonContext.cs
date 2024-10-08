using Hackathon.DataBase.Dto;

namespace Hackathon.DataBase;

using Microsoft.EntityFrameworkCore;

public class HackathonContext : DbContext
{
    public DbSet<HackathonDto> Hackathons { get; set; }
    public DbSet<EmployeeDto> Employees { get; set; }
    public DbSet<WishlistDto> Wishlists { get; set; }
    public DbSet<TeamDto> Teams { get; set; }
    public DbSet<SatisfactionDto> Satisfactions { get; set; }

    public HackathonContext(DbContextOptions<HackathonContext> options) 
        : base(options) { }
}
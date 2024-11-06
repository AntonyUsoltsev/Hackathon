using HrDirector.DataBase.Dto;
using Microsoft.EntityFrameworkCore;

namespace HrDirector.DataBase;

public class HackathonContext : DbContext
{
    public DbSet<HackathonEntity> Hackathons { get; set; }
    public DbSet<EmployeeEntity> Employees { get; set; }
    public DbSet<TeamEntity> Teams { get; set; }
    public DbSet<WishlistEntity> Wishlists { get; set; }
    public DbSet<SatisfactionEntity> Satisfactions { get; set; }

    public HackathonContext(DbContextOptions<HackathonContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeEntity>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<TeamEntity>()
            .HasOne(t => t.Hackathon)
            .WithMany(h => h.Teams)
            .HasForeignKey(t => t.HackathonId);

        modelBuilder.Entity<TeamEntity>()
            .HasOne(t => t.Junior)
            .WithMany(e => e.TeamsAsJunior)
            .HasForeignKey(t => t.JuniorId);

        modelBuilder.Entity<TeamEntity>()
            .HasOne(t => t.TeamLead)
            .WithMany(e => e.TeamsAsTeamLead)
            .HasForeignKey(t => t.TeamLeadId);

        modelBuilder.Entity<WishlistEntity>()
            .HasOne(w => w.Hackathon)
            .WithMany(h => h.Wishlists)
            .HasForeignKey(w => w.HackathonId);

        modelBuilder.Entity<WishlistEntity>()
            .HasOne(w => w.Employee)
            .WithMany(e => e.Wishlists)
            .HasForeignKey(w => w.EmployeeId);

        modelBuilder.Entity<WishlistEntity>()
            .HasOne(w => w.ChosenEmployee)
            .WithMany(e => e.ChosenWishlists)
            .HasForeignKey(w => w.ChosenEmployeeId);

        modelBuilder.Entity<SatisfactionEntity>()
            .HasOne(s => s.Hackathon)
            .WithMany(h => h.Satisfactions)
            .HasForeignKey(s => s.HackathonId);

        modelBuilder.Entity<SatisfactionEntity>()
            .HasOne(s => s.Employee)
            .WithMany(e => e.Satisfactions)
            .HasForeignKey(s => s.EmployeeId);

        modelBuilder.Entity<EmployeeEntity>().HasData(
            new EmployeeEntity(1, "junior", "Юдин Адам"),
            new EmployeeEntity(2, "junior", "Яшина Яна"),
            new EmployeeEntity(3, "junior", "Никитина Вероника"),
            new EmployeeEntity(4, "junior", "Рябинин Александр"),
            new EmployeeEntity(5, "junior", "Ильин Тимофей")
        );

        modelBuilder.Entity<EmployeeEntity>().HasData(
            new EmployeeEntity(6, "teamLead", "Филиппова Ульяна"),
            new EmployeeEntity(7, "teamLead", "Николаев Григорий"),
            new EmployeeEntity(8, "teamLead", "Андреева Вероника"),
            new EmployeeEntity(9, "teamLead", "Коротков Михаил"),
            new EmployeeEntity(10, "teamLead", "Кузнецов Александр")
        );
    }
}
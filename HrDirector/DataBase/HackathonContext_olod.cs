// using HrDirector.DataBase.Dto;
// using Microsoft.EntityFrameworkCore;
//
// namespace HrDirector.DataBase;
//
// public class HackathonContext : DbContext
// {
//     public DbSet<HackathonEntity> Hackathons { get; init; }
//     public DbSet<EmployeeEntity> Employees { get; init; }
//     public DbSet<WishlistEntity> Wishlists { get; init; }
//     public DbSet<TeamDto> Teams { get; init; }
//     public DbSet<SatisfactionDto> Satisfactions { get; init; }
//     
//     public HackathonContext(DbContextOptions<HackathonContext> options)
//         : base(options)
//     {
//     }
//
//     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//         modelBuilder.Entity<HackathonDto>(entity =>
//         {
//             entity.ToTable("hackathons");
//             entity.Property(e => e.Id).HasColumnName("id");
//             entity.Property(e => e.ResultHarmony).HasColumnName("result_harmony");
//         });
//
//         modelBuilder.Entity<EmployeeDto>(entity =>
//         {
//             entity.ToTable("employees");
//             entity.Property(e => e.Id).HasColumnName("id");
//             entity.Property(e => e.Role).HasColumnName("role");
//             entity.Property(e => e.Name).HasColumnName("name");
//         });
//
//         modelBuilder.Entity<WishlistDto>(entity =>
//         {
//             entity.ToTable("wishlists");
//             entity.Property(e => e.Id).HasColumnName("id");
//             entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
//             entity.Property(e => e.ChosenEmployeeId).HasColumnName("chosen_employee_id");
//             entity.Property(e => e.Rank).HasColumnName("rank");
//             entity.Property(e => e.HackathonId).HasColumnName("hackathon_id");
//         });
//
//         modelBuilder.Entity<TeamDto>(entity =>
//         {
//             entity.ToTable("teams");
//             entity.Property(e => e.Id).HasColumnName("id");
//             entity.Property(e => e.TeamLeadId).HasColumnName("teamlead_id");
//             entity.Property(e => e.JuniorId).HasColumnName("junior_id");
//             entity.Property(e => e.HackathonId).HasColumnName("hackathon_id");
//         });
//
//         modelBuilder.Entity<SatisfactionDto>(entity =>
//         {
//             entity.ToTable("satisfactions");
//             entity.Property(e => e.Id).HasColumnName("id");
//             entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
//             entity.Property(e => e.HackathonId).HasColumnName("hackathon_id");
//             entity.Property(e => e.SatisfactionRank).HasColumnName("satisfaction_rank");
//         });
//     }
// }
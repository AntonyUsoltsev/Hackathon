using HrDirector.DataBase.Dto;
using Microsoft.EntityFrameworkCore;

namespace HrDirector.DataBase
{
    public class HackathonContext : DbContext
    {
        public DbSet<HackathonDto> Hackathons { get; set; }
        public DbSet<EmployeeDto> Employees { get; set; }
        public DbSet<TeamDto> Teams { get; set; }
        public DbSet<WishlistDto> Wishlists { get; set; }
        public DbSet<SatisfactionDto> Satisfactions { get; set; }

        public HackathonContext(DbContextOptions<HackathonContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamDto>()
                .HasOne<HackathonDto>()
                .WithMany()
                .HasForeignKey(t => t.HackathonId);

            modelBuilder.Entity<TeamDto>()
                .HasOne<EmployeeDto>()
                .WithMany()
                .HasForeignKey(t => t.JuniorId);

            modelBuilder.Entity<TeamDto>()
                .HasOne<EmployeeDto>()
                .WithMany()
                .HasForeignKey(t => t.TeamLeadId);

            modelBuilder.Entity<WishlistDto>()
                .HasOne<HackathonDto>()
                .WithMany()
                .HasForeignKey(w => w.HackathonId);

            modelBuilder.Entity<WishlistDto>()
                .HasOne<EmployeeDto>()
                .WithMany()
                .HasForeignKey(w => w.EmployeeId);

            modelBuilder.Entity<WishlistDto>()
                .HasOne<EmployeeDto>()
                .WithMany()
                .HasForeignKey(w => w.ChosenEmployeeId);

            modelBuilder.Entity<SatisfactionDto>()
                .HasOne<HackathonDto>()
                .WithMany()
                .HasForeignKey(s => s.HackathonId);

            modelBuilder.Entity<SatisfactionDto>()
                .HasOne<HackathonDto>()
                .WithMany()
                .HasForeignKey(s => s.EmployeeId);
        
            modelBuilder.Entity<HackathonDto>(entity =>
            {
                entity.ToTable("hackathons");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.ResultHarmony).HasColumnName("result_harmony");
            });

            modelBuilder.Entity<EmployeeDto>(entity =>
            {
                entity.ToTable("employees");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Role).HasColumnName("role");
                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<WishlistDto>(entity =>
            {
                entity.ToTable("wishlists");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
                entity.Property(e => e.ChosenEmployeeId).HasColumnName("chosen_employee_id");
                entity.Property(e => e.Rank).HasColumnName("rank");
                entity.Property(e => e.HackathonId).HasColumnName("hackathon_id");
            });

            modelBuilder.Entity<TeamDto>(entity =>
            {
                entity.ToTable("teams");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.TeamLeadId).HasColumnName("teamlead_id");
                entity.Property(e => e.JuniorId).HasColumnName("junior_id");
                entity.Property(e => e.HackathonId).HasColumnName("hackathon_id");
            });

            modelBuilder.Entity<SatisfactionDto>(entity =>
            {
                entity.ToTable("satisfactions");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
                entity.Property(e => e.HackathonId).HasColumnName("hackathon_id");
                entity.Property(e => e.SatisfactionRank).HasColumnName("satisfaction_rank");
            });
        }
    }
}
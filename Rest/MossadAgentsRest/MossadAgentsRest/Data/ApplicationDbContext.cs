using Microsoft.EntityFrameworkCore;
using MossadAgentsRest.Models;

namespace MossadAgentsRest.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): DbContext(options)
    {
       
        //יצירת טבלאות לדתה בייס על פי המודלים
        public DbSet<AgentModel> AgentModel { get; set; }
        public DbSet<TargetModel> TargetModel { get; set; }
        public DbSet<MissionModel> MissionModel { get; set; }
        //הגדרת היחסים לדתה בייס
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MissionModel>()
                .HasOne(m => m.Agent)
                .WithMany(a => a.Missions)
                .HasForeignKey(m => m.AgentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MissionModel>()
                .HasOne(m => m.Target)
                .WithMany()
                .HasForeignKey(m => m.TargetId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AgentModel>()
                .Property(m => m.Status)
                .HasConversion<string>()
                .IsRequired();
            modelBuilder.Entity<TargetModel>()
                .Property(m => m.Status)
                .HasConversion<string>()
                .IsRequired();
            modelBuilder.Entity<MissionModel>()
               .Property(m => m.Status)
               .HasConversion<string>()
               .IsRequired();
          

            base.OnModelCreating(modelBuilder);
        }
    }
}

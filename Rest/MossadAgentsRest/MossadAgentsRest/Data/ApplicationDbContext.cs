using Microsoft.EntityFrameworkCore;
using MossadAgentsRest.Models;

namespace MossadAgentsRest.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): DbContext(options)
    {
        // Seed()
        private async Task Seed()
        {
            if (AgentModel == null)
            {
                AgentModel agent = new AgentModel
                {
                    Image = "",
                    Name = "Enosh",
                    Location_X = 5,
                    Location_Y = 5,
                    //Status = ,
                };
            };
        }

        public DbSet<AgentModel> AgentModel { get; set; }
        public DbSet<TargetModel> TargetModel { get; set; }
        public DbSet<MissionModel> MissionModel { get; set; }
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
           /* modelBuilder.Entity<MissionModel>()
                .HasOne(m => m.Target)
                .WithMany()
                .HasForeignKey(m => m.TargetId)
                .OnDelete(DeleteBehavior.Restrict);*/

            base.OnModelCreating(modelBuilder);
        }
    }
}

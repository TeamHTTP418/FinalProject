using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ReadyPlayerSite.Models
{
    public class PlayerDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<FreezeInfo> freezeInfos { get; set; }
        public DbSet<AdminAction> adminActions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().HasMany(p => p.tasksCompleted).WithMany().Map(pt => pt.ToTable("PlayerToTasks"));
            modelBuilder.Entity<Player>().HasMany(p => p.milestonesCompleted).WithMany().Map(pm => pm.ToTable("PlayerToMilestones"));
            modelBuilder.Entity<Player>().HasOptional(p => p.freezeInfo).WithRequired(fi => fi.player); 
            modelBuilder.Entity<Player>().HasRequired(p => p.user);
            modelBuilder.Entity<Player>().HasMany(p => p.adminActions).WithRequired(aa => aa.player).HasForeignKey(aa => aa.playerID).WillCascadeOnDelete(false);
            modelBuilder.Entity<AdminAction>().HasRequired(aa => aa.user).WithMany().HasForeignKey(s => s.userID).WillCascadeOnDelete(false);
         
            
        }
    }
}
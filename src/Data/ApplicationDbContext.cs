using Data.Models;
using Data.Models.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

namespace Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public override DbSet<User> Users { get; set; }
        public override DbSet<Role> Roles { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<UserStory> UserStories { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<SubTask> SubTasks { get; set; }
        public DbSet<Sprint> Sprints { get; set; }
        public DbSet<TeamRole> TeamRoles { get; set; }
        public DbSet<BacklogPriority> BacklogPriorities { get; set; }
        public DbSet<TicketSeverity> TicketSeverities { get; set; }
        public DbSet<SprintStatus> SprintStatuses { get; set; }
        public DbSet<MockupAttachment> MockupAttachments { get; set; }

    }
}

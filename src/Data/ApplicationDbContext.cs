﻿using Data.Models;
using Data.Models.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserStory> UserStories { get; set; }
        public DbSet<UserStoryTask> UserStoryTasks { get; set; }
        public DbSet<Bug> Bugs { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Sprint> Sprints { get; set; }
        public DbSet<TeamRole> TeamRoles { get; set; }
        public DbSet<BacklogPriority> BacklogPriorities { get; set; }
        public DbSet<Severity> Severities { get; set; }
        public DbSet<WorkItemType> WorkItemTypes { get; set; }
        public DbSet<SprintStatus> SprintStatuses { get; set; }
        public DbSet<MockupAttachment> MockupAttachments { get; set; }
        public DbSet<TeamsUsers> TeamsUsers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TeamsUsers>(x => x.HasKey(x => new { x.TeamId, x.UserId }));

            base.OnModelCreating(builder);
        }

    }
}

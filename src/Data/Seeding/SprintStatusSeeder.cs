using Data.Models;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Seeding
{
    public class SprintStatusSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var sprintStatuses = new List<string>()
            {
                SprintStatusConstants.Accepted,
                SprintStatusConstants.Active,
                SprintStatusConstants.Closed,
                SprintStatusConstants.Planning
            };
            foreach (var status in sprintStatuses)
            {
                await SeedSprintStatus(dbContext, status);
            }

        }

        private async Task SeedSprintStatus(ApplicationDbContext dbContext, string status)
        {
            var isSprintStatusSeeded = dbContext.SprintStatuses.Any(x => x.Status == status);

            if (!isSprintStatusSeeded)
            {
                await dbContext.SprintStatuses.AddAsync(new SprintStatus
                {
                    AddedOn = DateTime.UtcNow,
                    Status = status
                });
            }
        }
    }
}

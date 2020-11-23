using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Seeding
{
    public class BacklogPrioritySeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var priorities = new Dictionary<string, int>()
            {
                {BacklogPriorityConstants.LevelOne, 1},
                {BacklogPriorityConstants.LevelTwo, 2},
                {BacklogPriorityConstants.LevelThree, 3},
                {BacklogPriorityConstants.LevelFour, 4},
                { BacklogPriorityConstants.LevelFive, 5},
            };
            foreach (var kvp in priorities)
            {
                await SeedBacklogPrioriesAsync(dbContext, kvp.Key, kvp.Value);
            }
        }
        private async Task SeedBacklogPrioriesAsync(ApplicationDbContext dbContext, string priorityName, int weight)
        {
            var isPrioritySeeded = dbContext.BacklogPriorities
                .Any(x => x.Priority == priorityName && x.Weight == weight);

            if (!isPrioritySeeded)
            {
                await dbContext.BacklogPriorities.AddAsync(new Models.BacklogPriority
                {
                    AddedOn = DateTime.UtcNow,
                    Priority = priorityName,
                    Weight = weight
                });
            }
        }
    }
}

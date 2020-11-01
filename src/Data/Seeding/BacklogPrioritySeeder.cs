using System;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Seeding
{
    public class BacklogPrioritySeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var dict = Enum.GetValues(typeof(Shared.Enums.BacklogPriorityEnum)).Cast<int>()
                  .ToDictionary(v => v, v => Enum.GetName(typeof(Shared.Enums.BacklogPriorityEnum), v));

            foreach (var kvp in dict)
            {
                await SeedBacklogPrioriesAsync(dbContext, kvp.Value, kvp.Key);
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

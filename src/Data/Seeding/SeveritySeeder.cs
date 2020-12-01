using Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Seeding
{
    public class SeveritySeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var dict = Enum.GetValues(typeof(Shared.Enums.SeverityEnum)).Cast<int>()
                  .ToDictionary(v => v, v => Enum.GetName(typeof(Shared.Enums.SeverityEnum), v));

            foreach (var kvp in dict)
            {
                await SeedSeverity(dbContext, kvp.Value, kvp.Key);
            }
        }

        private async Task SeedSeverity(ApplicationDbContext dbContext, string severity, int weight)
        {
            var isSeveritySeeded = dbContext.Severities.Any(x => x.SeverityName == severity && x.Weight == weight);

            if (!isSeveritySeeded)
            {
                await dbContext.Severities.AddAsync(new Severity
                {
                    AddedOn = DateTime.UtcNow,
                    SeverityName = severity,
                    Weight = weight,
                });
            }
        }
    }
}

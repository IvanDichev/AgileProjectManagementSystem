using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var isSeveritySeeded = dbContext.TicketSeverities.Any(x => x.Severity == severity && x.Weight == weight);

            if (!isSeveritySeeded)
            {
                await dbContext.TicketSeverities.AddAsync(new TicketSeverity
                {
                    AddedOn = DateTime.UtcNow,
                    Severity = severity,
                    Weight = weight
                });
            }
        }
    }
}

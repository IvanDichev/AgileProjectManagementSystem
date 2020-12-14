using Data.Models;
using Microsoft.EntityFrameworkCore;
using Repo;
using Shared.Constants.Seeding;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Services.BurndownDatas
{
    public class BurndownDataService : IBurndownDataService
    {
        private readonly IRepository<BurndownData> burndownRepo;

        public BurndownDataService(IRepository<BurndownData> burndownRepo)
        {
            this.burndownRepo = burndownRepo;
        }

        /// <summary>
        /// Update total tasks and finished tasks.
        /// </summary>
        /// <returns></returns>
        public async Task UpdateData()
        {
            var burndown = await this.burndownRepo.AllAsNoTracking()
                .OrderBy(x => x.SprintId)
                .ThenBy(x => x.DayOfSprint)
                .ToListAsync();

            var distinctBundowns = await this.burndownRepo.AllAsNoTracking()
                .OrderBy(x => x.SprintId)
                .Select(x => x.SprintId)
                .Distinct()
                .ToListAsync();

            foreach (var sprintId in distinctBundowns)
            {
                var allBurndownsForSprint = await this.burndownRepo.All()
                    .Where(x => x.SprintId == sprintId &&
                               (x.Sprint.Status.Status == SprintStatusConstants.Planning ||
                                x.Sprint.Status.Status == SprintStatusConstants.Active))
                    .OrderBy(x => x.DayOfSprint)
                    .ToListAsync();

                if (allBurndownsForSprint.Count >= 2)
                {
                    var toUpdate = allBurndownsForSprint.Where(x => x.DayOfSprint.Date == DateTime.UtcNow.Date).FirstOrDefault();
                    if (toUpdate != allBurndownsForSprint[0])
                    {
                        var lastDay = allBurndownsForSprint.Where(x => x.DayOfSprint.Date == DateTime.UtcNow.Date.AddDays(-1)).FirstOrDefault();
                        toUpdate.TotalTasks = lastDay.TotalTasks;
                        toUpdate.FinishedTasks = lastDay.FinishedTasks;
                    }
                }
            }

            await this.burndownRepo.SaveChangesAsync();
        }
    }
}

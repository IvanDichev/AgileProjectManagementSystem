using Data.Models;
using Shared.Constants.Seeding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Seeding
{
    public class WorkItemTypesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var workItems = new List<string>()
            {
                WorkItemTypesConstants.Bug,
                WorkItemTypesConstants.SubTask,
                WorkItemTypesConstants.Task,
                WorkItemTypesConstants.TestCase,
                WorkItemTypesConstants.UserStory,
            };

            foreach (var workItem in workItems)
            {
                await this.SeedWorkItemTypes(dbContext, workItem);
            }            
        }

        private async Task SeedWorkItemTypes(ApplicationDbContext dbContext, string workItemType)
        {
            var isWorkItemTypesSeeded = dbContext.WorkItemTypes.Any(x => x.Type == workItemType);

            if (!isWorkItemTypesSeeded)
            {
                await dbContext.WorkItemTypes.AddAsync(new WorkItemType
                {
                    AddedOn = DateTime.UtcNow,
                    Type = workItemType,
                });
            }
        }
    }
}

using Data.Models;
using Shared.Constants.Seeding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Seeding
{
    public class TeamRolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {

            var roles = new List<string>()
            {
                TeamRolesConstants.BackendDeveloper,
                TeamRolesConstants.FrontendDeveloper,
                TeamRolesConstants.FullStackDeveloper,
                TeamRolesConstants.ProductOwner,
                TeamRolesConstants.ProjectManager,
                TeamRolesConstants.QAEngineer,
                TeamRolesConstants.QALead,
                TeamRolesConstants.ScrumMaster,
                TeamRolesConstants.TeamLead,
                TeamRolesConstants.TechLead,
                TeamRolesConstants.Tester,
                TeamRolesConstants.UIDesigner,
                TeamRolesConstants.UXDesigner,
            };

            foreach (var role in roles)
            {
                await SeedTeamRole(dbContext, role);
            }
        }

        public async Task SeedTeamRole(ApplicationDbContext dbContext, string roleName)
        {
            var isRoleSeeded = dbContext.TeamRoles.Any(x => x.Role == roleName);

            if (!isRoleSeeded)
            {
                await dbContext.TeamRoles.AddAsync(new TeamRole { AddedOn = DateTime.UtcNow, Role = roleName });
            }
        }
    }
}

using AutoMapper;
using Data.Models;
using DataModels.Models.WorkItems.Tests.Dtos;
using Microsoft.EntityFrameworkCore;
using Repo;
using Services.Projects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Services.WorkItems.Tests
{
    public class TestsService : ITestsService
    {
        private readonly IRepository<Test> repo;
        private readonly IProjectsService projectsService;

        public TestsService(IRepository<Test> repo, IMapper mapper,IProjectsService projectsService)
        {
            this.repo = repo;
            this.projectsService = projectsService;
        }

        public async Task ChangeColumnAsync(int itemId, int columnId)
        {
            var testToMove = await this.repo.All()
               .Where(x => x.Id == itemId)
               .FirstOrDefaultAsync();

            testToMove.KanbanBoardColumnId = columnId;
            await this.repo.SaveChangesAsync();
        }

        public async Task CreateAsync(int projectId, TestInputModelDto inputModel)
        {
            var nextId = await this.projectsService.GetNextIdForWorkItemAsync(projectId);

            var testToCreate = new Test()
            {
                AddedOn = DateTime.UtcNow,
                AcceptanceCriteria = inputModel.AcceptanceCriteria,
                Description = inputModel.Description,
                UserStoryId = inputModel.UserStoryId,
                Title = inputModel.Title,
                IdForProject = nextId
            };

            await this.repo.AddAsync(testToCreate);
            await this.repo.SaveChangesAsync();
        }

        public async Task DeleteAsync(int testId)
        {
            var toRemove = await this.repo.AllAsNoTracking()
                .Where(x => x.Id == testId)
                .FirstOrDefaultAsync();

            this.repo.Delete(toRemove);
            await this.repo.SaveChangesAsync();
        }
    }
}

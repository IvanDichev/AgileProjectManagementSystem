using AutoMapper;
using Data.Models;
using DataModels.Models.WorkItems.Tests.Dtos;
using Microsoft.EntityFrameworkCore;
using Repo;
using Services.BoardColumns;
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
        private readonly IRepository<UserStory> userStoryRepo;
        private readonly IBoardsService boardService;

        public TestsService(IRepository<Test> repo,
            IProjectsService projectsService,
            IRepository<UserStory> userStoryRepo,
            IBoardsService boardService)
        {
            this.repo = repo;
            this.projectsService = projectsService;
            this.userStoryRepo = userStoryRepo;
            this.boardService = boardService;
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
            var sprintId = await userStoryRepo.AllAsNoTracking()
                .Where(x => x.Id == inputModel.UserStoryId)
                .Select(x => x.SprintId)
                .FirstOrDefaultAsync() ?? default;

            var columnId = (await this.boardService.GetAllColumnsAsync(projectId, sprintId)).FirstOrDefault().Id;

            var testToCreate = new Test()
            {
                AddedOn = DateTime.UtcNow,
                AcceptanceCriteria = inputModel.AcceptanceCriteria,
                Description = inputModel.Description,
                UserStoryId = inputModel.UserStoryId,
                Title = inputModel.Title,
                IdForProject = nextId,
                KanbanBoardColumnId = columnId,
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

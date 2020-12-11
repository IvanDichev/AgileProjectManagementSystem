using Data.Models;
using DataModels.Models.WorkItems.Tasks.Dtos;
using Microsoft.EntityFrameworkCore;
using Repo;
using Services.BoardColumns;
using Services.Projects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Services.WorkItems.Tasks
{
    public class TasksService : ITasksService
    {
        private readonly IRepository<UserStoryTask> taskRepo;
        private readonly IProjectsService projectsService;
        private readonly IBoardsService boardService;
        private readonly IRepository<UserStory> userStoryRepo;

        public TasksService(IRepository<UserStoryTask> repo, 
            IProjectsService projectsService, 
            IBoardsService boardService,
            IRepository<UserStory> userStoryRepo)
        {
            this.taskRepo = repo;
            this.projectsService = projectsService;
            this.boardService = boardService;
            this.userStoryRepo = userStoryRepo;
        }

        public async Task ChangeColumnAsync(int itemId, int columnId)
        {
            var taskToMove = await this.taskRepo.All()
               .Where(x => x.Id == itemId)
               .FirstOrDefaultAsync();

            taskToMove.KanbanBoardColumnId = columnId;
            await this.taskRepo.SaveChangesAsync();
        }

        public async Task CreateAsync(TaskInputModelDto inputModelDto, int projectId)
        {
            var nextId = await this.projectsService.GetNextIdForWorkItemAsync(projectId);
            var sprintId = await userStoryRepo.AllAsNoTracking()
                .Where(x => x.Id == inputModelDto.UserStoryId)
                .Select(x => x.SprintId)
                .FirstOrDefaultAsync() ?? default;

            var columnId = (await this.boardService.GetAllColumnsAsync(projectId, sprintId)).FirstOrDefault().Id;

            var taskToCreate = new UserStoryTask()
            {
                AddedOn = DateTime.UtcNow,
                IdForProject = nextId,
                Description = inputModelDto.Description,
                Title = inputModelDto.Title,
                UserId = inputModelDto.UserId,
                UserStoryId = inputModelDto.UserStoryId,
                AcceptanceCriteria = inputModelDto.AcceptanceCriteria,
                KanbanBoardColumnId = columnId,
            };

            await this.taskRepo.AddAsync(taskToCreate);
            await this.taskRepo.SaveChangesAsync();
        }

        public async Task DeleteAsync(int taskId)
        {
            var toRemove = await this.taskRepo.All().Where(x => x.Id == taskId).FirstOrDefaultAsync();

            this.taskRepo.Delete(toRemove);
            await this.taskRepo.SaveChangesAsync();
        }
    }
}

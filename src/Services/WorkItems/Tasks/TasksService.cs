using Data.Models;
using DataModels.Models.WorkItems.Tasks.Dtos;
using Repo;
using Services.Projects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Services.WorkItems.Tasks
{
    public class TasksService : ITasksService
    {
        private readonly IRepository<UserStoryTask> repo;
        private readonly IProjectsService projectsService;

        public TasksService(IRepository<UserStoryTask> repo, IProjectsService projectsService)
        {
            this.repo = repo;
            this.projectsService = projectsService;
        }

        public async Task CreateAsync(TaskInputModelDto inputModelDto, int projectId)
        {
            var nextId = await this.projectsService.GetNextIdForWorkItemAsync(projectId);

            var taskToCreate = new UserStoryTask()
            {
                AddedOn = DateTime.UtcNow,
                IdForProject = nextId,
                Description = inputModelDto.Description,
                Title = inputModelDto.Title,
                UserId = inputModelDto.UserId,
                UserStoryId = inputModelDto.UserStoryId,
                AcceptanceCriteria = inputModelDto.AcceptanceCriteria,
            };

            await this.repo.AddAsync(taskToCreate);
            await this.repo.SaveChangesAsync();
        }
    }
}

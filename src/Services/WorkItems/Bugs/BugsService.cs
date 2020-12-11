using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Models;
using DataModels.Models.Severity;
using DataModels.Models.WorkItems.Bugs.Dtos;
using Microsoft.EntityFrameworkCore;
using Repo;
using Services.BoardColumns;
using Services.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.WorkItems.Bugs
{
    public class BugsService : IBugsService
    {
        private readonly IRepository<Bug> bugsRepo;
        private readonly IRepository<Severity> severityRepo;
        private readonly IMapper mapper;
        private readonly IProjectsService projectsService;
        private readonly IRepository<UserStory> userStoryRepo;
        private readonly IBoardsService boardService;

        public BugsService(IRepository<Bug> bugsRepo, 
            IRepository<Severity> severityRepo, 
            IMapper mapper, 
            IProjectsService projectsService,
            IRepository<UserStory> userStoryRepo,
            IBoardsService boardService)
        {
            this.bugsRepo = bugsRepo;
            this.severityRepo = severityRepo;
            this.mapper = mapper;
            this.projectsService = projectsService;
            this.userStoryRepo = userStoryRepo;
            this.boardService = boardService;
        }

        public async Task CreateBugAsync(int projectId, BugInputModelDto inputModel)
        {
            var nextId = await this.projectsService.GetNextIdForWorkItemAsync(projectId);
            var sprintId = await userStoryRepo.AllAsNoTracking()
                .Where(x => x.Id == inputModel.UserStoryId)
                .Select(x => x.SprintId)
                .FirstOrDefaultAsync() ?? default;

            var columnId = (await this.boardService.GetAllColumnsAsync(projectId, sprintId)).FirstOrDefault().Id;

            var toCreate = new Bug()
            {
                AcceptanceCriteria = inputModel.AcceptanceCriteria,
                Description = inputModel.Description,
                AddedOn = DateTime.UtcNow,
                SeverityId = inputModel.SeverityId,
                Title = inputModel.Title,
                UserStoryId = inputModel.UserStoryId,
                IdForProject = nextId,
                KanbanBoardColumnId = columnId,
            };

            await this.bugsRepo.AddAsync(toCreate);
            await this.bugsRepo.SaveChangesAsync();
        }

        public async Task DeleteAsync(int bugId)
        {
            var toRemove = await this.bugsRepo.All().Where(x => x.Id == bugId).FirstOrDefaultAsync();

            this.bugsRepo.Delete(toRemove);
            await this.bugsRepo.SaveChangesAsync();
        }

        public async Task<ICollection<SeverityDropDownModel>> GetSeverityDropDown()
        {
            var severityDropDown = await this.severityRepo
                .AllAsNoTracking()
                .ProjectTo<SeverityDropDownModel>(mapper.ConfigurationProvider)
                .ToListAsync();

            return severityDropDown;
        }
    }
}

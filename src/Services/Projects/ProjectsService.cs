﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Models;
using DataModels.Models.Projects;
using DataModels.Models.Projects.Dtos;
using DataModels.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repo;
using Shared.Constants;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Projects
{
    public class ProjectsService : IProjectsService
    {
        private readonly IRepository<Project> repo;
        private readonly IMapper mapper;
        private readonly ILogger<ProjectsService> logger;

        public ProjectsService(IRepository<Project> repo, IMapper mapper, ILogger<ProjectsService> logger)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<ProjectDto> GetAsync(int id)
        {
            var project = await this.repo.AllAsNoTracking()
                .Where(x => x.Id == id)
                .ProjectTo<ProjectDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return project;
        }

        public async Task<PaginatedProjectDto> GetAllAsync(int userId, PaginationFilter paginationFilter)
        {
            var query = this.repo.All();
            var allForUser = query.Where(x => x.Team.ProjectId == x.Id)
                .Where(p => p.Team.TeamsUsers.Any(x => x.TeamId == p.Team.Id && x.UserId == userId));

            var paginatedQuery = allForUser
                .OrderBy(x => x.Id)
                .Skip(paginationFilter.PageSize * (paginationFilter.PageNumber - 1))
                .Take(paginationFilter.PageSize);

            var paginatedResult = new PaginatedProjectDto()
            {
                PaginatedProjects = await paginatedQuery.ProjectTo<ProjectDto>(this.mapper.ConfigurationProvider).ToListAsync(),
                RecordsPerPage = paginationFilter.PageSize,
                TotalPages = (int)Math.Ceiling(await allForUser.CountAsync(x => x.Id == x.Id) / (double)paginationFilter.PageSize),
            };

            return paginatedResult;
        }

        public async Task CreateAsync(CreateProjectInputModel inputModel, int userId)
        {
            var project = new Project()
            {
                AddedOn = DateTime.UtcNow,
                Name = inputModel.Name,
                Description = inputModel.Description,
            };

            project.Team = new Team
            {
                AddedOn = DateTime.UtcNow,
                Name = project.Name + " Team"
            };

            project.Team.TeamsUsers.Add(new TeamsUsers()
            {
                UserId = userId,
                TeamId = project.Team.Id,
            });

            AddDefaultKanbanBoardToProject(project);

            await this.repo.AddAsync(project);
            await repo.SaveChangesAsync();

            var projectId = this.repo.All().FirstOrDefault(x => x.Name == project.Name).Id;
            
        }

        private static void AddDefaultKanbanBoardToProject(Project project)
        {
            var defaultBacklogColumnOptions = new KanbanBoardColumnOption()
            {
                AddedOn = DateTime.UtcNow,
                ColumnName = DefaultKanbanOptionsConstants.Backlog,
                MaxItems = DefaultKanbanOptionsConstants.BacklogMaxItems,
                PositionLTR = DefaultKanbanOptionsConstants.BacklogPosition,
            };

            var defaultDoingColumnOptions = new KanbanBoardColumnOption()
            {
                AddedOn = DateTime.UtcNow,
                ColumnName = DefaultKanbanOptionsConstants.Doing,
                MaxItems = DefaultKanbanOptionsConstants.DoingMaxItems,
                PositionLTR = DefaultKanbanOptionsConstants.DoingPosition,
            };

            var defaultDoneColumnOptions = new KanbanBoardColumnOption()
            {
                AddedOn = DateTime.UtcNow,
                ColumnName = DefaultKanbanOptionsConstants.Done,
                MaxItems = DefaultKanbanOptionsConstants.DoneMaxItems,
                PositionLTR = DefaultKanbanOptionsConstants.DonePosition,
            };

            var BacklogkanbanBoardColumn = new KanbanBoardColumn()
            {
                AddedOn = DateTime.UtcNow,
                KanbanBoardColumnOption = defaultBacklogColumnOptions,
            };

            var DoingkanbanBoardColumn = new KanbanBoardColumn()
            {
                AddedOn = DateTime.UtcNow,
                KanbanBoardColumnOption = defaultDoingColumnOptions,
            };
            
            var DonekanbanBoardColumn = new KanbanBoardColumn()
            {
                AddedOn = DateTime.UtcNow,
                KanbanBoardColumnOption = defaultDoneColumnOptions,
            };

            project.KanbanBoardColumns.Add(BacklogkanbanBoardColumn);
            project.KanbanBoardColumns.Add(DonekanbanBoardColumn);
            project.KanbanBoardColumns.Add(DoingkanbanBoardColumn);
        }

        public bool IsNameTaken(string name)
        {
            return this.repo.AllAsNoTracking()
                .Any(x => x.Name == name);
        }

        public async Task DeleteAsync(int id)
        {
            var toRemove = await this.repo.All()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            this.repo.Delete(toRemove);
            await this.repo.SaveChangesAsync();
        }

        public async Task UpdateAsync(EditProjectInputModel editModel)
        {
            var project = await this.repo.All()
                .Where(x => x.Id == editModel.ProjectId)
                .FirstOrDefaultAsync();

            project.Description = editModel.Description;
            project.ModifiedOn = DateTime.UtcNow;

            this.repo.Update(project);

            await this.repo.SaveChangesAsync();
        }

        public bool IsUserInProject(int projectId, int userId)
        {
            return this.repo.AllAsNoTracking()
                .Where(x => x.Id == projectId)
                .Any(x => x.Team.TeamsUsers
                    .Any(x => x.UserId == userId));
        }

        public async Task<int> GetNextIdForWorkItemAsync(int projectId)
        {
            var project = this.repo.All()
                .Where(x => x.Id == projectId)
                .FirstOrDefault();

            project.WorkItemsId += 1;
            this.repo.Update(project);
            await this.repo.SaveChangesAsync();
            return project.WorkItemsId;
        }
    }
}

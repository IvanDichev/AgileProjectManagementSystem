using AutoMapper;
using Data.Models;
using DataModels.Models.Projects;
using DataModels.Models.Projects.Dtos;
using DataModels.Models.UserStories;
using DataModels.Pagination;
using Microsoft.EntityFrameworkCore;
using Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Projects
{
    public class ProjectsService : IProjectsService
    {
        private readonly IRepository<Project> repo;
        private readonly IMapper mapper;

        public ProjectsService(IRepository<Project> repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<int> CreateAsync(CreateProjectInputModel inputModel, int userId)
        {
            var project = new Project
            {
                AddedOn = DateTime.UtcNow,
                Name = inputModel.Name,
                Description = inputModel.Description,
                Team = new Team()
                {
                    Name = inputModel.Name + " Team",
                    AddedOn = DateTime.UtcNow,
                }
            };

            await this.repo.AddAsync(project);

            project.Team.TeamsUsers.Add(new TeamsUsers() { UserId = userId, TeamId = project.Team.Id });

            await repo.SaveChangesAsync();

            var createdProject = await this.repo.AllAsNoTracking()
                .Where(x => x.Name == inputModel.Name)
                .FirstOrDefaultAsync();

            return createdProject.Id;
        }

        public bool IsNameTaken(string name)
        {
            return this.repo.AllAsNoTracking()
                .Any(x => x.Name == name);
        }

        public async Task<PaginatedProjectViewModel> GetAllAsync(int userId, PaginationFilter paginationFilter)
        {
            var query = this.repo.All();
            var all = query.Where(x => x.Team.ProjectId == x.Id)
                .Where(p => p.Team.TeamsUsers.Any(x => x.TeamId == p.Team.Id && x.UserId == userId));

            var filter = all
                .OrderBy(x => x.Id)
                .Skip(paginationFilter.PageSize * (paginationFilter.PageNumber - 1))
                .Take(paginationFilter.PageSize);

            var paginatedResult = new PaginatedProjectViewModel()
            {
                AllProjects = this.mapper.Map<ICollection<ProjectViewModel>>(filter),
                RecordsPerPage = paginationFilter.PageSize,
                TotalPages = (int)Math.Ceiling(await all.CountAsync(x => x.Id == x.Id) / (double)paginationFilter.PageSize)
            };

            var a = this.mapper.Map<IEnumerable<ProjectDto>>(filter);
            return paginatedResult;
        }

        public async Task<int> GetAllPagesAsync(int userId)
        {
            var query = this.repo.All();
            var all = await query.Where(x => x.Team.ProjectId == x.Id)
                .Where(p => p.Team.TeamsUsers.Any(x => x.TeamId == p.Team.Id && x.UserId == userId))
                .ToListAsync();

            return all.Count;
        }

        public async Task<ProjectDto> GetAsync(int id)
        {
            return mapper.Map<ProjectDto>(await this.repo.AllAsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync());
        }

        public async Task DeleteAsync(int id)
        {
            var toRemove = await this.repo.All()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            this.repo.Delete(toRemove);

            await this.repo.SaveChangesAsync();
        }

        public async Task EditAsync(EditProjectViewModel editModel)
        {
            var project = await this.repo.All()
                .Where(x => x.Id == editModel.ProjectId)
                .FirstOrDefaultAsync();

            project.Description = editModel.Description;
            project.ModifiedOn = DateTime.UtcNow;

            var p = mapper.Map<Project>(project);
            this.repo.Update(p);

            await this.repo.SaveChangesAsync();
        }

        public bool IsUserInProject(int projectId, int userId)
        {
            return this.repo.AllAsNoTracking()
                .Where(x => x.Id == projectId)
                .Any(x => x.Team.TeamsUsers
                    .Any(x => x.UserId == userId));
        }
    }
}

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Models;
using DataModels.Models.Projects;
using DataModels.Models.Projects.Dtos;
using DataModels.Pagination;
using Microsoft.EntityFrameworkCore;
using Repo;
using System;
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
            var project = this.mapper.Map<Project>(inputModel);
            await this.repo.AddAsync(project);
            project.Team.TeamsUsers.Add(new TeamsUsers() 
            { 
                UserId = userId, TeamId = project.Team.Id 
            });
            await repo.SaveChangesAsync();

            return await this.repo.AllAsNoTracking()
                .Where(x => x.Name == inputModel.Name)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
        }

        public bool IsNameTaken(string name)
        {
            return this.repo.AllAsNoTracking()
                .Any(x => x.Name == name);
        }

        public async Task<PaginatedProjectDto> GetAllAsync(int userId, PaginationFilter paginationFilter)
        {
            var query = this.repo.All();
            var allForUser = query.Where(x => x.Team.ProjectId == x.Id)
                .Where(p => p.Team.TeamsUsers.Any(x => x.TeamId == p.Team.Id && x.UserId == userId));

            var filter = allForUser
                .OrderBy(x => x.Id)
                .Skip(paginationFilter.PageSize * (paginationFilter.PageNumber - 1))
                .Take(paginationFilter.PageSize);

            var paginatedResult = new PaginatedProjectDto()
            {
                AllProjects = await filter.ProjectTo<ProjectDto>(this.mapper.ConfigurationProvider).ToListAsync(), 
                RecordsPerPage = paginationFilter.PageSize,
                TotalPages = (int)Math.Ceiling(await allForUser.CountAsync(x => x.Id == x.Id) / (double)paginationFilter.PageSize)
            };

            return paginatedResult;
        }

        public async Task<int> GetAllPagesAsync(int userId)
        {
            return await this.repo
                .AllAsNoTracking()
                .Where(x => x.Team.ProjectId == x.Id)
                .Where(p => p.Team.TeamsUsers.Any(x => x.TeamId == p.Team.Id && x.UserId == userId))
                .CountAsync();
        }

        public async Task<ProjectDto> GetAsync(int id)
        {
            return await this.repo.AllAsNoTracking()
                .Where(x => x.Id == id)
                .ProjectTo<ProjectDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var toRemove = await this.repo.All()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            this.repo.Delete(toRemove);

            await this.repo.SaveChangesAsync();
        }

        public async Task EditAsync(EditProjectInputModel editModel)
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
    }
}

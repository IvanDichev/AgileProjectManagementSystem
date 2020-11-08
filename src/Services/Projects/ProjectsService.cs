using AutoMapper;
using Data.Models;
using DataModels.Models.Projects;
using DataModels.Models.Projects.Dtos;
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

            project.Team.TeamsUsers.Add(new TeamsUsers() {UserId = userId, TeamId = project.Team.Id });

            await repo.SaveChangesAsync();

            return this.repo.AllAsNoTracking().Where(x => x.Name == inputModel.Name).FirstOrDefault().Id;
        }

        public bool IsNameTaken(string name)
        {
            return this.repo.AllAsNoTracking().Any(x => x.Name == name);
        }

        public IEnumerable<ProjectDto> GetAll(int userId)
        {
            var all = this.repo.All().Where(x => x.Team.ProjectId == x.Id)
                .Where(x => x.Team.TeamsUsers.FirstOrDefault().TeamId == x.Team.Id &&
                    x.Team.TeamsUsers.FirstOrDefault().UserId == userId);

            return mapper.Map<IEnumerable<ProjectDto>>(all.ToList());
        }

        public ProjectDto Get(int id)
        {
            return mapper.Map<ProjectDto>(this.repo.AllAsNoTracking().Where(x => x.Id == id).FirstOrDefault());
        }

        public async Task Delete(int id)
        {
            var toRemove = this.repo.All().Where(x => x.Id == id).FirstOrDefault();
            this.repo.Delete(toRemove);
            await this.repo.SaveChangesAsync();
        }
    }
}

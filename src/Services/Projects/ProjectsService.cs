using AutoMapper;
using Data.Models;
using DataModels.Dtos;
using DataModels.Models.Project;
using Repo;
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
        public Task Create(CreateProjectInputModel inputModel)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ProjectDto> GetAll()
        {
            return mapper.Map<IEnumerable<ProjectDto>>(this.repo.All().ToList());
        }

        public Task<ProjectDto> GetAsync(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}

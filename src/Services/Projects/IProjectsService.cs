using DataModels.Dtos;
using DataModels.Models.Project;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Projects
{
    public interface IProjectsService
    {
        Task<ProjectDto> GetAsync(int id);
        //ICollection<ProjectDto> GetAll();
        IEnumerable<ProjectDto> GetAll();
        Task Create(CreateProjectInputModel inputModel);
    }
}

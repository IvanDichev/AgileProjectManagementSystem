using DataModels.Dtos;
using DataModels.Models.Project;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Projects
{
    public interface IProjectsService
    {
        bool IsNameTaken(string name);
        Task<ProjectDto> GetAsync(int id);
        IEnumerable<ProjectDto> GetAll();
        Task<int> CreateAsync(CreateProjectInputModel inputModel);
    }
}

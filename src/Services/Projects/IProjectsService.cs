using DataModels.Models.Project;
using DataModels.Models.Project.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Projects
{
    public interface IProjectsService
    {
        bool IsNameTaken(string name);
        ProjectDto Get(int id);
        IEnumerable<ProjectDto> GetAll();
        Task<int> CreateAsync(CreateProjectInputModel inputModel);
    }
}

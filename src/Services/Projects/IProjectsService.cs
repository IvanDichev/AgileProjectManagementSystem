using DataModels.Models.Projects;
using DataModels.Models.Projects.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Projects
{
    public interface IProjectsService
    {
        bool IsNameTaken(string name);
        ProjectDto Get(int id);
        IEnumerable<ProjectDto> GetAll(int userId);
        Task<int> CreateAsync(CreateProjectInputModel inputModel, int userId);
        Task Delete(int id);
        Task Edit(EditProjectViewModel editModel);
    }
}

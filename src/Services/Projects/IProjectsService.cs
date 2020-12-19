using DataModels.Models.Projects;
using DataModels.Models.Projects.Dtos;
using DataModels.Models.Users.Dtos;
using DataModels.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Projects
{
    public interface IProjectsService
    {
        bool IsNameTaken(string projectName);
        Task<ProjectDto> GetAsync(int id);
        Task<PaginatedProjectDto> GetAllAsync(int userId, PaginationFilter paginationFilter);
        Task CreateAsync(CreateProjectInputModel inputModel, int userId);
        Task DeleteAsync(int id);
        Task UpdateAsync(EditProjectInputModel editModel);

        /// <summary>
        /// Check if project has relation to the project.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        bool IsUserInProject(int projectId, int userId);
        public Task<int> GetNextIdForWorkItemAsync(int projectId);

        public Task<ICollection<UserDto>> GetUsersDropDown(int projectId);

        public Task AddUserToProject(int userId, int projectId);
        Task RemoveUserFromProjectAsync(int userId, int projectId);
        Task<bool> IsLastUserInProjectAsync(int userId, int projectId);
    }
}

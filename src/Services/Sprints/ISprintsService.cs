using DataModels.Models.Sprints;
using DataModels.Models.Sprints.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Sprints
{
    public interface ISprintsService
    {
        Task<ICollection<SprintDto>> GetAllForProjectAsync(int projectId);
        Task<ICollection<SprintDropDownModel>> GetSprintDropDownAsync(int projectId);
        Task<SprintDto> GetByIdAsync(int sprintId);
        Task CreateSprintAsync(SprintInputDto inputDto);
        Task DeleteAsync(int sprintId);
        Task<bool> AreUserStoriesInSprintAsync(int sprintId);
        Task UpdateSprintStatus();
        Task<ICollection<OldSprintsBurndownData>> GetOldSprintBurndownData(int projectId);
    }
}

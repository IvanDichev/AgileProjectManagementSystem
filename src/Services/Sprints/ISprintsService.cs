using DataModels.Models.Sprints.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Sprints
{
    public interface ISprintsService
    {
        Task<ICollection<SprintDto>> GetAllForProjectAsync(int projectId);
        Task<SprintDto> GetByIdAsync(int sprintId);
        Task CreateSprintAsync(SprintInputDto inputDto);
    }
}

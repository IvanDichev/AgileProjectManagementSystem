using DataModels.Models.Sorting;
using DataModels.Models.WorkItems;
using DataModels.Models.WorkItems.UserStory;
using DataModels.Models.WorkItems.UserStory.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.WorkItems.UserStories
{
    public interface IUserStoryService
    {
        Task<IEnumerable<UserStoryAllDto>> GetAllAsync(int projectId, SortingFilter sortingFilter);
        Task<UserStoryDto> GetAsync(int WorkItemId);
        Task CreateAsync(UserStoryInputDto model);
        Task DeleteAsync(int userStoryId);
        Task UpdateAsync(UserStoryUpdateModel updateModel);
        Task<ICollection<UserStoryDropDownModel>> GetUserStoryDropDownsAsync(int projectId);
        Task ChangeColumnAsync(int userStoryId, int columnId);
    }
}
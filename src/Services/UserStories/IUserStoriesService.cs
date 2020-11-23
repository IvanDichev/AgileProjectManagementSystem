using DataModels.Models.UserStories;
using DataModels.Models.UserStories.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.UserStories
{
    public interface IUserStoriesService
    {
        Task<IEnumerable<UserStoryDto>> GetAllAsync(int projectId);
        Task<UserStoryDto> GetAsync(int userStoryId);
        Task CreateAsync(UserStoryInputModel model);
        Task DeleteAsync(int userStoryId);
        Task UpdateAsync(UserStoryUpdateModel updateModel);
    }
}

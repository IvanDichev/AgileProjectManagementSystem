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
        Task CreateAsync(CreateUserStoryInputModel model);
        Task DeleteAsync(int userStoryId);
    }
}

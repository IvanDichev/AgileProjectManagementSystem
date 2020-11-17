using DataModels.Models.UserStories;
using DataModels.Models.UserStories.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.UserStories
{
    public interface IUserStoriesService
    {
        IEnumerable<UserStoryDto> GetAll(int projectId);
        UserStoryDto Get(int userStoryId);
        bool IsUserInProject(int projectId, int userId);
        Task<int> CreateAsync(CreateUserStoryInputModel model);
    }
}

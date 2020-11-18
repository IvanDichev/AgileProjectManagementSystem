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
        Task CreateAsync(CreateUserStoryInputModel model);
        Task Delete(int userStoryId);
    }
}

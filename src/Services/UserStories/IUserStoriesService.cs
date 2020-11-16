using DataModels.Models.UserStories.Dtos;
using System.Collections.Generic;

namespace Services.UserStories
{
    public interface IUserStoriesService
    {
        IEnumerable<UserStoryDto> GetAll(int projectId);
        bool IsUserInProject(int projectId, int userId);
    }
}

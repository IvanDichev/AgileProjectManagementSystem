using System.Collections.Generic;

namespace DataModels.Models.UserStories.Dtos
{
    public class SortedPaginatedUserStoryDto
    {
        public ICollection<UserStoryAllDto> UserStoryViewModel { get; set; }
    }
}

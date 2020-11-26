﻿using DataModels.Models.Sorting;
using DataModels.Models.WorkItems;
using DataModels.Models.WorkItems.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.UserStories
{
    public interface IWorkItemService
    {
        Task<IEnumerable<WokrItemAllDto>> GetAllAsync(int projectId, SortingFilter sortingFilter);
        Task<WorkItemDto> GetAsync(int WorkItemId);
        Task CreateAsync(WorkItemInputModel model);
        Task DeleteAsync(int userStoryId);
        Task UpdateAsync(WorkItemUpdateModel updateModel);
    }
}

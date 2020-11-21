﻿using DataModels.Models.Projects;
using DataModels.Models.Projects.Dtos;
using DataModels.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Projects
{
    public interface IProjectsService
    {
        bool IsNameTaken(string name);
        Task<ProjectDto> GetAsync(int id);
        Task<PaginatedProjectViewModel> GetAllAsync(int userId, PaginationFilter paginationFilter);
        Task<int> CreateAsync(CreateProjectInputModel inputModel, int userId);
        Task DeleteAsync(int id);
        Task EditAsync(EditProjectViewModel editModel);

        /// <summary>
        /// Check if project has relation to the project.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        bool IsUserInProject(int projectId, int userId);
    }
}

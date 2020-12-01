﻿using DataModels.Models.WorkItems.Tasks.Dtos;
using System.Threading.Tasks;

namespace Services.WorkItems.Tasks
{
    public interface ITasksService
    {
        Task CreateAsync(TaskInputModelDto inputModelDto, int projectId);
    }
}

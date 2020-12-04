using AutoMapper;
using DataModels.Models.Sprints;
using DataModels.Models.Sprints.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Projects;
using Services.Sprints;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Authorize]
    public class SprintsController : BaseController
    {
        private readonly ISprintsService sprintsService;
        private readonly IMapper mapper;

        public SprintsController(ISprintsService sprintsService,
            IProjectsService projectsService,
            IMapper mapper)
            : base(projectsService)
        {
            this.sprintsService = sprintsService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> All(int projectId)
        {
            if (!this.IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            var sprintsViewModel = this.mapper.Map<ICollection<SprintAllViewModel>>
                (await this.sprintsService.GetAllForProjectAsync(projectId));

            return View(sprintsViewModel);
        }

        public IActionResult Create(int projectId)
        {
            if (!this.IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int projectId, SprintInputModel inputModel)
        {
            if (!this.IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            try
            {
                if (DateTime.Compare(inputModel.ParsedStartDate, inputModel.ParsedDueDate) > 0)
                {
                    this.ModelState.AddModelError(string.Empty, "Start date cannot be earlier than end date.");

                    return View(inputModel);
                }

                var inputDto = this.mapper.Map<SprintInputDto>(inputModel);
                inputDto.ProjectId = projectId;
                inputDto.StartDate = inputModel.ParsedStartDate;
                inputDto.DueDate = inputModel.ParsedDueDate;

                await this.sprintsService.CreateSprintAsync(inputDto);
                TempData["SprintSuccess"] = $"Successfully created sprint: {inputModel.Name}.";

                return RedirectToAction(nameof(All), new { projectId = projectId });
            }
            catch
            {
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int projectId, int sprintId, string sprintName)
        {
            //TODO enable delete only if team member is project manager or scrum master
            if (!this.IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            if (await this.sprintsService.AreUserStoriesInSprintAsync(sprintId))
            {
                try
                {
                    await this.sprintsService.DeleteAsync(sprintId);
                    TempData["SprintSuccess"] = $"Sprint {sprintName} was successfully deleted.";

                    return RedirectToAction(nameof(All), new { projectId = projectId });
                }
                catch { }
            }

            TempData["SprintDeleteError"] = "Sprint with assigned user stories cannot be deleted.";

            return RedirectToAction(nameof(All), new { projectId = projectId });
        }
    }
}

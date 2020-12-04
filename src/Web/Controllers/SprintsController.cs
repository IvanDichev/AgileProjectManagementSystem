using AutoMapper;
using DataModels.Models.Sprints;
using DataModels.Models.Sprints.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Projects;
using Services.Sprints;
using System;
using System.Collections.Generic;
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
            if(!this.IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int projectId, SprintInputModel inputModel)
        {
            if(!this.IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            try
            {
                if(DateTime.Compare(inputModel.StartDate, inputModel.DueDate) > 0)
                {
                    this.ModelState.AddModelError(string.Empty, "Start date cannot be earlier than end date.");

                    return View(inputModel);
                }

                var inputDto = this.mapper.Map<SprintInputDto>(inputModel);
                inputDto.ProjectId = projectId;

                await this.sprintsService.CreateSprintAsync(inputDto);

                return RedirectToAction(nameof(All), new { projectId = projectId });
            }
            catch
            {
                return RedirectToAction("Error", "Error");
            }
        }
    }
}

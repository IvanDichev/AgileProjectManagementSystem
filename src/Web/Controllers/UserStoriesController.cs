﻿using AutoMapper;
using DataModels.Models.UserStories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.BacklogPriorities;
using Services.Projects;
using Services.UserStories;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Authorize]
    [Route("Projects/{projectId}/[controller]/[action]")]
    public class UserStoriesController : BaseController
    {
        private readonly IUserStoriesService userStoriesService;
        private readonly IBacklogPrioritiesService backlogPrioritiesService;
        private readonly IProjectsService projectsService;
        private readonly IMapper mapper;

        public UserStoriesController(IUserStoriesService userStoriesService,
            IBacklogPrioritiesService backlogPrioritiesService,
            IProjectsService projectsService,
            IMapper mapper)
        {
            this.userStoriesService = userStoriesService;
            this.backlogPrioritiesService = backlogPrioritiesService;
            this.projectsService = projectsService;
            this.mapper = mapper;
        }

        [Route("", Name = "UserStoriesIndex")]
        public IActionResult Index(int projectId)
        {
            if (!IsUserInProject(projectId))
            {
                return Unauthorized();
            }

            var all = userStoriesService.GetAll(projectId);

            return View(all);
        }

        public async Task<IActionResult> Create(int projectId)
        {
            if (!IsUserInProject(projectId))
            {
                return Unauthorized();
            }

            var createViewModel = new CreateUserStoryInputModel()
            {
                PrioritiesDropDown = this.mapper.Map<ICollection<BacklogPriorityDropDownModel>>
                (await this.backlogPrioritiesService.GetAllAsync())
            };

            return View(createViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserStoryInputModel inputModel, int projectId)
        {
            if (!IsUserInProject(projectId))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (!this.userStoriesService.IsUserInProject(projectId, userId))
            {
                return Unauthorized();
            }

            await  this.userStoriesService.CreateAsync(inputModel);

            return RedirectToRoute("UserStoriesIndex");
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Check if project has relation to the project.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        private bool IsUserInProject(int projectId)
        {
            var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            return this.projectsService.IsUserInProject(projectId, userId);
        }
    }
}

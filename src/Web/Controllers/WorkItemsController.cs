using AutoMapper;
using DataModels.Models.Sorting;
using DataModels.Models.WorkItems;
using DataModels.Models.WorkItems.UserStory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.BacklogPriorities;
using Services.Projects;
using Services.WorkItems;
using Services.WorkItems.UserStories;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Authorize]
    public class WorkItemsController : BaseController
    {
        private readonly IWorkItemService workItemService;
        private readonly IBacklogPrioritiesService backlogPrioritiesService;
        private readonly IMapper mapper;
        private readonly IUserStoryService userStoryService;

        public WorkItemsController(IWorkItemService workItemService,
            IBacklogPrioritiesService backlogPrioritiesService,
            IProjectsService projectsService,
            IMapper mapper,
            IUserStoryService userStoryService) : base(projectsService)
        {
            this.workItemService = workItemService;
            this.backlogPrioritiesService = backlogPrioritiesService;
            this.mapper = mapper;
            this.userStoryService = userStoryService;
        }

        public async Task<IActionResult> GetAll(int projectId, SortingFilter sortingFilter)
        {
            if (!IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            var all = this.mapper.Map<IEnumerable<UserStoryAllViewmodel>>
                (await userStoryService.GetAllAsync(projectId, sortingFilter));

            return View(all);
        }

        public async Task<IActionResult> AddUserStory(int projectId)
        {
            if (!IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            var createViewModel = new UserStoryInputModel()
            {
                PrioritiesDropDown = this.mapper.Map<ICollection<BacklogPriorityDropDownModel>>
                    (await this.backlogPrioritiesService.GetAllAsync()),
            };

            return View(createViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserStory(UserStoryInputModel inputModel, int projectId)
        {
            if (!IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                inputModel.PrioritiesDropDown = this.mapper.Map<ICollection<BacklogPriorityDropDownModel>>
                    (await this.backlogPrioritiesService.GetAllAsync());
                return View(inputModel);
            }

            try
            {
                inputModel.ProjectId = projectId;
                await this.userStoryService.CreateAsync(inputModel);

                return RedirectToAction(nameof(GetAll), new { projectId = projectId });
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserStory(int projectId, int UserStoryId) // userStoryId
        {
            if (!IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            try
            {
                await this.userStoryService.DeleteAsync(UserStoryId);

                return RedirectToAction(nameof(GetAll), new { projectId = projectId });
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error");
            }
        }

        public async Task<IActionResult> GetUserStory(int projectId, int UserStoryId) // userStoryId
        {
            if (!IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            var workItem = new UserStoryUpdateViewModel()
            {
                PrioritiesDropDown = this.mapper.Map<ICollection<BacklogPriorityDropDownModel>>
                    (await this.backlogPrioritiesService.GetAllAsync()),
                ViewModel = this.mapper.Map<UserStoryViewModel>(await userStoryService.GetAsync(UserStoryId)),
            };

            if (workItem.ViewModel == null)
            {
                return NotFound();
            }

            return View(workItem);
        }

        [HttpPost]
        public async Task<IActionResult> GetUserStory(UserStoryUpdateViewModel model, int projectId)
        {
            if (!IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                // If model is not valid prioritiesDropDown will be null so we need do populated it.
                model.PrioritiesDropDown = this.mapper.Map<ICollection<BacklogPriorityDropDownModel>>
                    (await this.backlogPrioritiesService.GetAllAsync());

                return View(model);
            }

            var userStory = this.mapper.Map<UserStoryUpdateModel>(model.ViewModel);
            userStory.AcceptanceCriteria = model.ViewModel.SanitizedAcceptanceCriteria;
            userStory.Description = model.ViewModel.SanitizedDescription;

            if (!string.IsNullOrEmpty(model.Comment.Description))
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                userStory.Comment = model.Comment;
                userStory.Comment.UserStoryId = model.ViewModel.Id;
                userStory.Comment.AddedById = userId;
            }

            try
            {
                await this.userStoryService.UpdateAsync(userStory);
                return RedirectToAction(nameof(GetAll), new { projectId = projectId });
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error");
            }
        }
    }
}

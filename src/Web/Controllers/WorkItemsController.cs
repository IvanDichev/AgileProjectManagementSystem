using AutoMapper;
using DataModels.Models.Sorting;
using DataModels.Models.WorkItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.BacklogPriorities;
using Services.Projects;
using Services.UserStories;
using Services.WorkItemTypesSErvices;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Authorize]
    public class WorkItemsController : BaseController
    {
        private readonly IWorkItemService userStoriesService;
        private readonly IBacklogPrioritiesService backlogPrioritiesService;
        private readonly IWorkItemTypesService workItemTypesService;
        private readonly IMapper mapper;

        public WorkItemsController(IWorkItemService userStoriesService,
            IBacklogPrioritiesService backlogPrioritiesService,
            IProjectsService projectsService,
            IWorkItemTypesService workItemTypesService,
            IMapper mapper) : base(projectsService)
        {
            this.userStoriesService = userStoriesService;
            this.backlogPrioritiesService = backlogPrioritiesService;
            this.workItemTypesService = workItemTypesService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index(int projectId, SortingFilter sortingFilter)
        {
            if (!IsUserInProject(projectId))
            {
                return Unauthorized();
            }

            var all = this.mapper.Map<IEnumerable<WorkItemAllViewModel>>
                (await userStoriesService.GetAllAsync(projectId, sortingFilter));

            return View(all);
        }

        public async Task<IActionResult> Create(int projectId)
        {
            if (!IsUserInProject(projectId))
            {
                return Unauthorized();
            }

            var createViewModel = new WorkItemInputModel()
            {
                PrioritiesDropDown = this.mapper.Map<ICollection<BacklogPriorityDropDownModel>>
                    (await this.backlogPrioritiesService.GetAllAsync()),
                WorkItemTypesDropDown = this.mapper.Map<ICollection<WorkItemTypesDropDownModel>>
                    (await this.workItemTypesService.GetWorkItemTypesAsync())
            };

            return View(createViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(WorkItemInputModel inputModel, int projectId)
        {
            if (!IsUserInProject(projectId))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                inputModel.PrioritiesDropDown = this.mapper.Map<ICollection<BacklogPriorityDropDownModel>>
                    (await this.backlogPrioritiesService.GetAllAsync());
                inputModel.WorkItemTypesDropDown = this.mapper.Map<ICollection<WorkItemTypesDropDownModel>>
                    (await this.workItemTypesService.GetWorkItemTypesAsync());
                return View(inputModel);
            }

            await this.userStoriesService.CreateAsync(inputModel);
            return RedirectToAction("Index", new { projectId = projectId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int projectId, int userStoryId)
        {
            if (!IsUserInProject(projectId))
            {
                return Unauthorized();
            }

            await this.userStoriesService.DeleteAsync(userStoryId);

            return RedirectToAction("Index", new { projectId = projectId });
        }

        public async Task<IActionResult> Get(int projectId, int userStoryId)
        {
            if (!IsUserInProject(projectId))
            {
                return Unauthorized();
            }

            var userStory = new UpdateWorkItemViewModel()
            {
                PrioritiesDropDown = this.mapper.Map<ICollection<BacklogPriorityDropDownModel>>
                    (await this.backlogPrioritiesService.GetAllAsync()),
                ViewModel = this.mapper.Map<WorkItemViewModel>(await userStoriesService.GetAsync(userStoryId)),
                WorkItemTypesDropDown = this.mapper.Map<ICollection<WorkItemTypesDropDownModel>>
                    (await this.workItemTypesService.GetWorkItemTypesAsync()),
            };

            if (userStory.ViewModel == null)
            {
                return NotFound();
            }

            return View(userStory);
        }

        [HttpPost]
        public async Task<IActionResult> Get(UpdateWorkItemViewModel model, int projectId)
        {
            if (!IsUserInProject(projectId))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                // If model is not valid prioritiesDropDown will be null so we need do populated it.
                model.PrioritiesDropDown = this.mapper.Map<ICollection<BacklogPriorityDropDownModel>>
                    (await this.backlogPrioritiesService.GetAllAsync());
                model.WorkItemTypesDropDown = this.mapper.Map<ICollection<WorkItemTypesDropDownModel>>
                    (await this.workItemTypesService.GetWorkItemTypesAsync());

                return View(model);
            }

            var userStory = this.mapper.Map<WorkItemUpdateModel>(model.ViewModel);

            if (!string.IsNullOrWhiteSpace(model.Comment.Description))
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                userStory.Comment = model.Comment;
                userStory.Comment.WorkItemId = model.ViewModel.Id;
                userStory.Comment.AddedById = userId;
            }

            userStory.ProjectId = projectId;
            await this.userStoriesService.UpdateAsync(userStory);

            return RedirectToAction("Index", new { projectId = projectId });
        }
    }
}

using AutoMapper;
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
    public class UserStoriesController : BaseController
    {
        private readonly IUserStoriesService userStoriesService;
        private readonly IBacklogPrioritiesService backlogPrioritiesService;
        private readonly IProjectsService projectsService;
        private readonly IMapper mapper;

        public UserStoriesController(IUserStoriesService userStoriesService,
            IBacklogPrioritiesService backlogPrioritiesService,
            IProjectsService projectsService,
            IMapper mapper) : base(projectsService)
        {
            this.userStoriesService = userStoriesService;
            this.backlogPrioritiesService = backlogPrioritiesService;
            this.projectsService = projectsService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index(int projectId)
        {
            if (!IsUserInProject(projectId))
            {
                return Unauthorized();
            }

            var all = this.mapper.Map<IEnumerable<UserStoriesAllViewModel>>
                (await userStoriesService.GetAllAsync(projectId));

            return View(all);
        }

        public async Task<IActionResult> Create(int projectId)
        {
            if (!IsUserInProject(projectId))
            {
                return Unauthorized();
            }

            var createViewModel = new UserStoryInputModel()
            {
                PrioritiesDropDown = this.mapper.Map<ICollection<BacklogPriorityDropDownModel>>
                (await this.backlogPrioritiesService.GetAllAsync())
            };

            return View(createViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserStoryInputModel inputModel, int projectId)
        {
            if (!IsUserInProject(projectId))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                inputModel.PrioritiesDropDown = this.mapper.Map<ICollection<BacklogPriorityDropDownModel>>
                    (await this.backlogPrioritiesService.GetAllAsync());
                return View(inputModel);
            }

            await this.userStoriesService.CreateAsync(inputModel);
            return RedirectToAction("Index", new { projectId = projectId });
        }

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

            var userStory = new UpdateUserStoriesViewModel()
            {
                PrioritiesDropDown = this.mapper.Map<ICollection<BacklogPriorityDropDownModel>>
                    (await this.backlogPrioritiesService.GetAllAsync()),
                ViewModel = this.mapper.Map<UserStoryViewModel>(await userStoriesService.GetAsync(userStoryId))
            };

            return View(userStory);
        }

        [HttpPost]
        public async Task<IActionResult> Get(UpdateUserStoriesViewModel model, int projectId)
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

                return View(model);
            }

            var userStory = this.mapper.Map<UserStoryUpdateModel>(model.ViewModel);

            if (!string.IsNullOrWhiteSpace(model.Comment.Description))
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                userStory.Comment = model.Comment;
                userStory.Comment.UserStoryId = model.ViewModel.Id;
                userStory.Comment.AddedById = userId;
            }

            userStory.ProjectId = projectId;
            await this.userStoriesService.UpdateAsync(userStory);

            return RedirectToAction("Index", new { projectId = projectId });
        }
    }
}

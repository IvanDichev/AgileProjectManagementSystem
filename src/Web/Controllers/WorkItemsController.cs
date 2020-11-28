using AutoMapper;
using DataModels.Models.Sorting;
using DataModels.Models.WorkItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.BacklogPriorities;
using Services.Projects;
using Services.WorkItems;
using Services.WorkItemTypesServices;
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
        private readonly IWorkItemTypesService workItemTypesService;
        private readonly IMapper mapper;

        public WorkItemsController(IWorkItemService workItemService,
            IBacklogPrioritiesService backlogPrioritiesService,
            IProjectsService projectsService,
            IWorkItemTypesService workItemTypesService,
            IMapper mapper) : base(projectsService)
        {
            this.workItemService = workItemService;
            this.backlogPrioritiesService = backlogPrioritiesService;
            this.workItemTypesService = workItemTypesService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> GetAll(int projectId, SortingFilter sortingFilter)
        {
            if (!IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            var all = this.mapper.Map<IEnumerable<WorkItemAllViewModel>>
                (await workItemService.GetAllAsync(projectId, sortingFilter));

            return View(all);
        }

        public async Task<IActionResult> Create(int projectId)
        {
            if (!IsCurrentUserInProject(projectId))
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
            if (!IsCurrentUserInProject(projectId))
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

            try
            {
                await this.workItemService.CreateAsync(inputModel);

                return RedirectToAction(nameof(GetAll), new { projectId = projectId });
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int projectId, int workItemId)
        {
            if (!IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            try
            {
                await this.workItemService.DeleteAsync(workItemId);

                return RedirectToAction(nameof(GetAll), new { projectId = projectId });
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error");
            }
        }

        public async Task<IActionResult> Get(int projectId, int workItemId)
        {
            if (!IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            var workItem = new UpdateWorkItemViewModel()
            {
                PrioritiesDropDown = this.mapper.Map<ICollection<BacklogPriorityDropDownModel>>
                    (await this.backlogPrioritiesService.GetAllAsync()),
                WorkItemTypesDropDown = this.mapper.Map<ICollection<WorkItemTypesDropDownModel>>
                    (await this.workItemTypesService.GetWorkItemTypesAsync()),
                ViewModel = this.mapper.Map<WorkItemViewModel>(await workItemService.GetAsync(workItemId)),
            };

            if (workItem.ViewModel == null)
            {
                return NotFound();
            }

            return View(workItem);
        }

        [HttpPost]
        public async Task<IActionResult> Get(UpdateWorkItemViewModel model, int projectId)
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
                model.WorkItemTypesDropDown = this.mapper.Map<ICollection<WorkItemTypesDropDownModel>>
                    (await this.workItemTypesService.GetWorkItemTypesAsync());

                return View(model);
            }

            var workItem = this.mapper.Map<WorkItemUpdateModel>(model.ViewModel);
            workItem.AcceptanceCriteria = model.ViewModel.SanitizedAcceptanceCriteria;
            workItem.Description = model.ViewModel.SanitizedDescription;
            workItem.ProjectId = projectId;

            if (!string.IsNullOrEmpty(model.Comment.Description))
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                workItem.Comment = model.Comment;
                workItem.Comment.WorkItemId = model.ViewModel.Id;
                workItem.Comment.AddedById = userId;
            }

            try
            {
                await this.workItemService.UpdateAsync(workItem);
                return RedirectToAction(nameof(GetAll), new { projectId = projectId });
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error");
            }
        }
    }
}

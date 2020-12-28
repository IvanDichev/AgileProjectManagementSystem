using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DataModels.Models.Sorting;
using DataModels.Models.WorkItems;
using DataModels.Models.WorkItems.Bugs;
using DataModels.Models.WorkItems.Bugs.Dtos;
using DataModels.Models.WorkItems.Tasks;
using DataModels.Models.WorkItems.Tasks.Dtos;
using DataModels.Models.WorkItems.Tests;
using DataModels.Models.WorkItems.Tests.Dtos;
using DataModels.Models.WorkItems.UserStory;
using DataModels.Models.WorkItems.UserStory.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Services.BacklogPriorities;
using Services.Projects;
using Services.Sprints;
using Services.WorkItems;
using Services.WorkItems.Bugs;
using Services.WorkItems.Tasks;
using Services.WorkItems.Tests;
using Services.WorkItems.UserStories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly ITasksService tasksService;
        private readonly ITestsService testsService;
        private readonly IBugsService bugsService;
        private readonly ISprintsService sprintsService;
        private readonly IConfiguration config;

        public WorkItemsController(IWorkItemService workItemService,
            IBacklogPrioritiesService backlogPrioritiesService,
            IProjectsService projectsService,
            IMapper mapper,
            IUserStoryService userStoryService,
            ITasksService tasksService,
            ITestsService testsService,
            IBugsService bugsService,
            ISprintsService sprintsService,
            IConfiguration config) : base(projectsService)
        {
            this.workItemService = workItemService;
            this.backlogPrioritiesService = backlogPrioritiesService;
            this.mapper = mapper;
            this.userStoryService = userStoryService;
            this.tasksService = tasksService;
            this.testsService = testsService;
            this.bugsService = bugsService;
            this.sprintsService = sprintsService;
            this.config = config;
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
                var savedFilepath = string.Empty;
                if (inputModel.Mockup?.Length > 0)
                {
                    string filePath = Guid.NewGuid().ToString();
                    Account account = new Account(
                        config["Cloudinary:CloudName"],
                        config["Cloudinary:ApiKey"],
                        config["Cloudinary:ApiSecret"]
                        );

                    Cloudinary cloudinary = new Cloudinary(account);

                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(filePath + ".png", inputModel.Mockup.OpenReadStream()),
                        Overwrite = true,
                    };
                    var uploadResult = cloudinary.Upload(uploadParams);

                    savedFilepath = uploadResult.SecureUrl.AbsoluteUri;
                }

                inputModel.ProjectId = projectId;
                var inputDto = this.mapper.Map<UserStoryInputDto>(inputModel);
                inputDto.MockupPath = savedFilepath;
                await this.userStoryService.CreateAsync(inputDto);

                return RedirectToAction(nameof(GetAll), new { projectId = projectId });
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserStory(int projectId, int userStoryId)
        {
            if (!IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            try
            {
                var userstory = await this.userStoryService.GetAsync(userStoryId);

                Account account = new Account(
                    config["Cloudinary:CloudName"],
                    config["Cloudinary:ApiKey"],
                    config["Cloudinary:ApiSecret"]
                    );

                Cloudinary cloudinary = new Cloudinary(account);

                foreach (var mockup in userstory.Mockups)
                {
                    // Get public name of the file.
                    var deleteParams = new DeletionParams(mockup.MockUpPath.Split('/').Last().Split('.')[0]);
                    var deleteresult = cloudinary.Destroy(deleteParams);
                }

                await this.userStoryService.DeleteAsync(userStoryId);

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
                SprintDropDownModel = (await this.sprintsService.GetSprintDropDownAsync(projectId)),
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
                model.SprintDropDownModel = await this.sprintsService.GetSprintDropDownAsync(projectId);

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
                var savedFilepaths = new List<string>();
                if (model.ViewModel.MockupFiles?.Count > 0)
                {                   
                    Account account = new Account(
                        config["Cloudinary:CloudName"],
                        config["Cloudinary:ApiKey"],
                        config["Cloudinary:ApiSecret"]
                        );

                    Cloudinary cloudinary = new Cloudinary(account);
                    foreach (var file in model.ViewModel.MockupFiles)
                    {
                        string filePath = Guid.NewGuid().ToString();

                        var uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(filePath + ".png", file.OpenReadStream()),
                            Overwrite = true,
                        };
                        var uploadResult = cloudinary.Upload(uploadParams);

                        savedFilepaths.Add(uploadResult.SecureUrl.AbsoluteUri);
                    }
                }

                userStory.MockupPaths = savedFilepaths;
                await this.userStoryService.UpdateAsync(userStory);

                return RedirectToAction(nameof(GetAll), new { projectId = projectId });
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error");
            }
        }

        public async Task<IActionResult> AddTask(int projectId, int userStoryId)
        {
            var inputModel = new TaskInputModel()
            {
                UserStoryId = userStoryId,
                UserStoryDropDown = await this.userStoryService.GetUserStoryDropDownsAsync(projectId),
            };

            return View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(int projectId, TaskInputModel inputModel)
        {
            if (!IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                inputModel.UserStoryDropDown = await this.userStoryService.GetUserStoryDropDownsAsync(projectId);
                return View(inputModel);
            }

            try
            {
                var inputDto = this.mapper.Map<TaskInputModelDto>(inputModel);
                inputDto.Description = inputModel.SanitizedDescription;
                inputDto.AcceptanceCriteria = inputModel.SanitizedDescription;

                await this.tasksService.CreateAsync(inputDto, projectId);
                return RedirectToAction(nameof(GetAll), new { projectId = projectId });
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTask(int projectId, int taskId)
        {
            if (!IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            try
            {
                await this.tasksService.DeleteAsync(taskId);

                return RedirectToAction(nameof(GetAll), new { projectId = projectId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Error");
            }

        }

        public async Task<IActionResult> AddTest(int projectId, int userStoryId)
        {
            var testInputModel = new TestInputModel()
            {
                UserStoryId = userStoryId,
                UserStoryDropDown = await this.userStoryService.GetUserStoryDropDownsAsync(projectId),
            };

            return View(testInputModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddTest(int projectId, TestInputModel testInputModel)
        {
            if (!this.IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                testInputModel.UserStoryDropDown = await this.userStoryService.GetUserStoryDropDownsAsync(projectId);

                return View(testInputModel);
            }

            try
            {
                var inputDto = this.mapper.Map<TestInputModelDto>(testInputModel);
                await this.testsService.CreateAsync(projectId, inputDto);

                return RedirectToAction(nameof(GetAll), new { projectId = projectId });
            }
            catch
            {
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTest(int projectId, int testId)
        {
            if (!this.IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            try
            {
                await this.testsService.DeleteAsync(testId);

                return RedirectToAction(nameof(GetAll), new { projectId = projectId });
            }
            catch
            {
                return RedirectToAction("Error", "Error");
            }
        }

        public async Task<IActionResult> AddBug(int projectId, int userStoryId)
        {
            var inputModel = new BugInputModel()
            {
                UserStoryId = userStoryId,
                UserStoryDropDown = await this.userStoryService.GetUserStoryDropDownsAsync(projectId),
                SeverityDropDown = await this.bugsService.GetSeverityDropDown(),
            };

            return View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddBug(int projectId, BugInputModel inputModel)
        {
            if (!this.IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                inputModel.UserStoryDropDown = await this.userStoryService.GetUserStoryDropDownsAsync(projectId);
                inputModel.SeverityDropDown = await this.bugsService.GetSeverityDropDown();

                return View(inputModel);
            }

            try
            {
                var inputDto = this.mapper.Map<BugInputModelDto>(inputModel);
                await this.bugsService.CreateBugAsync(projectId, inputDto);

                return RedirectToAction(nameof(GetAll), new { projectId = projectId });
            }
            catch
            {
                return RedirectToAction("Error", "Error");
            }

        }

        [HttpPost]
        public async Task<IActionResult> DeleteBug(int projectId, int bugId)
        {
            if (!this.IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            try
            {
                await this.bugsService.DeleteAsync(bugId);

                return RedirectToAction(nameof(GetAll), new { projectId = projectId });
            }
            catch
            {
                return RedirectToAction("Error", "Error");
            }
        }
    }
}

using AutoMapper;
using DataModels.Models.Comments;
using DataModels.Models.Comments.Dtos;
using Microsoft.AspNetCore.Mvc;
using Services.Comments;
using Services.Projects;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class CommentsController : BaseController
    {
        private readonly ICommentsService commentsService;
        private readonly IMapper mapper;

        public CommentsController(IProjectsService projectsService, 
            ICommentsService commentsService,
            IMapper mapper)
            : base(projectsService)
        {
            this.commentsService = commentsService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Edit(int commentId)
        {
            var commentViewModel = this.mapper.Map<CommentViewModel>
                (await this.commentsService.Get(commentId));
                
            return View(commentViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int projectId, CommentInputModel model)
        {
            var uesrId = int.Parse(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var updateModel = this.mapper.Map<CommentsUpdateModel>(model);
            await this.commentsService.Update(updateModel);
            var updated = await this.commentsService.Get(model.Id);

            return RedirectToAction("Get", "UserStories", new { ProjectId = projectId, userStoryId = updated.UserStoryId });
        }
        
        public IActionResult Index()
        {
            return View();
        }

        private bool IsUsersComment(int commentId)
        {
            var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return this.commentsService.IsUsersComment(userId, commentId);
        }
    }
}

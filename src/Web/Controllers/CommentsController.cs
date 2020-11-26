using AutoMapper;
using DataModels.Models.Comments;
using DataModels.Models.Comments.Dtos;
using Microsoft.AspNetCore.Mvc;
using Services.Comments;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Extentions;
using Web.Helpers;

namespace Web.Controllers
{
    public class CommentsController : BaseController
    {
        private readonly ICommentsService commentsService;
        private readonly IMapper mapper;

        public CommentsController(ICommentsService commentsService,
            IMapper mapper)
        {
            this.commentsService = commentsService;
            this.mapper = mapper;
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(int commentId)
        {
            var commentViewModel = this.mapper.Map<CommentViewModel>
                (await this.commentsService.GetAsync(commentId));

            return Json(new { html = await this.RenderViewAsStringAsync("Edit", commentViewModel, true) });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int projectId, CommentInputModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            if(!IsUsersComment(model.Id))
            {
                return Unauthorized();
            }

            var updateModel = this.mapper.Map<CommentsUpdateModel>(model);
            await this.commentsService.UpdateAsync(updateModel);
            var updated = await this.commentsService.GetAsync(model.Id);

            return RedirectToAction("Get", "UserStories", new { ProjectId = projectId, userStoryId = updated.UserStoryId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int projectId, int commentId, int userStoryId)
        {
            if (!IsUsersComment(commentId))
            {
                return Unauthorized();
            }

            await this.commentsService.DeleteAsync(commentId);

            return RedirectToAction("Get", "UserStories", new { projectId = projectId, userStoryId = userStoryId });
        }
        
        private bool IsUsersComment(int commentId)
        {
            var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return this.commentsService.IsUsersComment(userId, commentId);
        }
    }
}

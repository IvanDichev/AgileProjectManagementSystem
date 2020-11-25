using DataModels.Models.Comments.Dtos;
using System.Threading.Tasks;

namespace Services.Comments
{
    public interface ICommentsService
    {
        Task<CommentDto> Get(int commentId); 
        Task Update(CommentsUpdateModel updateModel); 

        bool IsUsersComment(int userId, int commentId);
    }
}

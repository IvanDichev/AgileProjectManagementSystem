using DataModels.Models.Comments.Dtos;
using System.Threading.Tasks;

namespace Services.Comments
{
    public interface ICommentsService
    {
        Task<CommentDto> GetAsync(int commentId); 

        Task UpdateAsync(CommentsUpdateModel updateModel); 

        Task DeleteAsync(int commentId); 

        bool IsUsersComment(int userId, int commentId);
    }
}

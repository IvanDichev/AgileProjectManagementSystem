using DataModels.Models.Board.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.BoardColumns
{
    public interface IBoardColumnsService
    {
        Task<ICollection<BoardColumnAllDto>> GetAllColumnsAsync(int projectId);
    }
}

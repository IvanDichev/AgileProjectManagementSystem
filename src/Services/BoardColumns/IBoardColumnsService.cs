using DataModels.Models.Board;
using DataModels.Models.Board.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.BoardColumns
{
    public interface IBoardColumnsService
    {
        Task<ICollection<BoardColumnAllDto>> GetAllColumnsAsync(int projectId);
        Task<ICollection<BoardColumnAllNamePositionDto>> GetColumnsNamesPositionAsync(int projectId);
        Task AddcolumnToTheLeftAsync(BoardOptionsInputModel inputModel);
    }
}

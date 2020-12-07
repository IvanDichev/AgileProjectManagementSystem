using DataModels.Models.Board;
using DataModels.Models.Board.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.BoardColumns
{
    public interface IBoardService
    {
        Task<ICollection<BoardColumnAllDto>> GetAllColumnsAsync(int projectId);
        Task<ICollection<BoardColumnAllDto>> GetAllColumnsForSprintAndProjectAsync(int projectId, int sprintId);
        Task<ICollection<BoardColumnAllNamePositionDto>> GetColumnsNamesPositionAsync(int projectId);
        Task AddcolumnToTheLeftAsync(BoardOptionsInputModel inputModel);
    }
}

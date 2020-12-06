using DataModels.Models.WorkItems.UserStory.Dtos;
using System.Collections.Generic;

namespace DataModels.Models.Board.Dtos
{
    public class BoardColumnAllDto
    {
        public int Id { get; set; }

        public string KanbanBoardColumnOptionColumnName { get; set; }

        public ushort KanbanBoardColumnOptionMaxItems { get; set; }

        public byte KanbanBoardColumnOptionPositionLTR { get; set; }

        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public ICollection<UserStoryDto> UserStories { get; set; }
    }
}

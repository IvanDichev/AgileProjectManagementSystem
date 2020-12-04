using DataModels.Models.WorkItems.UserStory.Dtos;
using System.Collections.Generic;

namespace DataModels.Models.Board.Dtos
{
    public class BoardColumnAllDto
    {
        public int Id { get; set; }

        public string ColumnName { get; set; }

        public ushort MaxItems { get; set; }

        public byte ColumnOrder { get; set; }

        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public ICollection<UserStoryDto> UserStories { get; set; }
    }
}

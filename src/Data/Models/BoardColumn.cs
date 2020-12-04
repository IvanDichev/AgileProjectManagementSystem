using Data.Models.Base;
using System.Collections.Generic;

namespace Data.Models
{
    public class BoardColumn : BaseEntity<int>
    {
        public BoardColumn()
        {
            this.UserStories = new HashSet<UserStory>();
        }

        public string ColumnName { get; set; }
        
        public ushort MaxItems { get; set; }
        
        public byte ColumnOrder { get; set; }
        
        public int ProjectId { get; set; }
        
        public Project Project { get; set; }

        public ICollection<UserStory> UserStories { get; set; }
    }
}
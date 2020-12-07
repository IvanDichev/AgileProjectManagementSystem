using Data.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class KanbanBoardColumnOption : BaseEntity<int>
    {
        [Required]
        [MaxLength(75)]
        public string ColumnName { get; set; }
        
        [Range(1, ushort.MaxValue)]
        public ushort MaxItems { get; set; }

        [Range(1, byte.MaxValue)]
        public byte PositionLTR { get; set; }

        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }
    }
}
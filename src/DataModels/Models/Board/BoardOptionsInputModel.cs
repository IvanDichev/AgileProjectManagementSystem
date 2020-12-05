using DataModels.Models.Board.Dtos;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModels.Models.Board
{
    public class BoardOptionsInputModel
    {
        [Required]
        [MaxLength(75)]
        [Display(Name = "Column Name")]
        public string ColumnName { get; set; }

        [Range(0, ushort.MaxValue)]
        public ushort MaxItems { get; set; }

        [Range(1, byte.MaxValue)]
        [Display(Name = "Position")]
        public byte ColumnOrder { get; set; }

        public int ProjectId { get; set; }

        public ICollection<BoardColumnAllNamePositionDto> AlreadyColumns { get; set; }
    }
}

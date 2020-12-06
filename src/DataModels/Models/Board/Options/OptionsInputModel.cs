using System.ComponentModel.DataAnnotations;

namespace DataModels.Models.Board.Options
{
    public class OptionsInputModel
    {
        [Required]
        [MaxLength(75)]
        public string ColumnName { get; set; }

        [Range(1, ushort.MaxValue)]
        public ushort MaxItems { get; set; }

        [Range(1, byte.MaxValue)]
        public byte PositionLTR { get; set; }
    }
}
